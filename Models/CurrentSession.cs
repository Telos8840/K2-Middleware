using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows.Documents;
using RCS.K2.NFLN.Helpers;
using RCS.K2.NFLN.Services;

namespace RCS.K2.NFLN.Models
{
    public static class CurrentSession
    {
        private static Config _config;

        public static Config Config
        {
            get { return _config; }
            set
            {
                if (_config == value)
                    return;
                _config = value;
            }
        }

        private static VizEngine _vizEngine;

        public static VizEngine VizEngine
        {
            get { return _vizEngine; }
            set
            {
                if (_vizEngine == value)
                    return;
                _vizEngine = value;
            }
        }

        private static string _localIp;

        public static string LocalIp
        {
            get
            {
                if (_localIp == null)
                    _localIp = GetLocalIp();
                return _localIp;
            }
            set
            {
                if (_localIp == value)
                    return;
                _localIp = value;
            }
        }

        private static SelectedUser _user;

        public static SelectedUser User
        {
            get { return _user; }
            set
            {
                if (_user == value)
                    return;
                _user = value;
            }
        }

        private static List<NomineePods> _listData;

        public static List<NomineePods> ListData
        {
            get { return _listData; }
            set
            {
                if (_listData == value)
                    return;
                _listData = value;
            }
        }

        private static List<MockDraft> _overallData;

        public static List<MockDraft> OverallData
        {
            get { return _overallData; }
            set
            {
                if (_overallData == value)
                    return;
                _overallData = value;
            }
        }

        private static IClipPlayerServiceAgent _player;

        public static IClipPlayerServiceAgent Player
        {
            get { return _player; }
            set
            {
                if (_player == value)
                    return;
                _player = value;
            }
        }


        private static string GetLocalIp()
        {
            string sHostName = Dns.GetHostName();
            IPHostEntry ipE = Dns.GetHostByName(sHostName);
            IPAddress[] IpA = ipE.AddressList;
            return IpA.Count() > 0 ? IpA[0].ToString() : "";
        }
    }
}
