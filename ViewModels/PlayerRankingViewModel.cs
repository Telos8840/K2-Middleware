using System;
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
    public class PlayerRankingViewModel : ViewModelBase<PlayerRankingViewModel>
    {
        // TODO: Add a member for IXxxServiceAgent
        private IRankingServiceAgent _serviceAgent;

        // Default ctor
        public PlayerRankingViewModel() { }

        // TODO: ctor that accepts IXxxServiceAgent
        public PlayerRankingViewModel(IRankingServiceAgent serviceAgent)
        {
            _serviceAgent = serviceAgent;
            LoadSegments();
        }

        // TODO: Add events to notify the view or obtain data from the view
        public event EventHandler<NotificationEventArgs<Exception>> ErrorNotice;

        // TODO: Add properties using the mvvmprop code snippet
        private ObservableCollection<Segments> _segments;

        public ObservableCollection<Segments> Segments
        {
            get { return _segments; }
            set
            {
                if (_segments == value)
                    return;
                _segments = value;
                NotifyPropertyChanged(m => m.Segments);
            }
        }

        // TODO: Add methods that will be called by the view
        public void LoadSegments()
        {
            var segs = _serviceAgent.LoadSegments();
            Segments = new ObservableCollection<Segments>(segs);
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