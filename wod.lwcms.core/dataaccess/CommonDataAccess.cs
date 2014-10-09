using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using wod.lwcms.services;
using wod.lwcms.models;

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
                        dataLst.Add(GetArticle(pagedDr, allCats));
                    }
                }
            }
            return dataLst;
        }

        private models.article GetArticle(DbDataReader dr, List<models.category> allCats)
        {
            var art = new models.article();
            art.id = dr.GetString(dr.GetOrdinal("ID"));
            art.name = dr.GetString(dr.GetOrdinal("art_Name"));
            art.code = dr.GetString(dr.GetOrdinal("art_Code"));
                art.content = string.Format("{0}", dr.GetValue(dr.GetOrdinal("art_Content")));
                art.page = string.Format("{0}", dr.GetValue(dr.GetOrdinal("art_Page")));
            art.image = common.FromJson<siteImage>(Convert.ToString(dr.GetValue(dr.GetOrdinal("art_SiteImage"))));
            art.extendData = Convert.ToString(dr.GetValue(dr.GetOrdinal("art_ExtendData")));

            //art_CreateOn,art_ViewCount,art_Creater
            art.createOn = dr.GetDateTime(dr.GetOrdinal("art_CreateOn"));
            art.viewCount = dr.GetInt32(dr.GetOrdinal("art_ViewCount"));
            art.creater = dr.GetString(dr.GetOrdinal("art_Creater"));
            art.state = Convert.ToString(dr.GetValue(dr.GetOrdinal("art_State")));

            art.preContent = string.Format("{0}", dr.GetValue(dr.GetOrdinal("art_PreContent")));
            art.category = GetCate(dr.GetString(dr.GetOrdinal("cat_Path")), allCats);
            art.description = string.Format("{0}", dr.GetValue(dr.GetOrdinal("art_Description")));
            art.keywords = string.Format("{0}", dr.GetValue(dr.GetOrdinal("art_Keywords")));
            return art;
        }

        private models.category GetCate(string path, List<models.category> allCats)
        {
            models.category cate = cateService.getCurCategory(allCats, path);
            return cate ;//== null ? null : new JsonCategory(cate);
        }

        //private class JsonSiteImage : models.siteImage, LitJson.IJsonWrapper
        //{
        //    public JsonSiteImage()
        //    {
        //    }
        //    void LitJson.IJsonWrapper.ToJson(LitJson.JsonWriter writer)
        //    {
        //        models.siteImage img = new siteImage();

        //        img.bigImg = base.bigImg;
        //        img.normalImg = base.normalImg;
        //        img.smallImg = base.smallImg;
        //        new LitJson.JsonData(common.ToJson(img)).ToJson(writer);
        //    }

        //    string LitJson.IJsonWrapper.ToJson()
        //    {
        //        models.siteImage img = new siteImage();
        //        img.bigImg = base.bigImg;
        //        img.normalImg = base.normalImg;
        //        img.smallImg = base.smallImg;
        //        return new LitJson.JsonData(common.ToJson(img)).ToJson();
        //    }

        //    #region MyRegion


        //    bool LitJson.IJsonWrapper.GetBoolean()
        //    {
        //        return default(bool);
        //    }

        //    double LitJson.IJsonWrapper.GetDouble()
        //    {
        //        return default(double);
        //    }

        //    int LitJson.IJsonWrapper.GetInt()
        //    {
        //        return default(int);
        //    }

        //    LitJson.JsonType LitJson.IJsonWrapper.GetJsonType()
        //    {
        //        return LitJson.JsonType.Object;
        //    }

        //    long LitJson.IJsonWrapper.GetLong()
        //    {
        //        return default(long);
        //    }

        //    string LitJson.IJsonWrapper.GetString()
        //    {
        //        return default(string);
        //    }

        //    bool LitJson.IJsonWrapper.IsArray
        //    {
        //        get { return false; }
        //    }

        //    bool LitJson.IJsonWrapper.IsBoolean
        //    {
        //        get { return false; }
        //    }

        //    bool LitJson.IJsonWrapper.IsDouble
        //    {
        //        get { return false; }
        //    }

        //    bool LitJson.IJsonWrapper.IsInt
        //    {
        //        get { return false; }
        //    }

        //    bool LitJson.IJsonWrapper.IsLong
        //    {
        //        get { return false; }
        //    }

        //    bool LitJson.IJsonWrapper.IsObject
        //    {
        //        get { return false; }
        //    }

        //    bool LitJson.IJsonWrapper.IsString
        //    {
        //        get { return false; }
        //    }

        //    void LitJson.IJsonWrapper.SetBoolean(bool val)
        //    {
        //    }

        //    void LitJson.IJsonWrapper.SetDouble(double val)
        //    {
        //    }

        //    void LitJson.IJsonWrapper.SetInt(int val)
        //    {
        //    }

        //    void LitJson.IJsonWrapper.SetJsonType(LitJson.JsonType type)
        //    {
        //    }

        //    void LitJson.IJsonWrapper.SetLong(long val)
        //    {
        //    }

        //    void LitJson.IJsonWrapper.SetString(string val)
        //    {
        //    }

        //    int System.Collections.IList.Add(object value)
        //    {
        //        return 0;
        //    }

        //    void System.Collections.IList.Clear()
        //    {
        //    }

        //    bool System.Collections.IList.Contains(object value)
        //    {
        //        return false;
        //    }

        //    int System.Collections.IList.IndexOf(object value)
        //    {
        //        return 0;
        //    }

        //    void System.Collections.IList.Insert(int index, object value)
        //    {
        //    }

        //    bool System.Collections.IList.IsFixedSize
        //    {
        //        get { return false; }
        //    }

        //    bool System.Collections.IList.IsReadOnly
        //    {
        //        get { return false; }
        //    }

        //    void System.Collections.IList.Remove(object value)
        //    {
        //    }

        //    void System.Collections.IList.RemoveAt(int index)
        //    {
        //    }

        //    object System.Collections.IList.this[int index]
        //    {
        //        get
        //        {
        //            return null;
        //        }
        //        set
        //        {
        //        }
        //    }

        //    void System.Collections.ICollection.CopyTo(Array array, int index)
        //    {
        //    }

        //    int System.Collections.ICollection.Count
        //    {
        //        get
        //        {
        //            return 0;
        //        }
        //    }

        //    bool System.Collections.ICollection.IsSynchronized
        //    {
        //        get { return false; }
        //    }

        //    object System.Collections.ICollection.SyncRoot
        //    {
        //        get { return null; }
        //    }

        //    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        //    {
        //        return null;
        //    }

        //    System.Collections.IDictionaryEnumerator System.Collections.Specialized.IOrderedDictionary.GetEnumerator()
        //    {
        //        return null;
        //    }

        //    void System.Collections.Specialized.IOrderedDictionary.Insert(int index, object key, object value)
        //    {
        //    }

        //    void System.Collections.Specialized.IOrderedDictionary.RemoveAt(int index)
        //    {
        //    }

        //    object System.Collections.Specialized.IOrderedDictionary.this[int index]
        //    {
        //        get
        //        {
        //            return null;
        //        }
        //        set
        //        {
        //        }
        //    }

        //    void System.Collections.IDictionary.Add(object key, object value)
        //    {
        //    }

        //    void System.Collections.IDictionary.Clear()
        //    {
        //    }

        //    bool System.Collections.IDictionary.Contains(object key)
        //    {
        //        return false;
        //    }

        //    System.Collections.IDictionaryEnumerator System.Collections.IDictionary.GetEnumerator()
        //    {
        //        return null;
        //    }

        //    bool System.Collections.IDictionary.IsFixedSize
        //    {
        //        get { return false; }
        //    }

        //    bool System.Collections.IDictionary.IsReadOnly
        //    {
        //        get { return false; }
        //    }

        //    System.Collections.ICollection System.Collections.IDictionary.Keys
        //    {
        //        get { return null; }
        //    }

        //    void System.Collections.IDictionary.Remove(object key)
        //    {
        //    }

        //    System.Collections.ICollection System.Collections.IDictionary.Values
        //    {
        //        get { return null; }
        //    }

        //    object System.Collections.IDictionary.this[object key]
        //    {
        //        get
        //        {
        //            return null;
        //        }
        //        set
        //        {
        //        }
        //    }
        //    #endregion
        //}

        //private class JsonCategory : models.category,LitJson.IJsonWrapper
        //{
        //    public JsonCategory(models.category cate)
        //        : base()
        //    {
        //        base.id = cate.id;
        //        base.name = cate.name;
        //        base.code = cate.code;
        //        base.level = cate.level;
        //        base.fullpath = cate.fullpath;
        //        base.parent = cate.parent;
        //        base.image = cate.image;
        //        base.description = cate.description;
        //        base.subCategory.AddRange(cate.subCategory);
        //        base.keywords = cate.keywords;
        //        base.content = cate.content;
        //        base.page = cate.page;
        //        base.contentpage = cate.contentpage;
        //    }

        //    void LitJson.IJsonWrapper.ToJson(LitJson.JsonWriter writer)
        //    {
        //        new LitJson.JsonData(base.fullpath).ToJson(writer);
        //    }

        //    string LitJson.IJsonWrapper.ToJson()
        //    {
        //        return new LitJson.JsonData(base.fullpath).ToJson();
        //    }

        //    #region MyRegion


        //    bool LitJson.IJsonWrapper.GetBoolean()
        //    {
        //        return default(bool);
        //    }

        //    double LitJson.IJsonWrapper.GetDouble()
        //    {
        //        return default(double);
        //    }

        //    int LitJson.IJsonWrapper.GetInt()
        //    {
        //        return default(int);
        //    }

        //    LitJson.JsonType LitJson.IJsonWrapper.GetJsonType()
        //    {
        //        return LitJson.JsonType.Object;
        //    }

        //    long LitJson.IJsonWrapper.GetLong()
        //    {
        //        return default(long);
        //    }

        //    string LitJson.IJsonWrapper.GetString()
        //    {
        //        return default(string);
        //    }

        //    bool LitJson.IJsonWrapper.IsArray
        //    {
        //        get { return false; }
        //    }

        //    bool LitJson.IJsonWrapper.IsBoolean
        //    {
        //        get { return false; }
        //    }

        //    bool LitJson.IJsonWrapper.IsDouble
        //    {
        //        get { return false; }
        //    }

        //    bool LitJson.IJsonWrapper.IsInt
        //    {
        //        get { return false; }
        //    }

        //    bool LitJson.IJsonWrapper.IsLong
        //    {
        //        get { return false; }
        //    }

        //    bool LitJson.IJsonWrapper.IsObject
        //    {
        //        get { return false; }
        //    }

        //    bool LitJson.IJsonWrapper.IsString
        //    {
        //        get { return false; }
        //    }

        //    void LitJson.IJsonWrapper.SetBoolean(bool val)
        //    {
        //    }

        //    void LitJson.IJsonWrapper.SetDouble(double val)
        //    {
        //    }

        //    void LitJson.IJsonWrapper.SetInt(int val)
        //    {
        //    }

        //    void LitJson.IJsonWrapper.SetJsonType(LitJson.JsonType type)
        //    {
        //    }

        //    void LitJson.IJsonWrapper.SetLong(long val)
        //    {
        //    }

        //    void LitJson.IJsonWrapper.SetString(string val)
        //    {
        //    }

        //    int System.Collections.IList.Add(object value)
        //    {
        //        return 0;
        //    }

        //    void System.Collections.IList.Clear()
        //    {
        //    }

        //    bool System.Collections.IList.Contains(object value)
        //    {
        //        return false;
        //    }

        //    int System.Collections.IList.IndexOf(object value)
        //    {
        //        return 0;
        //    }

        //    void System.Collections.IList.Insert(int index, object value)
        //    {
        //    }

        //    bool System.Collections.IList.IsFixedSize
        //    {
        //        get { return false; }
        //    }

        //    bool System.Collections.IList.IsReadOnly
        //    {
        //        get { return false; }
        //    }

        //    void System.Collections.IList.Remove(object value)
        //    {
        //    }

        //    void System.Collections.IList.RemoveAt(int index)
        //    {
        //    }

        //    object System.Collections.IList.this[int index]
        //    {
        //        get
        //        {
        //            return null;
        //        }
        //        set
        //        {
        //        }
        //    }

        //    void System.Collections.ICollection.CopyTo(Array array, int index)
        //    {
        //    }

        //    int System.Collections.ICollection.Count
        //    {
        //        get
        //        {
        //            return 0;
        //        }
        //    }

        //    bool System.Collections.ICollection.IsSynchronized
        //    {
        //        get { return false; }
        //    }

        //    object System.Collections.ICollection.SyncRoot
        //    {
        //        get { return null; }
        //    }

        //    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        //    {
        //        return null;
        //    }

        //    System.Collections.IDictionaryEnumerator System.Collections.Specialized.IOrderedDictionary.GetEnumerator()
        //    {
        //        return null;
        //    }

        //    void System.Collections.Specialized.IOrderedDictionary.Insert(int index, object key, object value)
        //    {
        //    }

        //    void System.Collections.Specialized.IOrderedDictionary.RemoveAt(int index)
        //    {
        //    }

        //    object System.Collections.Specialized.IOrderedDictionary.this[int index]
        //    {
        //        get
        //        {
        //            return null;
        //        }
        //        set
        //        {
        //        }
        //    }

        //    void System.Collections.IDictionary.Add(object key, object value)
        //    {
        //    }

        //    void System.Collections.IDictionary.Clear()
        //    {
        //    }

        //    bool System.Collections.IDictionary.Contains(object key)
        //    {
        //        return false;
        //    }

        //    System.Collections.IDictionaryEnumerator System.Collections.IDictionary.GetEnumerator()
        //    {
        //        return null;
        //    }

        //    bool System.Collections.IDictionary.IsFixedSize
        //    {
        //        get { return false; }
        //    }

        //    bool System.Collections.IDictionary.IsReadOnly
        //    {
        //        get { return false; }
        //    }

        //    System.Collections.ICollection System.Collections.IDictionary.Keys
        //    {
        //        get { return null; }
        //    }

        //    void System.Collections.IDictionary.Remove(object key)
        //    {
        //    }

        //    System.Collections.ICollection System.Collections.IDictionary.Values
        //    {
        //        get { return null; }
        //    }

        //    object System.Collections.IDictionary.this[object key]
        //    {
        //        get
        //        {
        //            return null;
        //        }
        //        set
        //        {
        //        }
        //    }
        //    #endregion
        //}

        public models.article GetSingleArticle(DbDataReader singleDr,List<models.category> allCats)
        {
            using (singleDr)
            {
                if (singleDr.Read())
                {
                    return GetArticle(singleDr, allCats);
                }
                else
                {
                    return new models.article();
                }
            }
        }


        public List<comment> GetPagedComment(DbDataReader pagedDr, int startRowIndex, int endRowIndex)
        {
            List<models.comment> dataLst = new List<models.comment>();
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
                        dataLst.Add(GetComment(pagedDr));
                    }
                }
            }
            return dataLst;
        }

        private comment GetComment(DbDataReader dr)
        {
            var art = new models.comment();
            art.id = dr.GetString(dr.GetOrdinal("ID"));
            art.userName = dr.GetString(dr.GetOrdinal("cmt_userName"));
            art.userEmail = dr.GetString(dr.GetOrdinal("cmt_userEmail"));
            art.userType = dr.GetString(dr.GetOrdinal("cmt_userType"));
            art.commentAid = dr.GetString(dr.GetOrdinal("cmt_aid"));
            art.commentTime = dr.GetDateTime(dr.GetOrdinal("cmt_Time"));
            art.commentContent = dr.GetString(dr.GetOrdinal("cmt_Content"));
            return art;
        }
    }
}
