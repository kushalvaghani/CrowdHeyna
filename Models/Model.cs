using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CrowdHeyna.Models
{

    public class Tweet
    {
        public int Id { get; set; }
        public string Text { get; set; }
    }

    public class Keyword
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int TagCount { get; set; }
    }

    public class Keywords
    {
        public Keywords()
        {
            fromText = new List<Keyword>();
            fromHashtag = new List<Keyword>();
            fromURL = new List<Keyword>();
        }
        public List<Keyword> fromText { get; set; }
        public List<Keyword> fromHashtag { get; set; }
        public List<Keyword> fromURL { get; set; }
    }

    public class ViewTag
    {
        public int Id { get; set; }
        public string Name { get; set; }


        public string Color { get; set; }
    }

    public class KeywordTags
    {

        public int KeywordId { get; set; }
        public int TagId { get; set; }
    }

    public class KeywordUserTags
    {
        public KeywordUserTags()
        {
            TagInfo = new List<Models.TagInfo>();
        }
        public String TagName { get; set; }
        public int TagId { get; set; }

        public string TagColor { get; set; }

        public List<TagInfo> TagInfo { get; set; }
    }

    public class TagInfo
    {
        public int UserCount { get; set; }
        public int TagUserCount { get; set; }
        public int KeywordId { get; set; }
        public string KeywordName { get; set; }
    }

    public class GridRowModel
    {
        public string Keyword { get; set; }

        public string Jargon { get; set; }

        public string Abbreviation { get; set; }

        public string Misspelt { get; set; }

        public string Similarity { get; set; }
    }

    public class UserKeywordSuggestion
    {
        public int KeywordId { get; set; }
        public int TagId { get; set; }
        public string Suggestion { get; set; }
    }

}