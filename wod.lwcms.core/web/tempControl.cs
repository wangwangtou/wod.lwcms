using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;

namespace wod.lwcms.web
{
    public class tempControl : UserControl
    {
        public static string ResourceUrl(string path)
        {
            return System.Web.VirtualPathUtility.GetDirectory("~/") + path;
        }

        public pageData PD
        {
            get { return (Page as tempPage).PD; }
        }
    }

    public class tempMaster : MasterPage
    {
        public static string ResourceUrl(string path)
        {
            return System.Web.VirtualPathUtility.GetDirectory("~/") + path;
        }

        public static string ThemeResourceUrl(string path)
        {
            return ResourceUrl(string.Format("theme/{0}/{1}",page.tempName,path));
        }

        public pageData PD
        {
            get { return (Page as tempPage).PD; }
        }
    }
}
