# Chat TCP Simples em C#

Este projeto implementa um servidor e cliente de chat TCP simples em C# utilizando a biblioteca SuperSimpleTcp.

## Funcionalidades
- Identificação de clientes por nome ou e-mail.
- Mensagens públicas (enviadas para todos os clientes conectados).
- Mensagens privadas entre clientes usando o formato `@destinatario: mensagem`.
- Mensagens do servidor para todos ou para um cliente específico.
- Logs informativos e de depuração no console do servidor.

## Como usar

### Requisitos
- .NET 6.0 ou superior
- Biblioteca [SuperSimpleTcp](https://github.com/jchristn/SuperSimpleTcp)

### Executando o Servidor
1. Compile e execute o projeto do servidor (`Program.cs` na pasta principal).
2. O servidor aguardará conexões na porta 9000 do localhost.
3. O console exibirá logs de conexões, desconexões e mensagens.

### Executando o Cliente
1. Compile e execute o projeto do cliente (`Program.cs` na pasta `tmpc`).
2. Informe seu nome ou e-mail ao iniciar.
3. Para enviar mensagem pública, digite normalmente e pressione ENTER.
4. Para enviar mensagem privada, use o formato:
   ```
   @nome_ou_email: mensagem
   ```
5. Pressione ENTER sem digitar nada para sair.

## Exemplo de uso

- Cliente 1 se identifica como `joao@exemplo.com`
- Cliente 2 se identifica como `maria@exemplo.com`
- Cliente 1 envia:
  - Mensagem pública: `Olá a todos!`
  - Mensagem privada: `@maria@exemplo.com: Olá Maria!`

## Observações
- O servidor identifica cada cliente pela primeira mensagem recebida (nome ou e-mail).
- Mensagens privadas só funcionam se o destinatário já estiver conectado e identificado.
- O servidor não envia mensagens públicas de volta para o remetente.

## Autor
Rafael Rodrigo  
rafael@itprati.ao
