using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ClientProject
{
    public class CommunicationHandler
    {
        public const int DefaultPort = 2510;
        public const string DefaultServerAddress = "127.0.0.1";
        private static readonly TimeSpan ServerResponseWaitTimeout = TimeSpan.FromSeconds(2);

        private readonly string remoteServerAddress;
        private readonly int communicationPort;
        private Socket? clientSocketToServer;

        public CommunicationHandler(string serverAddress, int port)
        {
            remoteServerAddress = string.IsNullOrWhiteSpace(serverAddress) ? DefaultServerAddress : serverAddress;
            communicationPort = port;
        }

        public void StartCommunication()
        {
            Thread? listenerThread = null;

            try
            {
                clientSocketToServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                AppLogger.Info("connect", $"Connecting to {remoteServerAddress}:{communicationPort}");
                clientSocketToServer.Connect(remoteServerAddress, communicationPort);
                AppLogger.Info("connect", $"Connected to {clientSocketToServer.RemoteEndPoint}");

                listenerThread = new Thread(ListenForMessages)
                {
                    IsBackground = true
                };
                listenerThread.Start();

                AppLogger.Info("input", "Type a message and press Enter. Type 'exit' to close the client.");

                while (true)
                {
                    string? message = Console.ReadLine();
                    if (message is null)
                    {
                        break;
                    }

                    if (string.IsNullOrWhiteSpace(message))
                    {
                        AppLogger.Warning("input", "Ignoring empty message.");
                        continue;
                    }

                    SendMessage(message);

                    if (string.Equals(message, "exit", StringComparison.OrdinalIgnoreCase))
                    {
                        AppLogger.Info("shutdown", "Exit command sent. Closing client.");
                        break;
                    }
                }
            }
            catch (SocketException ex)
            {
                AppLogger.Error("connect", $"Socket error while connecting to the server: {ex.Message}");
            }
            catch (Exception ex)
            {
                AppLogger.Error("client", $"Unexpected client error: {ex.Message}");
            }
            finally
            {
                CloseConnection();
                if (listenerThread?.IsAlive == true)
                {
                    bool listenerJoined = listenerThread.Join(ServerResponseWaitTimeout);
                    if (!listenerJoined)
                    {
                        AppLogger.Warning("shutdown", $"Listener thread did not stop within {ServerResponseWaitTimeout.TotalSeconds} seconds.");
                    }
                }
            }
        }

        private void ListenForMessages()
        {
            if (clientSocketToServer is null)
            {
                return;
            }

            byte[] recMessageBytes = new byte[1024];
            StringBuilder messageBuffer = new StringBuilder();

            try
            {
                while (true)
                {
                    int recMessageBytesLength = clientSocketToServer.Receive(recMessageBytes);
                    if (recMessageBytesLength == 0)
                    {
                        AppLogger.Warning("receive", "Server closed the connection.");
                        break;
                    }

                    messageBuffer.Append(Encoding.UTF8.GetString(recMessageBytes, 0, recMessageBytesLength));

                    string bufferedText = messageBuffer.ToString();
                    string[] messages = bufferedText.Split('\n');

                    for (int i = 0; i < messages.Length - 1; i++)
                    {
                        string receivedMessage = messages[i].Trim();
                        if (!string.IsNullOrEmpty(receivedMessage))
                        {
                            AppLogger.Info("receive", receivedMessage);
                        }
                    }

                    messageBuffer.Clear();
                    if (!string.IsNullOrEmpty(messages[^1]))
                    {
                        messageBuffer.Append(messages[^1]);
                    }
                }
            }
            catch (SocketException ex)
            {
                AppLogger.Error("receive", $"Socket error while receiving data: {ex.Message}");
            }
            catch (ObjectDisposedException)
            {
                AppLogger.Warning("receive", "Receive loop stopped because the socket was disposed.");
            }
        }

        private void SendMessage(string message)
        {
            if (clientSocketToServer is null)
            {
                AppLogger.Warning("send", "Cannot send message because the socket is not connected.");
                return;
            }

            byte[] messageBytes = Encoding.UTF8.GetBytes($"{message}\n");
            clientSocketToServer.Send(messageBytes);
            AppLogger.Info("send", $"Sent message: {message}");
        }

        private void CloseConnection()
        {
            if (clientSocketToServer is null)
            {
                return;
            }

            try
            {
                if (clientSocketToServer.Connected)
                {
                    clientSocketToServer.Shutdown(SocketShutdown.Both);
                }
            }
            catch (SocketException ex)
            {
                AppLogger.Warning("shutdown", $"Socket shutdown warning: {ex.Message}");
            }
            finally
            {
                clientSocketToServer.Close();
                clientSocketToServer.Dispose();
                clientSocketToServer = null;
                AppLogger.Info("shutdown", "Client connection closed.");
            }
        }
    }
}