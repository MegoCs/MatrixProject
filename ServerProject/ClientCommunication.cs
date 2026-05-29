using System;
using System.Net.Sockets;
using System.Text;

namespace ServerProject
{
    public class ClientCommunication : CustomThread
    {
        private const string WelcomeMessage = "Hello from the server. Send messages or type 'exit' to disconnect.";
        private readonly Socket clientSocket;

        public ClientCommunication(Socket clintSocket)
        {
            clientSocket = clintSocket;
        }

        public override void RunThread()
        {
            string clientName = clientSocket.RemoteEndPoint?.ToString() ?? "unknown-client";
            byte[] buffer = new byte[1024];
            StringBuilder messageBuffer = new StringBuilder();

            try
            {
                AppLogger.Info("client-session", $"Started session for {clientName}");
                SendMessage(WelcomeMessage);

                while (true)
                {
                    int receivedBytesLength = clientSocket.Receive(buffer);
                    if (receivedBytesLength == 0)
                    {
                        AppLogger.Warning("client-session", $"Client {clientName} disconnected.");
                        break;
                    }

                    messageBuffer.Append(Encoding.UTF8.GetString(buffer, 0, receivedBytesLength));
                    string bufferedText = messageBuffer.ToString();
                    string[] messages = bufferedText.Split('\n');

                    for (int i = 0; i < messages.Length - 1; i++)
                    {
                        string clientMessage = messages[i].Trim();
                        if (string.IsNullOrEmpty(clientMessage))
                        {
                            continue;
                        }

                        AppLogger.Info("receive", $"Received from {clientName}: {clientMessage}");

                        if (string.Equals(clientMessage, "exit", StringComparison.OrdinalIgnoreCase))
                        {
                            SendMessage("Goodbye from server.");
                            AppLogger.Info("client-session", $"Closing session for {clientName} on exit request.");
                            return;
                        }

                        SendMessage($"Server received: {clientMessage}");
                    }

                    messageBuffer.Clear();
                    messageBuffer.Append(messages[^1]);
                }
            }
            catch (SocketException ex)
            {
                AppLogger.Error("client-session", $"Socket error for {clientName}: {ex.Message}");
            }
            catch (Exception ex)
            {
                AppLogger.Error("client-session", $"Unexpected error for {clientName}: {ex.Message}");
            }
            finally
            {
                try
                {
                    if (clientSocket.Connected)
                    {
                        clientSocket.Shutdown(SocketShutdown.Both);
                    }
                }
                catch (SocketException ex)
                {
                    AppLogger.Warning("client-session", $"Shutdown warning for {clientName}: {ex.Message}");
                }
                finally
                {
                    clientSocket.Close();
                    clientSocket.Dispose();
                    AppLogger.Info("client-session", $"Session closed for {clientName}");
                }
            }
        }

        private void SendMessage(string message)
        {
            clientSocket.Send(Encoding.UTF8.GetBytes($"{message}\n"));
            AppLogger.Info("send", $"Sent to {clientSocket.RemoteEndPoint}: {message}");
        }
    }
}