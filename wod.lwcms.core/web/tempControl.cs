using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;

namespace wod.lwcms.web
{
    public class tempControl : UserControl
    {
        public string ResourceUrl(string path)
        {
            return System.Web.VirtualPathUtility.GetDirectory("~/") + path;
        }

        public string RenderPartView(partView view)
        {
            return (Page as tempPage).RenderPartView(view);
        }

        public pageData PD
        {
            get { return (Page as tempPage).PD; }
        }
    }

    public class tempMaster : MasterPage
    {
        public string ResourceUrl(string path)
        {
            return System.Web.VirtualPathUtility.GetDirectory("~/") + path;
        }

        public string ThemeResourceUrl(string path)
        {
            return ResourceUrl(string.Format("theme/{0}/{1}",cssTempName,path));
        }

        public string RenderPartView(partView view)
        {
            return (Page as tempPage).RenderPartView(view);
        }

        public string tempName
        {
            get { return (Page as tempPage).tempName; }
        }

        public string cssTempName
        {
            get { return (Page as tempPage).cssTempName; }
        }

        public pageData PD
        {
            get { return (Page as tempPage).PD; }
        }
    }
}
