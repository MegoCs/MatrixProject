using System;
using System.Net;
using System.Net.Sockets;

namespace ServerProject
{
    public class Server
    {
        IPHostEntry ipHostInfo ;  
        IPAddress ipAddress ;  
        IPEndPoint localEndPoint ;
        int portNum ;
        Socket sSocket;
        public void StartServer() {
            Console.WriteLine("Server Thread Started @Port : " + portNum);
            sSocket.Bind(localEndPoint);  
            Console.WriteLine(localEndPoint.ToString());
            sSocket.Listen(1);
            while (true)
            {
                Socket _clientSocket=sSocket.Accept();
                ClientCommunication handler = new ClientCommunication(_clientSocket);
                handler.RunThread();
            }          
        }
        public Server(int _port){

            portNum=_port;
            // Establish the local endpoint for the socket.  
            // Dns.GetHostName returns the name of the   
            // host running the application.     
            ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());  
            ipAddress = ipHostInfo.AddressList[0];  
            localEndPoint = new IPEndPoint(ipAddress, portNum);  

            // Create a TCP/IP socket.  
         sSocket = new Socket(ipAddress.AddressFamily,  
            SocketType.Stream, ProtocolType.Tcp );  
        }
    }
}
