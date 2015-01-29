using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCS.K2.NFLN.Services
{
    public interface ISignInServiceAgent
    {
        List<string> LoadUsers();
    }
}
