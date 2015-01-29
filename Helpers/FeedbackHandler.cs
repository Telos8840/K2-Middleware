using System;
using System.Collections.Generic;
using System.Linq;
using RCS.K2.NFLN.Models;
using RCS.K2.NFLN.Services;

namespace RCS.K2.NFLN.Helpers
{
    public enum ClipOption
    {
        Load = 0,
        Play = 1,
        Pause = 2,
        Reset = 3,
        Eject = 4
    }

    class FeedbackHandler
    {

        #region Private Fields
        #endregion

        #region public Properties

        #endregion
        //public FeedbackHandler(CurrentSession CS)
        //{
        //    cs = CS;
        //}
        #region ICommands Members

        public void Execute(object param = null)
        {
            try
            {
                if (param != null && param is TcpMessageReceivedEventArgs)
                {
                    TcpMessageReceivedEventArgs arg = param as TcpMessageReceivedEventArgs;

                    List<string> message = arg.Message.Split('|').ToList();
                    
                    if (message.Count == 2)
                    {
                        if (int.Parse(message[0]) == (decimal) ClipOption.Load)
                        {
                            CurrentSession.Player.LoadClip(message[1]);
                        }
                        else if (int.Parse(message[0]) == (decimal)ClipOption.Play)
                        {
                            CurrentSession.Player.PlayClip();
                        }
                        else if (int.Parse(message[0]) == (decimal)ClipOption.Pause)
                        {
                            CurrentSession.Player.PauseClip();
                        }
                        else if (int.Parse(message[0]) == (decimal)ClipOption.Reset)
                        {
                            CurrentSession.Player.ResetClip();
                        }
                        else if (int.Parse(message[0]) == (decimal)ClipOption.Eject)
                        {
                            CurrentSession.Player.EjectClip();
                        }

                        CurrentSession.Player.ConfirmDone(arg.Message);
                    }

                    //                    player.EndServerSession();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        #endregion

    }
}
