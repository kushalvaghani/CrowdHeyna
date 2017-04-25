using CrowdHeyna.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace CrowdHeyna.Controllers
{
    public class UserKeywordSuggestionsController : ApiController
    { 
        [HttpGet]
        public List<string> Get(int keyword, int id)
        {
            CrowdHeynaEntities db = new CrowdHeynaEntities();
            //var key = db.Keywords.Where(k => k.Keyword1 == keywordId).FirstOrDefault().Id;
            return db.UserKeywordTagSuggestions.Where(uts => uts.KeywordId == keyword && uts.TagId == id).Select(s => s.Suggestion).ToList();
        }

        public void Post(List<UserKeywordSuggestion> suggestions)
        {
            CrowdHeynaEntities db = new CrowdHeynaEntities();
            var username = (string)HttpContext.Current.Session["username"];
            var user = db.AspNetUsers.Where(us => us.UserName == username).FirstOrDefault();
            //

            foreach (UserKeywordSuggestion sugg in suggestions)
            {
                var internalUser = db.Users.Where(us => us.UserId == user.Id).FirstOrDefault();
                if (internalUser != null)
                {
                    UserKeywordTagSuggestion sug = new UserKeywordTagSuggestion()
                    {
                        KeywordId = sugg.KeywordId,
                        TagId = sugg.TagId,
                        UserId = internalUser.Id,
                        Suggestion = sugg.Suggestion
                    };
                    db.UserKeywordTagSuggestions.Add(sug);
                }
            }
            db.SaveChanges();
        }

    }
}