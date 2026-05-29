using System;

namespace ServerProject
{
    class Program
    {
        static void Main(string[] args)
        {
            int portNumber = 2510;
            if (args.Length > 0 && !int.TryParse(args[0], out portNumber))
            {
                AppLogger.Warning("startup", $"Invalid port '{args[0]}'. Falling back to 2510.");
                portNumber = 2510;
            }

            AppLogger.Info("startup", $"Server starting on port {portNumber}");

            Server mainServer = new Server(portNumber);
            mainServer.StartServer();
        }
    }
}
