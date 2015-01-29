using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Threading;
using System.Collections.ObjectModel;

// Toolkit namespace
using RCS.K2.NFLN.Models;
using RCS.K2.NFLN.Services;
using SimpleMvvmToolkit;

namespace RCS.K2.NFLN.ViewModels
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// Use the <strong>mvvmprop</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// </summary>
    public class PlayerEditViewModel : ViewModelBase<PlayerEditViewModel>
    {
        // TODO: Add a member for IXxxServiceAgent
        private IPlayerEditServiceAgent _serviceAgent;

        // Default ctor
        public PlayerEditViewModel() { }

        // TODO: ctor that accepts IXxxServiceAgent
        public PlayerEditViewModel(IPlayerEditServiceAgent serviceAgent)
        {
            _serviceAgent = serviceAgent;

            if (CurrentSession.ListData != null)
                LoadListData();
            else if (CurrentSession.OverallData != null)
                LoadOverallData();
        }

        // TODO: Add events to notify the view or obtain data from the view
        public event EventHandler<NotificationEventArgs<Exception>> ErrorNotice;

        // TODO: Add properties using the mvvmprop code snippet
        private ObservableCollection<NomineePods> _listData;

        public ObservableCollection<NomineePods> ListData
        {
            get { return _listData; }
            set
            {
                if (_listData == value)
                    return;
                _listData = value;
                NotifyPropertyChanged(m => m.ListData);
            }
        }

        // TODO: Add methods that will be called by the view
        private void LoadListData()
        {
            ListData = new ObservableCollection<NomineePods>(_serviceAgent.LoadListData());
        }

        private void LoadOverallData()
        {
            ListData = new ObservableCollection<NomineePods>();

            foreach (var player in _serviceAgent.LoadOverallData().SelectMany(list => list.ListData))
                ListData.Add(player);
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