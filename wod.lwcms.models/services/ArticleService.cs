using System;
using System.Collections.Generic;
using System.Text;

namespace wod.lwcms.services
{
    public class ArticleService : IArticleService
    {
        public bool checkViewArticleParam()
        {
            return true;
        }

        public bool checkCommentArticleParam(string name, string email, string content)
        {
            if (string.IsNullOrEmpty(name))
                return false;

            if (string.IsNullOrEmpty(email))
                return false;

            if (string.IsNullOrEmpty(content) || content.Length < 10)
                return false;

            return true;
        }
    }
}
