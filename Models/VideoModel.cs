using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleMvvmToolkit;

namespace RCS.K2.NFLN.Models
{
    public class VideoModel : ModelBase<VideoModel>
    {
        private string _iD;

        public string ID
        {
            get { return _iD; }
            set
            {
                if (_iD == value)
                    return;
                _iD = value;
                NotifyPropertyChanged(m => m.ID);
            }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name == value)
                    return;
                _name = value;
                NotifyPropertyChanged(m => m.Name);
            }
        }

        private string _path;

        public string Path
        {
            get { return _path; }
            set
            {
                if (_path == value)
                    return;
                _path = value;
                NotifyPropertyChanged(m => m.Path);
            }
        }

        private string _timeStamp;

        public string TimeStamp
        {
            get { return _timeStamp; }
            set
            {
                if (_timeStamp == value)
                    return;
                _timeStamp = value;
                NotifyPropertyChanged(m => m.TimeStamp);
            }
        }

        private DateTime _created;

        public DateTime Created
        {
            get { return _created; }
            set
            {
                if (_created == value)
                    return;
                _created = value;
                NotifyPropertyChanged(m => m.Created);
            }
        }
    }
}