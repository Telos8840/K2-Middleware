using System;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using RCS.K2.NFLN.Helpers;
using RCS.K2.NFLN.Models;
using RCS.NFLN.SCHEDULE;
using SimpleMvvmToolkit;

namespace RCS.K2.NFLN
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// Use the <strong>mvvmprop</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// </summary>
    public class MainPageViewModel : ViewModelBase<MainPageViewModel>
    {
        #region Private Fields

        private Boolean _check;
        private string connectStr;

        #endregion

        // Default ctor
        public MainPageViewModel()
        {
            CurrentSession.VizEngine.EngineStatus += UpdateToggle;
            ConnectViz();
        }

        // TODO: Add events to notify the view or obtain data from the view
        public event EventHandler<NotificationEventArgs<Exception>> ErrorNotice;
        // Add properties using the mvvmprop code snippet
        private string _bannerText = "Hello Simple MVVM Toolkit";
        public string BannerText
        {
            get
            {
                if (this.IsInDesignMode()) return "Banner";
                return _bannerText;
            }
            set
            {
                _bannerText = value;
                NotifyPropertyChanged(m => m.BannerText);
            }
        }

        private Boolean _isVizConnected;

        public Boolean IsVizConnected
        {
            get { return _isVizConnected; }
            set
            {
                if (_isVizConnected == value)
                    return;
                _isVizConnected = value;
                NotifyPropertyChanged(m => m.IsVizConnected);
            }
        }

        private Boolean _isConsoleOn;

        public Boolean IsConsoleOn
        {
            get { return _isConsoleOn; }
            set
            {
                if (_isConsoleOn == value)
                    return;
                _isConsoleOn = value;
                NotifyPropertyChanged(m => m.IsConsoleOn);
            }
        }

        private string _conString;

        public string ConString
        {
            get { return _conString; }
            set
            {
                if (_conString == value)
                    return;
                _conString = value;
                NotifyPropertyChanged(m => m.ConString);
            }
        }

        private SolidColorBrush _color;

        public SolidColorBrush Color
        {
            get { return _color; }
            set
            {
                if (_color == value)
                    return;
                _color = value;
                NotifyPropertyChanged(m => m.Color);
            }
        }

        // Methods that will be called by the view
        private void UpdateToggle(string status)
        {
            IsVizConnected = status.Equals("Connected");

            if (IsVizConnected)
            {
                ConString = "Connected";
                Color = Brushes.Green;
            }
            else
            {
                ConString = "Disconnected";
                Color = Brushes.Red;
            }
        }

        public void ConnectViz()
        {
            //if (!_check)
            //{
                CurrentSession.VizEngine.IP = CurrentSession.Config.EngineIp;
                CurrentSession.VizEngine.Port = CurrentSession.Config.EnginePort;
                CurrentSession.VizEngine.ConnectToEngine();
                Thread t = new Thread(o => setEndPoint());
                t.Start();
            //}
        }

        public void DisconnectViz()
        {
            if (_check && connectStr.Equals("Connected"))
            {
                CurrentSession.VizEngine.DisconnectFromEngine();
                _check = false;
            }
        }

        private void setEndPoint()
        {
            bool done = true;
            while (done)
            {
                if (CurrentSession.VizEngine.Connected)
                {
                    CurrentSession.VizEngine.Invoke("SetTCPCallbackEndpoint \"" + NetHelper.GetLocalIP() + "\" \"" + CurrentSession.Config.FeedbackPort + "\"");
                    done = false;
                    _check = true;
                    Console.WriteLine(" \n*** VIZPGM CONNECTED *** \n");
                    connectStr = "Connected";
                }
            }
        }

        public void TurnOnConsole()
        {
            IsConsoleOn = true;
            ConsoleManager.Show();
        }

        public void TurnOffConsole()
        {
            IsConsoleOn = false;
            ConsoleManager.Hide();
        }

        // TODO: Optionally add callback methods for async calls to the service agent

        // Helper method to notify View of an error
        private void NotifyError(string message, Exception error)
        {
            // Notify view of an error
            Notify(ErrorNotice, new NotificationEventArgs<Exception>(message, error));
        }
    }
}