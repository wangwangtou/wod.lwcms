using System;
using System.Collections.Generic;
using System.Text;

namespace wod.lwcms.web
{
    public class partView
    {
        public string name { get; set; }
        public string cssClass { get; set; }
        public viewType type { get; set; }
        public string data { get; set; }

        public enum viewType
        { 
            control
        }

        public enum viewPos
        {
            commandbar,
            statusbar,
            toolpanel
        }
    }
}
