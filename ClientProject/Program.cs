using System;

namespace ClientProject
{
    class Program
    {
        static void Main(string[] args)
        {
            string serverAddress = args.Length > 0 ? args[0] : CommunicationHandler.DefaultServerAddress;
            int port = CommunicationHandler.DefaultPort;

            if (args.Length > 1 && !int.TryParse(args[1], out port))
            {
                AppLogger.Warning("startup", $"Invalid port '{args[1]}'. Falling back to {CommunicationHandler.DefaultPort}.");
                port = CommunicationHandler.DefaultPort;
            }

            AppLogger.Info("startup", $"Client starting for {serverAddress}:{port}");

            CommunicationHandler comHandler = new CommunicationHandler(serverAddress, port);
            comHandler.StartCommunication();
        }
    }
}
