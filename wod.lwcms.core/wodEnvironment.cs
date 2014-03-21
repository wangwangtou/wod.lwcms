using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace wod.lwcms
{
    public static class wodEnvironment
    {
        public static readonly string resourcePath = "~/App_Data/";
        public static readonly string siteKey = "default";

        public static string GetDataPath(string path)
        {
            return HttpContext.Current.Server.MapPath(resourcePath + path);
        }
    }
}
