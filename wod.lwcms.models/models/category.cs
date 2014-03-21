using System;
using System.Collections.Generic;
using System.Text;

namespace wod.lwcms.models
{
    public class category : ISeoObject
    {
        public category()
        {
            subCategory = new List<category>();
        }

        public string id { get; set; }
        public string name { get; set; }
        public string code { get; set; }

        public int level { get; set; }
        public string fullpath { get; set; }

        public List<category> subCategory { get; private set; }

        public category parent { get; set; }

        public siteImage image { get; set; }

        public string description{ get; set; }

        public string keywords{ get; set; }

        public string content { get; set; }

        public string page { get; set; }
        public string contentpage { get; set; }

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
    }
}
