
using System;
using System.Net.Sockets;
using System.Text;

namespace ServerProject{
    public class ClientCommunication : CustomThread
    {
        const string  WELCOMEMESSAGE ="Hello Server, Client Is Conected"; 
        Socket clientSocket ;
        public ClientCommunication(Socket _clintSocket){
             clientSocket=_clintSocket;
        }
        public override void RunThread()
        {
            //Sending a WELCOMEMESSAGE To The Client
            while (true)
            {
                            clientSocket.Send(Encoding.ASCII.GetBytes(WELCOMEMESSAGE));
                            clientSocket.Send(Encoding.ASCII.GetBytes(Console.ReadLine()));
            }
        
        }
    }

}