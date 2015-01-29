using System;
using System.Timers;
using System.Threading;
using System.Collections.ObjectModel;

// Toolkit namespace
using RCS.K2.NFLN.Models;
using RCS.K2.NFLN.Services;
using SimpleMvvmToolkit;
using Timer = System.Timers.Timer;

namespace RCS.K2.NFLN.ViewModels
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// Use the <strong>mvvmprop</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// </summary>
    public class VideoViewModel : ViewModelBase<VideoViewModel>
    {
        // TODO: Add a member for IXxxServiceAgent
        private IVideoServiceAgent _serviceAgent;

        // Default ctor
        public VideoViewModel() { }

        // TODO: ctor that accepts IXxxServiceAgent
        public VideoViewModel(IVideoServiceAgent serviceAgent)
        {
            _serviceAgent = serviceAgent;
            LoadVideos();
            InitTimer();
        }

        // TODO: Add events to notify the view or obtain data from the view
        public event EventHandler<NotificationEventArgs<Exception>> ErrorNotice;

        // TODO: Add properties using the mvvmprop code snippet
        private ObservableCollection<VideoModel> _videos;

        public ObservableCollection<VideoModel> Videos
        {
            get { return _videos; }
            set
            {
                if (_videos == value)
                    return;
                _videos = value;
                NotifyPropertyChanged(m => m.Videos);
            }
        }

        // TODO: Add methods that will be called by the view
        public void LoadVideos()
        {
            Videos = new ObservableCollection<VideoModel>(_serviceAgent.LoadVideos());
        }

        public void InitTimer()
        {
            Timer time = new Timer();
            time.Elapsed += ExecuteTimer;
            time.Interval = 60000; // in miliseconds
            time.Start();
        }

        private void ExecuteTimer(object sender, EventArgs e)
        {
            Console.WriteLine("*** Pinging K2 for new Clips ***");
            LoadVideos();
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