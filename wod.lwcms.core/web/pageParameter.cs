using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace wod.lwcms.web
{

    public class pageParameter
    {
        private static void AddLwParameters(objectPool op,HttpRequest request)
        {
            op.setOjbect("lw_actData", wodEnvironment.GetActDataString(request));
            op.setOjbect("lw_newid", wodEnvironment.GetNewId());
            op.setOjbect("lw_now", wodEnvironment.GetNowString());

            op.setOjbect("lw_user", wodEnvironment.GetUser());
            op.setOjbect("lw_usertype", wodEnvironment.GetUserType());
        }

        public pageType pageType { get; private set; }

        public string pageCommandId { get; private set; }

        public string requestName { get; set; }

        public string requestSubName { get; set; }

        public static pageParameter Parse(HttpRequest request, ref objectPool op)
        {
            pageParameter p = new pageParameter();
            string path = request.QueryString["path"];
            if (string.IsNullOrEmpty(path) || path == "index") {
                p.pageType = pageType.index;
                p.pageCommandId = "index";
            }
            else if (path.IndexOf("/common/") == 0) {
                p.pageType = pageType.common;
                p.requestName = path.Substring("/common/".Length);
                p.pageCommandId = p.requestName;
            }
            else if (path.EndsWith(".html"))
            {
                p.pageType = pageType.article;
                p.pageCommandId = "article";

                int pos = path.LastIndexOf('/');
                path = path.Substring(0,path.Length -".html".Length);
                p.requestName = path.Substring(pos + 1);

                op.setOjbect("categoryPath", path.Substring(0, pos));
                op.setOjbect("articleName", p.requestName);
            }
            else
            {
                p.pageType = pageType.category;
                p.pageCommandId = "category";
                p.requestName = path;
                op.setOjbect("categoryPath", path);
            }
            op.setOjbect("requestName", p.requestName);
            op.setOjbect("requestKey", request.Url.ToString().ToLower());
            foreach (string item in request.QueryString.Keys)
            {
                op.setOjbect(item, getObj(request.QueryString, item));
            }
            AddLwParameters(op,request);
            if (p.pageType == pageType.common)
            {
                foreach (string item in request.Form.Keys)
                {
                    op.setOjbect(item, getObj(request.Form, item));
                }
            }
            return p;
        }

        private static object getObj(System.Collections.Specialized.NameValueCollection nameValueCollection, string item)
        {
            if (queryStringTypes.ContainsKey(item))
            {
                return Convert.ChangeType(nameValueCollection[item], queryStringTypes[item]);
            }
            else
            {
                return nameValueCollection[item];
            }
        }

        private static readonly Dictionary<string, Type> queryStringTypes = new Dictionary<string, Type>()
        {
            {"pageIndex",typeof(int)}
        };
    }
}
