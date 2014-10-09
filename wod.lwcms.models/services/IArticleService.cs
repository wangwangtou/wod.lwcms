using System;
using System.Collections.Generic;
using System.Text;

namespace wod.lwcms.services
{
    public interface IArticleService
    {
        bool checkViewArticleParam();
        bool checkCommentArticleParam(string name, string email, string content);
    }
}
