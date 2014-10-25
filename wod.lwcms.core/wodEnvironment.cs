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

        public static string GetDataPath(string sKey, string path)
        {
            if (siteKey == sKey)
            {
                return HttpContext.Current.Server.MapPath(resourcePath + path);
            }
            else
            {
                return HttpContext.Current.Server.MapPath(resourcePath + sKey + "/" + path);
            }
        }

        public static string GetDataPath(string path)
        {
            return HttpContext.Current.Server.MapPath(resourcePath + path);
        }

        public static object GetNowString()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static string GetNewId()
        {
            return Guid.NewGuid().ToString();
        }

        public static object GetActDataString(HttpRequest request)
        { 
            return common.ToJson(GetActData(request));
        }

        public static object GetUser()
        {
            return ANONYMOUS_USER;
        }

        private const string ANONYMOUS_USER = "ANONYMOUS_USER";


        private static actData GetActData(HttpRequest request)
        {
            actData data = new actData();
            data.IP = GetRequestIp(request);
            data.Time = GetNowString();
            return data;
        }

        private static string GetRequestIp(HttpRequest request)
        {
            if (request.ServerVariables["HTTP_VIA"] != null)
                return request.ServerVariables["HTTP_X_FORWARDED_FOR"].Split(new char[] { ',' })[0];
            else
                return request.ServerVariables["REMOTE_ADDR"];
        }

        public class actData
        {
            public object IP { get; set; }
            public object Time { get; set; }
        }

        public static Type GetTypeByName(string typeName)
        {
            return Type.GetType(typeName);
        }
    }
}
