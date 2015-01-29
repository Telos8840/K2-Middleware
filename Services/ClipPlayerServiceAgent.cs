using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrassValley.Mseries.AppServer;
using GrassValley.Mseries.Control;
using GrassValley.Mseries.MediaMgr;
using NflnInteractive.Lookup.Entities;
using RCS.K2.NFLN.Models;

namespace RCS.K2.NFLN.Services
{
    public class ClipPlayerServiceAgent : IClipPlayerServiceAgent
    {
        private ISimpleController icontroller;
        private ISimplePlayerRecorder player;
        private IAppServer iappServer;
        private IMediaMgr mediaMgr;
        private AppServerMgrProxy videoServer;
        private string appName;
        private string client;
        private int count;
        private int cookie;
        private List<VideoClip> Videos;

        public ClipPlayerServiceAgent()
        {
            videoServer = new AppServerMgrProxy();
            videoServer.SetHost(ConfigurationManager.AppSettings["K2Ip"]);
            videoServer.SetUserCredentials(ConfigurationManager.AppSettings["K2UserName"],
                ConfigurationManager.AppSettings["K2Password"], "", false);

            if (!videoServer.Connect())
            {
                Console.WriteLine("*** Cannot connect to the K2 ***");
                return;
            }

            appName = "RCS.K2.NFLN";
            client = "Client 1";

            bool newConnection = false;
            iappServer = videoServer.CreateAppServer(appName, client, out newConnection);
            
            bool isNewController = false;
            icontroller = iappServer.CreateController(appName, "C1", out isNewController);
            player = (ISimplePlayerRecorder)icontroller;
            
            using (var sql = new NFLLookupEntities())
            {
                Videos = new List<VideoClip>(sql.VideoClips);
            }
        }

        public void LoadClip(string id)
        {
            using (var sql = new NFLLookupEntities())
            {
                Videos = new List<VideoClip>(sql.VideoClips);
            }

            var videoClip = Videos.SingleOrDefault(i => i.VID.Equals(id));
            if (videoClip != null)
            {
                player.Load("edl/cmf//local/" + videoClip.Path);
                player.CueStart();                
            }
        }

        public void PlayClip()
        {
            player.LoopPlayMode = false;
            player.Play();
        }

        public void PauseClip()
        {
            player.Stop();
        }

        public void ResetClip()
        {
            player.CueStart();
        }

        public void EjectClip()
        {
            player.Eject();
        }

        public void ConfirmDone(string message)
        {
            CurrentSession.VizEngine.Invoke("telestration_k2Receive \"" + message + "\"");
        }

        public void EndServerSession()
        {
            icontroller.CloseChannel();
            iappServer.CloseConnection();
            videoServer.Disconnect();
        }

        public void CreateMediaMgr()
        {
            mediaMgr = iappServer.CreateMediaMgr(appName);

            count = 0;
            cookie = mediaMgr.EnumerateAssets("V:", "default", ref count);
        }

        public object[] GetAssets(string query)
        {
            int maxCount = 999;
            object dataObj = null;

            mediaMgr.GetResultProperty(cookie, query, 0, maxCount, out count, out dataObj);

            object[] clipArray = (object[])dataObj;

            return clipArray;
        }

        public void EndMediaSession()
        {
            mediaMgr.CloseResults(cookie);
            mediaMgr.Dispose();
        }
    }
}