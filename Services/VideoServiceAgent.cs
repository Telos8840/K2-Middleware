using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using GrassValley.Mseries.AppServer;
using GrassValley.Mseries.Control;
using GrassValley.Mseries.MediaMgr;
using NflnInteractive.Lookup.Entities;
using RCS.K2.NFLN.Models;

namespace RCS.K2.NFLN.Services
{
    public class VideoServiceAgent : IVideoServiceAgent
    {
        public List<VideoModel> LoadVideos()
        {
            var videos = new List<VideoModel>();
            try
            {
                CurrentSession.Player.CreateMediaMgr();

                object[] clipArray = CurrentSession.Player.GetAssets("MovieId+Name+FileLengthStr+Created");

                string path = "";
                foreach (object clip in clipArray)
                {
                    object[] propertyArray = (object[])clip;
                    path = String.Format(@"V:\default\{0}", propertyArray[1].ToString());

                    videos.Add(new VideoModel
                    {
                        ID = propertyArray[0].ToString(),
                        Name = propertyArray[1].ToString(),
                        Path = path,
                        TimeStamp = propertyArray[2].ToString(),
                        Created = (DateTime) propertyArray[3]
                    });
                }

                using (var sql = new NFLLookupEntities())
                {
                    var clips = sql.VideoClips;

                    foreach (var clip in clips)
                    {
                        var exist = videos.FirstOrDefault(v => v.ID.Equals(clip.VID));

                        if (exist == null)
                            sql.VideoClips.Remove(clip);
                    }

                    sql.SaveChanges();

                    foreach (var video in videos)
                    {
                        var exist = sql.VideoClips.SingleOrDefault(v => v.VID.Equals(video.ID));

                        if (exist == null)
                        {
                            sql.VideoClips.Add(new VideoClip
                            {
                                VID = video.ID,
                                ClipName = video.Name,
                                Path = video.Path,
                                Length = video.TimeStamp,
                                Created = video.Created
                            });
                        }
                    }
                    sql.SaveChanges();
                }

                CurrentSession.Player.EndMediaSession();

                /*CONVERTING TIMESTAMPS*/

                //TimeSpan time;
                //foreach (var video in videos)
                //{
                //    if (video.TimeStamp.Contains(","))
                //    {
                //        time = TimeSpan.ParseExact(video.TimeStamp, "G", null);
                //        Console.WriteLine(time);
                //    }
                //    else
                //    {
                //        time = TimeSpan.ParseExact(video.TimeStamp, "c", null);
                //        Console.WriteLine(time);
                //    }

                //}

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return videos.OrderByDescending(c => c.Created).ToList();
        }
    }
}