using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NflnInteractive.Lookup.Entities;

namespace RCS.K2.NFLN.Services
{
    public class SignInServiceAgent : ISignInServiceAgent
    {
        public List<string> LoadUsers()
        {
            var users = new List<string>();

            try
            {
                using (var sql = new NFLIntDevEntities())
                {
                    var tempUsers = sql.UserProfiles;

                    users.AddRange(tempUsers.Select(user => user.UserName));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return users;
        }
    }
}
