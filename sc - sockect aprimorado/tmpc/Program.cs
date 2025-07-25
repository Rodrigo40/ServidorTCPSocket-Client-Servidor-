using System;
using SuperSimpleTcp;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Cliente iniciado por Rafael Rodrigo | rafael@itprati.ao");

        Console.Write("Informe seu nome ou e-mail: ");
        string? identificacao = Console.ReadLine()?.Trim();

        if (string.IsNullOrWhiteSpace(identificacao))
        {
            Console.WriteLine("[ERRO] Nome ou e-mail é obrigatório. Encerrando.");
            return;
        }

        using (SimpleTcpClient client = new SimpleTcpClient("127.0.0.1:9000"))
        {
            client.Events.Connected += (sender, e) =>
            {
                Console.WriteLine("[INFO] Conectado ao servidor.");
                client.Send(identificacao); // Envia identificação logo ao conectar
            };

            client.Events.Disconnected += (sender, e) =>
            {
                Console.WriteLine("[INFO] Desconectado do servidor.");
            };

            client.Events.DataReceived += (sender, e) =>
            {
                string msg = System.Text.Encoding.UTF8.GetString(e.Data);
                Console.WriteLine($"[RECEBIDO] {msg}");
            };

            try
            {
                client.Connect();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERRO] Falha ao conectar: {ex.Message}");
                return;
            }

            Console.WriteLine("\nDigite mensagens normalmente.");
            Console.WriteLine("Para mensagens privadas, use: @destinatario: mensagem");
            Console.WriteLine("Exemplo: @joao: Olá João!\n");

            string? input;
            while (!string.IsNullOrEmpty(input = Console.ReadLine()))
            {
                client.Send(input.Trim());
            }

            client.Disconnect();
        }

        Console.WriteLine("Cliente encerrado.");
    }
}
