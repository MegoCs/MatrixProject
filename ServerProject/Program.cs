using System;

namespace ServerProject
{
    class Program
    {
        static void Main(string[] args)
        {
            int PORTNUMBER=2510;
             Server _mainServer = new Server(PORTNUMBER);
             _mainServer.StartServer();
             
        }
    }
}
