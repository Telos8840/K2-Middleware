using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCS.K2.NFLN.Services
{
    public interface IClipPlayerServiceAgent
    {
        void LoadClip(string id);
        void PlayClip();
        void PauseClip();
        void ResetClip();
        void EjectClip();
        void ConfirmDone(string message);
        void EndServerSession();
        void CreateMediaMgr();
        object[] GetAssets(string query);
        void EndMediaSession();
    }
}