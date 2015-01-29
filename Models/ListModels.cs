using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Accessibility;
using GrassValley.Mseries.AppServer;
using GrassValley.Mseries.MediaMgr;
using Newtonsoft.Json;
using NflnInteractive.Lookup.Entities;
using RCS.K2.NFLN.UI;
using SimpleMvvmToolkit;

namespace RCS.K2.NFLN.Models
{
    public enum SegmentType
    {
        MockDraft = 0,
        PlayerRanking = 1,
        FreeAgency = 2
    }

    public class SelectedUser : ModelBase<SelectedUser>
    {
        private int _iD;

        public int ID
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
    }

    public class Segments : ModelBase<Segments>
    {
        private int _iD;

        public int ID
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

        private string _title;

        public string Title
        {
            get { return _title; }
            set
            {
                if (_title == value)
                    return;
                _title = value;
                NotifyPropertyChanged(m => m.Title);
            }
        }

        public void LoadTables()
        {
            try
            {
                using (var sql = new NFLIntDevEntities())
                {
                    var pRanking = sql.PlayerRankings.Single(s => s.SID == ID);

                    int type = (int)pRanking.Type;
                    if (type == 0)
                    {
                        CurrentSession.ListData = JsonConvert.DeserializeObject<List<NomineePods>>(pRanking.Data).Where(n => !String.IsNullOrEmpty(n.Tricode)).ToList();
                        CurrentSession.OverallData = null;
                    }
                    else
                    {
                        CurrentSession.OverallData = JsonConvert.DeserializeObject<List<MockDraft>>(pRanking.Data)
                            .Where(n => n.ListData != null)
                            .ToList();
                        CurrentSession.ListData = null;
                    }

                    var playerWindow = new PlayerEditWindow();
                    playerWindow.Show();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }

    public class PlayerInfo : ModelBase<PlayerInfo>
    {
        private string _team;

        public string Team
        {
            get { return _team; }
            set
            {
                if (_team == value)
                    return;
                _team = value;
                NotifyPropertyChanged(m => m.Team);
            }
        }

        private string _player;

        public string Player
        {
            get { return _player; }
            set
            {
                if (_player == value)
                    return;
                _player = value;
                NotifyPropertyChanged(m => m.Player);
            }
        }
    }

    public class NomineePods
    {
        public string ID { get; set; }
        public string Position { get; set; }
        public string Tricode { get; set; }
        public string TeamName { get; set; }
        public string Name { get; set; }

        public void LoadVideos()
        {
            try
            {
                var videoServer = new AppServerMgrProxy();
                //IAppServer iappServer = videoServer.CreateAppServer("","")
                //IMediaMgr mediaMgr = videoServer

                videoServer.SetHost("10.0.2.53");
                videoServer.SetUserCredentials("administrator", "adminK2", "", false);

                if (videoServer.Connect())
                {
                    Console.WriteLine("CONNECTED!!");
                }
                else
                {
                    Console.WriteLine("TOTAL FAIL =/");
                }

                videoServer.Disconnect();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }

    public class MockDraft
    {
        public string Position { get; set; }
        public List<NomineePods> ListData { get; set; }
    }
}