using System;
using System.Net;
using System.Net.Sockets;

namespace ServerProject
{
    public class Server
    {
        private readonly IPEndPoint localEndPoint;
        private readonly int portNum;
        private readonly Socket sSocket;

        public void StartServer()
        {
            AppLogger.Info("startup", $"Binding server socket on {localEndPoint}");
            sSocket.Bind(localEndPoint);
            sSocket.Listen(10);
            AppLogger.Info("startup", $"Server listening on {localEndPoint}");

            while (true)
            {
                Socket clientSocket = sSocket.Accept();
                AppLogger.Info("accept", $"Accepted client {clientSocket.RemoteEndPoint}");
                ClientCommunication handler = new ClientCommunication(clientSocket);
                handler.Start();
            }
        }

        public Server(int port)
        {
            portNum = port;
            localEndPoint = new IPEndPoint(System.Net.IPAddress.Any, portNum);

            sSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
    }
}
