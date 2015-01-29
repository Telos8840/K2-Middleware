using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Threading;
using System.Collections.ObjectModel;

// Toolkit namespace
using NflnInteractive.Lookup.Entities;
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
    public class SignInViewModel : ViewModelBase<SignInViewModel>
    {
        // TODO: Add a member for IXxxServiceAgent
        private ISignInServiceAgent _serviceAgent;

        // Default ctor
        public SignInViewModel() { }

        // TODO: ctor that accepts IXxxServiceAgent
        public SignInViewModel(ISignInServiceAgent serviceAgent)
        {
            _serviceAgent = serviceAgent;
            LoadUsers();
            IsActive = false;
        }

        // TODO: Add events to notify the view or obtain data from the view
        public event EventHandler<NotificationEventArgs<Exception>> ErrorNotice;

        // TODO: Add properties using the mvvmprop code snippet
        private ObservableCollection<string> _users;

        public ObservableCollection<string> Users
        {
            get { return _users; }
            set
            {
                if (_users == value)
                    return;
                _users = value;
                NotifyPropertyChanged(m => m.Users);
            }
        }

        private string _selectedUser;

        public string SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                if (_selectedUser == value)
                    return;
                _selectedUser = value;
                NotifyPropertyChanged(m => m.SelectedUser);
            }
        }

        private Boolean _isActive;

        public Boolean IsActive
        {
            get { return _isActive; }
            set
            {
                if (_isActive == value)
                    return;
                _isActive = value;
                NotifyPropertyChanged(m => m.IsActive);
            }
        }

        // TODO: Add methods that will be called by the view
        public void SelectUser()
        {
            if (String.IsNullOrEmpty(SelectedUser))
            {
                IsActive = false;
                return;
            } 
            
            IsActive = true;

            try
            {
                using (var sql = new NFLIntDevEntities())
                {
                    CurrentSession.User.ID = sql.UserProfiles.Single(u => u.UserName.Equals(SelectedUser)).UserId;
                    CurrentSession.User.Name = SelectedUser;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void StartApp()
        {
            var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
            window.Close();

            var mainWindow = new MainWindow();
            mainWindow.Show();
        }
        
        private void LoadUsers()
        {
            var users = _serviceAgent.LoadUsers();
            Users = new ObservableCollection<string>(users);
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