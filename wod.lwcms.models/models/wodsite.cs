using System;
using System.Collections.Generic;
using System.Text;

namespace wod.lwcms.models
{
    public class wodsite : ISeoObject
    {
        public wodsite()
        {
            navis = new Dictionary<string, List<siteNavi>>();
        }
        public string siteName { get; set; }
        public string siteUrl { get; set; }
        public string description { get; set; }
        public string keywords { get; set; }
        public string copyright { get; set; }
        public string title { get; set; }
        public Dictionary<string,List<siteNavi>> navis { get; private set; }

        string ISeoObject.seotitle
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
            }
        }
    }

    public class siteNavi
    {
        public siteNavi()
        {
            subNavis = new List<siteNavi>();
        }
        public string name { get; set; }
        public string title { get; set; }
        public string naviUrl { get; set; }
        public string target { get; set; }

        public List<siteNavi> subNavis { get; private set; }
    }
}
