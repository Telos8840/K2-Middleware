using System;
using RCS.K2.NFLN.Models;

namespace RCS.K2.NFLN.Helpers
{
    public class FeedbackCoordinator
    {
        private TcpClients Client;
        private FeedbackHandler FeedbackHandler;

        public FeedbackCoordinator(Config theConfig)
        {
            CreateCommands();
            Client = new TcpClients(theConfig.FeedbackPort);
            Client.StartListening();
            Client.TcpMessageReceived += (o, a) => Console.WriteLine(@"TCP MESSAGE RECEIVED");
            FeedbackHandler = new FeedbackHandler();
            Client.TcpMessageReceived += HandleFeedback;
        }

        private void CreateCommands()
        {
            //FeedbackHandler = new FeedbackHandler(Cs);
        }

        public void StopListening()
        {
            Client.Dispose();
        }

        private void HandleFeedback(object sender, TcpMessageReceivedEventArgs args)
        {
            FeedbackHandler.Execute(args);
        }
    }
}
