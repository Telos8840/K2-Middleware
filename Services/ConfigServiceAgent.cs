using System;
using System.Threading;
using RCS.K2.NFLN.Helpers;
using RCS.K2.NFLN.Models;

namespace RCS.K2.NFLN.Services
{
    public class ConfigServiceAgent : IConfigServiceAgent
    {
        public ConfigServiceAgent()
        {

        }

        public void LoadScene()
        {
            if (!CurrentSession.VizEngine.Connected)
                ConnectEngine();

            try
            {
                if (CurrentSession.VizEngine != null && CurrentSession.VizEngine.Connected)
                {
                    string strScenePath;
                    if (CurrentSession.VizEngine.Connected)
                    {
                        strScenePath = CurrentSession.Config.ScenePath;

                        //if (!IsSceneInMainLayer(strScenePath) )
                        //{
                        //clear renderer
                        CurrentSession.VizEngine.SendToEngine("-1 RENDERER*FRONT_LAYER SET_OBJECT ");
                        CurrentSession.VizEngine.SendToEngine("-1 RENDERER*MAIN_LAYER SET_OBJECT ");
                        CurrentSession.VizEngine.SendToEngine("-1 RENDERER*BACK_LAYER SET_OBJECT ");
                        CurrentSession.VizEngine.SendToEngine("-1 SCENE CLEANUP ");
                        Thread.Sleep(500);

                        //set scene on middle layer
                        CurrentSession.VizEngine.SendToEngine("-1 RENDERER SET_OBJECT SCENE*" + strScenePath);

                        //display on console
                        Console.WriteLine("LOAD SCENE");
                        Console.WriteLine("SENT TO ENGINE: 1 RENDERER SET_OBJECT SCENE*" + strScenePath);
                        //}
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: " + e.Message);
            }
        }

        public void CloseConfig()
        {

        }

        public void ConnectEngine()
        {
            CurrentSession.VizEngine.IP = CurrentSession.Config.EngineIp;
            CurrentSession.VizEngine.Port = CurrentSession.Config.EnginePort;
            CurrentSession.VizEngine.ConnectToEngine();

            Thread t = new Thread(o => setEndPoint());
            t.Start();
            //SetTCPCallbackEndpoint
        }

        private void setEndPoint()
        {
            bool done = true;
            while (done)
            {
                if (CurrentSession.VizEngine.Connected)
                {
                    CurrentSession.VizEngine.Invoke("SetTCPCallbackEndpoint \""
                        + NetHelper.GetLocalIP() + "\" \"" + CurrentSession.Config.FeedbackPort + "\"");
                    done = false;
                    Console.WriteLine(" \n*** VIZPGM CONNECTED *** \n");
                }

            }
        }

        private bool IsSceneInMainLayer(string fullScenePath)
        {
            string path = CurrentSession.VizEngine.SendToEngineResponse("-1 RENDERER*MAIN_LAYER*LOCATION_PATH GET").Replace("\0", "").Trim();
            string path2 = fullScenePath;
            bool check = String.Equals(path, path2);
            return String.Equals(path, fullScenePath.ToLower());
        }
    }
}