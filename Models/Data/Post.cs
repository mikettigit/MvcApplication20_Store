using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication20.Models
{
    public class Post
    {
        public string Id;
        public string Title;
        public string Description;
        public string Content;
        public string Meta_Keywords;
        public string Meta_Description;
        public string Meta_Title;
        public DateTime Date;
        public string DateString
        {
            get { return Date.ToString("dd.MM.yyyy"); }
        }

        public Post()
        {
            Id = "";
            Title = "";
            Description = "";
            Content = "";
            Meta_Keywords = "";
            Meta_Description = "";
            Date = DateTime.MinValue;
        }
    }
}