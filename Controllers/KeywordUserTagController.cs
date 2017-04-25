using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace CrowdHeyna.Controllers
{
    public class KeywordUserTagController : ApiController
    {
        [HttpGet]
        public List<Models.KeywordUserTags> Get(int tweetid, string keyword = null)
        {
            var keywordUserTags = new List<Models.KeywordUserTags>();
            //
            using (CrowdHeynaEntities db = new CrowdHeynaEntities())
            {
                var tweet = db.Tweets.Where(tw => tw.Id == tweetid).FirstOrDefault();
                var tags = db.Tags.ToList();
                var userKeywordTags = db.UserKeywordTags.ToList();
                var totalUsers = db.UserKeywordTags.Select(m => m.UserId).Distinct().Count();
                //
                foreach(Tag tag in tags)
                {
                    var kut = new Models.KeywordUserTags()
                    {
                        TagId = tag.Id,
                        TagName = tag.Name
                    };
                    switch (kut.TagId)
                    {
                        case 1: kut.TagColor = "Red"; break;
                        case 2: kut.TagColor = "Green"; break;
                        case 3: kut.TagColor = "Blue"; break;
                        case 4: kut.TagColor = "Purple"; break;
                    }
                    //
                    foreach (Keyword k in tweet.Keywords.ToList())
                    {
                        if (keyword != "0" && keyword != k.Keyword1)
                            continue;
                        //
                        var keywordTags = userKeywordTags.Where(ukt => ukt.KeywordId == k.Id && ukt.Keyword.TweetId == tweet.Id);
                        if(keywordTags != null)
                        {
                            var list = keywordTags.ToList();
                            foreach(UserKeywordTag ukt in list)
                            {
                                if (ukt.TagId == tag.Id)
                                {
                                    if (kut.TagInfo.Exists(ti => ti.KeywordId == ukt.KeywordId))
                                    {
                                        //var tagInf = kut.TagInfo.Where(ti => ti.KeywordId == ukt.KeywordId).FirstOrDefault();
                                    }
                                    else
                                    {
                                        Models.TagInfo tagInfo = new Models.TagInfo();
                                        tagInfo.KeywordId = ukt.KeywordId;
                                        tagInfo.KeywordName = ukt.Keyword.Keyword1;
                                        tagInfo.TagUserCount = keywordTags.Where(kt => kt.TagId == tag.Id).Count();
                                        tagInfo.UserCount = totalUsers;
                                        //
                                        kut.TagInfo.Add(tagInfo);
                                    }
                                }
                            }
                        }
                    }
                    keywordUserTags.Add(kut);
                }
            }
                //
                return keywordUserTags;
        }
    }
}