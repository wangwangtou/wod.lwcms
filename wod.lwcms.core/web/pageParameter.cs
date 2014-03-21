using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace wod.lwcms.web
{
    public class pageParameter
    {
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
            foreach (string item in request.QueryString.Keys)
            {
                op.setOjbect(item, request.QueryString[item]);
            }
            return p;
        }
    }
}
