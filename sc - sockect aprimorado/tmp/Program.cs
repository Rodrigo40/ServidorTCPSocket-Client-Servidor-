using System;
using System.Collections.Generic;
using SuperSimpleTcp;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Servidor iniciado por Rafael Rodrigo | rafael@itprati.ao");

        SimpleTcpServer server = new SimpleTcpServer("127.0.0.1:9000");
        var clientNames = new Dictionary<string, string>(); // IpPort -> Nome

        server.Events.ClientConnected += (sender, e) =>
        {
            Console.WriteLine($"[INFO] Cliente conectado: {e.IpPort}");
        };

        server.Events.ClientDisconnected += (sender, e) =>
        {
            string nome = clientNames.ContainsKey(e.IpPort) ? clientNames[e.IpPort] : e.IpPort;
            Console.WriteLine($"[INFO] Cliente desconectado: {nome}");
            clientNames.Remove(e.IpPort);
        };

        server.Events.DataReceived += (sender, e) =>
        {
            string msg = System.Text.Encoding.UTF8.GetString(e.Data).Trim();
            Console.WriteLine($"[DEBUG] Mensagem recebida de {e.IpPort}: {msg}");

            // Se cliente ainda não se identificou, assume que é o nome/email
            if (!clientNames.ContainsKey(e.IpPort))
            {
                clientNames[e.IpPort] = msg;
                Console.WriteLine($"[INFO] Cliente {e.IpPort} identificado como: {msg}");
                return;
            }

            string remetente = clientNames[e.IpPort];

            // Mensagem privada com @nome: mensagem
            if (msg.StartsWith("@"))
            {
                int idx = msg.IndexOf(":");
                if (idx > 1)
                {
                    string destinatario = msg.Substring(1, idx - 1).Trim();
                    string conteudo = msg.Substring(idx + 1).Trim();

                    Console.WriteLine($"[DEBUG] Procurando destinatário: {destinatario}");
                    Console.WriteLine("[DEBUG] Lista de clientes registrados:");
                    foreach (var kvp in clientNames)
                        Console.WriteLine($"- {kvp.Value} ({kvp.Key})");

                    string destIpPort = null;
                    foreach (var kvp in clientNames)
                    {
                        if (kvp.Value.Trim().Equals(destinatario, StringComparison.OrdinalIgnoreCase))
                        {
                            destIpPort = kvp.Key;
                            break;
                        }
                    }

                    if (destIpPort != null)
                    {
                        server.Send(destIpPort, $"Mensagem privada de {remetente}: {conteudo}");
                        Console.WriteLine($"[INFO] Mensagem privada de {remetente} para {destinatario}: {conteudo}");
                    }
                    else
                    {
                        server.Send(e.IpPort, $"[ERRO] Destinatário '{destinatario}' não encontrado.");
                        Console.WriteLine($"[WARN] Destinatário '{destinatario}' não encontrado.");
                    }
                }
                return;
            }

            // Mensagem pública
            Console.WriteLine($"[INFO] Mensagem pública de {remetente}: {msg}");
            foreach (var client in server.GetClients())
            {
                if (client != e.IpPort) // Evita mandar para si mesmo
                {
                    server.Send(client, $"{remetente} (público): {msg}");
                }
            }
        };

        server.Start();
        Console.WriteLine("[SERVIDOR] Pronto para receber conexões em 127.0.0.1:9000");

        Console.WriteLine("Digite ENTER vazio para encerrar o servidor.");
        while (!string.IsNullOrEmpty(Console.ReadLine())) { }

        server.Stop();
        Console.WriteLine("Servidor encerrado.");
    }
}
