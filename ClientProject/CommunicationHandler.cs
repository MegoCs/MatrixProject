using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ClientProject{
    public class CommunicationHandler{
        IPHostEntry ipHostInfo ;  
        IPAddress ipAddress ;  
        IPEndPoint remoteEP ;
        Socket clientSocketToServer;
        int COMMUNICATIONPORT=2510;
        string REMOTESERVERIP="fe80::7cea:448b:8c61:52a9%8";
        public CommunicationHandler(){
            // Establish the remote endpoint for the socket.  
            ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());  
            ipAddress = IPAddress.Parse(REMOTESERVERIP); 
            remoteEP = new IPEndPoint(ipAddress,COMMUNICATIONPORT);  

            // Create a TCP/IP  socket.  
            clientSocketToServer = new Socket(ipAddress.AddressFamily,   
            SocketType.Stream, ProtocolType.Tcp );    
        }
        public void StartCommunication(){
            try
            {
                clientSocketToServer.Connect(remoteEP);  
                Console.WriteLine("Socket connected to {0}",clientSocketToServer.RemoteEndPoint.ToString());      
                // Start The Real Opreation With the server.
                while (true)
                {
                    byte[] recMessageBytes = new byte[1024]; 
                    int recMessageBytesLeng= clientSocketToServer.Receive(recMessageBytes);
                    Console.WriteLine(Encoding.ASCII.GetString(recMessageBytes,0,recMessageBytesLeng));       
                }
            }
            catch (Exception)
            {
                
            }
        }
    }
}