using SimpleMvvmToolkit;

namespace RCS.K2.NFLN.Models
{
    public class Config : ModelBase<Config>
    {
        private string _engineIp;

        public string EngineIp
        {
            get { return _engineIp; }
            set
            {
                if (_engineIp == value)
                    return;
                _engineIp = value;
                NotifyPropertyChanged(m => m.EngineIp);
            }
        }

        private int _enginePort;

        public int EnginePort
        {
            get { return _enginePort; }
            set
            {
                if (_enginePort == value)
                    return;
                _enginePort = value;
                NotifyPropertyChanged(m => m.EnginePort);
            }
        }

        private string _scenePath;

        public string ScenePath
        {
            get { return _scenePath; }
            set
            {
                if (_scenePath == value)
                    return;
                _scenePath = value;
                NotifyPropertyChanged(m => m.ScenePath);
            }
        }

        private int _feedbackPort;

        public int FeedbackPort
        {
            get { return _feedbackPort; }
            set
            {
                if (_feedbackPort == value)
                    return;
                _feedbackPort = value;
                NotifyPropertyChanged(m => m.FeedbackPort);
            }
        }

        public Config(string engineIp, int enginePort, string scenePath, int feedbackPort)
        {
            _engineIp = engineIp;
            _enginePort = enginePort;
            _scenePath = scenePath;
            _feedbackPort = feedbackPort;
        }

        public Config()
        {

        }
    }
}