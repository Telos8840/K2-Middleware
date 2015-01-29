using System;
using System.Configuration;
using System.Windows;
using System.Windows.Media;
using Elysium;
using RCS.K2.NFLN.Helpers;
using RCS.K2.NFLN.Models;
using RCS.K2.NFLN.Properties;
using RCS.K2.NFLN.Services;
using RCS.K2.NFLN.UI;

namespace RCS.K2.NFLN
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public VizEngine VizPgm { get; set; }
        public Config TheConfig { get; set; }
        public SelectedUser User { get; set; }
        public SignInWindow SignInWindow { get; set; }
        //private MainWindow mainWindow { get; set; }
        private FeedbackCoordinator _feedback;

        private void StartupHandler(object sender, StartupEventArgs e)
        {
            this.Apply(Theme.Dark, AccentBrushes.Sky, Brushes.White);
            PrepareForTheShow();
        }

        /// <summary>
        /// It creates all the necessary object for the model to be loaded
        /// </summary>
        private void PrepareForTheShow()
        {
            SettingsHelper.InitializeSettings();
            TheConfig = new Config(ConfigurationManager.AppSettings["VizIp"],
                Convert.ToInt32(ConfigurationManager.AppSettings["VizPort"]), 
                "",
                Convert.ToInt32(ConfigurationManager.AppSettings["FeedbackPort"]));
            VizPgm = new VizEngine(TheConfig.EngineIp, TheConfig.EnginePort);
            User = new SelectedUser();
            CurrentSession.Config = TheConfig;
            CurrentSession.VizEngine = VizPgm;
            CurrentSession.User = User;
            CurrentSession.Player = new ClipPlayerServiceAgent();
            _feedback = new FeedbackCoordinator(TheConfig);
            SignInWindow = new SignInWindow();
            //mainWindow = new MainWindow();
        }

        public void Application_Exit(object sender, ExitEventArgs e)
        {
            CurrentSession.Player.EndServerSession();
            _feedback.StopListening();
            Settings.Default.Save();
            VizPgm.DisconnectFromEngine();
            Console.WriteLine(@"ENGINE DISCONNECTED");
        }
    }
}
