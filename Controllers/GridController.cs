using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CrowdHeyna.Controllers
{
    public class GridController : ApiController
    {
        [HttpGet]
        public List<Models.GridRowModel> Get(int tweetId)
        {
            var resultRows = new List<Models.GridRowModel>();
            using (CrowdHeynaEntities db = new CrowdHeynaEntities())
            {
                var keywords = db.UserKeywordTags.Where(key => key.Keyword.TweetId == tweetId).ToList();
                //
                var uniqueKeywords = keywords.Select(s => s.Keyword.Keyword1).Distinct().ToList();
                //
                foreach(string keyword in uniqueKeywords)
                {
                    Models.GridRowModel newRow = new Models.GridRowModel();
                    newRow.Keyword = keyword;
                    //
                    resultRows.Add(newRow);
                }
            }
            //
            return resultRows;
        }

        [HttpPost]
        public void Post(List<Models.GridRowModel> data)
        {
            //save to DB
        }
    }
}
