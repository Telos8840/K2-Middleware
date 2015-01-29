using System;
using RCS.K2.NFLN.Models;
using RCS.K2.NFLN.Services;
using SimpleMvvmToolkit;

namespace RCS.K2.NFLN.ViewModels
{
    /// <summary>
    /// This class contains properties that a View can databind to.
    /// </summary>
    public class ConfigViewModel : ViewModelDetailBase<ConfigViewModel, Config>
    {
        private IConfigServiceAgent _serviceAgent;

        // Default ctor
        public ConfigViewModel() { }

        public ConfigViewModel(IConfigServiceAgent serviceAgent)
        {
            _serviceAgent = serviceAgent;
            GetConfig();
        }

        // TODO: Add events to notify the view or obtain data from the view
        public event EventHandler<NotificationEventArgs<Exception>> ErrorNotice;

        #region Properties

        private Config _config;

        public Config Config
        {
            get { return _config; }
            set
            {
                if (_config == value)
                    return;
                _config = value;
                NotifyPropertyChanged(m => m.Config);
            }
        }

        #endregion

        // Methods that will be called by the view
        #region Public Functions

        public void LoadScene()
        {
            _serviceAgent.LoadScene();
        }

        public void CloseConfig()
        {
            _serviceAgent.CloseConfig();
        }

        private void GetConfig()
        {
            Config = CurrentSession.Config;
        }

        #endregion

        // TODO: Optionally add callback methods for async calls to the service agent

        // Helper method to notify View of an error
        private void NotifyError(string message, Exception error)
        {
            // Notify view of an error
            Notify(ErrorNotice, new NotificationEventArgs<Exception>(message, error));
        }
    }
}