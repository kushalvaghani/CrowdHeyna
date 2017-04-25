using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CrowdHeyna.Controllers
{
    public class TweetsController : ApiController
    {
        [HttpGet]
        public List<Models.Tweet> get()
        {
            try
            {
                CrowdHeynaEntities db = new CrowdHeynaEntities();
                List<Models.Tweet> tweets = new List<Models.Tweet>();
                var skip = new Random().Next(tweets.Count);
               // var tList = db.Tweets.OrderBy(x => x.Id).Skip(skip).Take(15).ToList();
                var tList = db.Tweets.OrderBy(x => x.Id).Take(15).ToList();
                foreach (Tweet t in tList)
                {
                    Models.Tweet tw = new Models.Tweet()
                    {
                        Id = t.Id,
                        Text = t.Text
                    };
                    tweets.Add(tw);
                }
                return tweets;
            }
            catch (Exception ex)
            {
                var message = ex.Message;
            }
            return null;
        }

        [HttpGet]
        public Models.Tweet get(int id)
        {
            try
            {
                CrowdHeynaEntities db = new CrowdHeynaEntities();
                var tweet = db.Tweets.Where(t => t.Id == id).FirstOrDefault();
                var tw = new Models.Tweet()
                {
                    Id = tweet.Id,
                    Text = tweet.Text
                };
                return tw;
            }
            catch (Exception ex)
            {
                var message = ex.Message;
            }
            return null;
        }
    }
}
