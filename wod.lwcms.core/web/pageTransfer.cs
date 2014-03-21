using System;
using System.Collections.Generic;
using System.Text;

namespace wod.lwcms.web
{
    public class pageTransfer
    {
        public string getIndexTransfer()
        {
            return "index";
        }

        public string getCommonTransfer(string requestName)
        {
            return requestName;
        }

        public string getCategoryTransfer(wod.lwcms.models.category cat)
        {
            return string.IsNullOrEmpty(cat.page) ? "category" : cat.page;
        }

        public string getArticleTransfer(wod.lwcms.models.article art)
        {
            return string.IsNullOrEmpty(art.page) ? "article" : art.page;
        }
    }
}
