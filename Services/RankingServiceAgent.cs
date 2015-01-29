using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NflnInteractive.Lookup.Entities;
using RCS.K2.NFLN.Models;

namespace RCS.K2.NFLN.Services
{
    public class RankingServiceAgent : IRankingServiceAgent
    {
        public List<Segments> LoadSegments()
        {
            var segments = new List<Segments>();

            try
            {
                using (var sql = new NFLIntDevEntities())
                {
                    var segs = sql.Segments.Where(i => i.UID == CurrentSession.User.ID && i.Type == (decimal)SegmentType.PlayerRanking);

                    segments.AddRange(segs.Select(segment => new Segments
                    {
                        ID = segment.ID,
                        Name = segment.Name,
                        Title = segment.Title
                    }));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return segments;
        }

        public List<PlayerInfo> LoadPlayerInfo()
        {
            var players = new List<PlayerInfo>();

            return players;
        }
    }
}
