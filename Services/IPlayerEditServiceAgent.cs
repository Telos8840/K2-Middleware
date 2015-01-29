using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RCS.K2.NFLN.Models;

namespace RCS.K2.NFLN.Services
{
    public interface IPlayerEditServiceAgent
    {
        List<NomineePods> LoadListData();
        List<MockDraft> LoadOverallData();
    }
}
