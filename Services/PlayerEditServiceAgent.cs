using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RCS.K2.NFLN.Models;

namespace RCS.K2.NFLN.Services
{
    public class PlayerEditServiceAgent : IPlayerEditServiceAgent
    {
        public List<NomineePods> LoadListData()
        {
            return CurrentSession.ListData;
        }

        public List<MockDraft> LoadOverallData()
        {
            return CurrentSession.OverallData;
        }
    }
}
