using System;
using System.Collections.Generic;
using System.Text;

namespace wod.lwcms.models
{
    public class article :ISeoObject
    {
        public string id { get; set; }
        public string name { get; set; }
        public string code { get; set; }

        public string content { get; set; }
        public string preContent { get; set; }

        public string page { get; set; }

        public category category { get; set; }

        public siteImage image { get; set; }

        public string description
        {
            get;
            set;
        }

        public string keywords
        {
            get;
            set;
        }

        string ISeoObject.seotitle
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        public DateTime createOn { get; set; }

        public long viewCount { get; set; }

        public string creater { get; set; }

        public string state { get; set; }

        public string extendData { get; set; }

        public string extendForm { get { return category == null ? null : category.extendform; } }
    }
}
