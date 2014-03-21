using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using wod.lwcms.services;

namespace wod.lwcms.dataaccess
{
    public class CommonDataAccess : ICommonDataAccess
    {
        private ICategoryService cateService;

        public CommonDataAccess(ICategoryService service)
        {
            this.cateService = service;
        }

        public List<models.article> GetPagedArticle(DbDataReader pagedDr, int startRowIndex, int endRowIndex
            ,List<models.category> allCats)
        {
            List<models.article> dataLst = new List<models.article>();
            using (pagedDr)
            {
                int s = 1;
                bool noData = false;
                while (s < startRowIndex)
                {
                    if (!pagedDr.Read())
                    {
                        noData = true;
                        break; 
                    }
                    s++;
                }
                if (!noData)
                {
                    while (pagedDr.Read())
                    {
                        dataLst.Add(GetArticle(pagedDr, false, allCats));
                    }
                }
            }
            return dataLst;
        }

        private models.article GetArticle(DbDataReader dr, bool hasContent, List<models.category> allCats)
        {
            var art = new models.article();
            art.id = dr.GetString(dr.GetOrdinal("ID"));
            art.name = dr.GetString(dr.GetOrdinal("art_Name"));
            art.code = dr.GetString(dr.GetOrdinal("art_Code"));
            if (hasContent)
            {
                art.content = string.Format("{0}", dr.GetValue(dr.GetOrdinal("art_Content")));
                art.page = string.Format("{0}", dr.GetValue(dr.GetOrdinal("art_Page")));
            }
            //art_CreateOn,art_ViewCount,art_Creater
            art.createOn = dr.GetDateTime(dr.GetOrdinal("art_CreateOn"));
            art.viewCount = dr.GetInt32(dr.GetOrdinal("art_ViewCount"));
            art.creater = dr.GetString(dr.GetOrdinal("art_Creater"));

            art.preContent = string.Format("{0}", dr.GetValue(dr.GetOrdinal("art_PreContent")));
            art.category = GetCate(dr.GetString(dr.GetOrdinal("cat_Path")), allCats);
            art.description = string.Format("{0}", dr.GetValue(dr.GetOrdinal("art_Description")));
            art.keywords = string.Format("{0}", dr.GetValue(dr.GetOrdinal("art_Keywords")));
            return art;
        }

        private models.category GetCate(string path, List<models.category> allCats)
        {
            return cateService.getCurCategory(allCats,path);
        }

        public models.article GetSingleArticle(DbDataReader singleDr,List<models.category> allCats)
        {
            using (singleDr)
            {
                if (singleDr.Read())
                {
                    return GetArticle(singleDr, true, allCats);
                }
                else
                {
                    return new models.article();
                }
            }
        }
    }
}
