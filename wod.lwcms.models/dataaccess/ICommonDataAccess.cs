using System;
using System.Collections.Generic;
using System.Text;
using wod.lwcms.models;
using System.Data.Common;
using System.Collections;

namespace wod.lwcms.dataaccess
{
    public interface ICommonDataAccess 
    {
        List<models.article> GetPagedArticle(DbDataReader pagedDr, int startRowIndex, int endRowIndex, List<models.category> allCats);
        models.article GetSingleArticle(DbDataReader singleDr, List<models.category> allCats);
       
        List<models.comment> GetPagedComment(DbDataReader pagedDr, int startRowIndex, int endRowIndex);
    }
}
