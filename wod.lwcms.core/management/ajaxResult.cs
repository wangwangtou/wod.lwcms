using System;
using System.Collections.Generic;
using System.Text;

namespace wod.lwcms.management
{
    public class ajaxResult
    {
        public bool status { get; set; }
        public string message { get; set; }
        public object result { get; set; }
    }
}
