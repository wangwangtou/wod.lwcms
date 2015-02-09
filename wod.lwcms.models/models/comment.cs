using System;
using System.Collections.Generic;
using System.Text;

namespace wod.lwcms.models
{
    public class comment
    {
        public comment()
        {
        }

        public string id { get; set; }

        public string userName { get; set; }
        public string userEmail { get; set; }

        public string userType { get; set; }
        public string userID { get; set; }

        public string commentContent { get; set; }
        public DateTime commentTime { get; set; }

        public string commentAid { get; set; }

        public string state { get; set; }
    }
}
