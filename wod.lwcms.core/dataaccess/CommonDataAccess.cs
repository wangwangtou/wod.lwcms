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
            art.image = common.FromJson<JsonSiteImage>(Convert.ToString(dr.GetValue(dr.GetOrdinal("art_SiteImage"))));
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
            return new JsonCategory(cateService.getCurCategory(allCats,path));
        }

        private class JsonSiteImage : models.siteImage, LitJson.IJsonWrapper
        {
            public JsonSiteImage()
            {
            }
            void LitJson.IJsonWrapper.ToJson(LitJson.JsonWriter writer)
            {
                models.siteImage img = new siteImage();

                img.bigImg = base.bigImg;
                img.normalImg = base.normalImg;
                img.smallImg = base.smallImg;
                new LitJson.JsonData(common.ToJson(img)).ToJson(writer);
            }

            string LitJson.IJsonWrapper.ToJson()
            {
                models.siteImage img = new siteImage();
                img.bigImg = base.bigImg;
                img.normalImg = base.normalImg;
                img.smallImg = base.smallImg;
                return new LitJson.JsonData(common.ToJson(img)).ToJson();
            }

            #region MyRegion


            bool LitJson.IJsonWrapper.GetBoolean()
            {
                throw new NotImplementedException();
            }

            double LitJson.IJsonWrapper.GetDouble()
            {
                throw new NotImplementedException();
            }

            int LitJson.IJsonWrapper.GetInt()
            {
                throw new NotImplementedException();
            }

            LitJson.JsonType LitJson.IJsonWrapper.GetJsonType()
            {
                throw new NotImplementedException();
            }

            long LitJson.IJsonWrapper.GetLong()
            {
                throw new NotImplementedException();
            }

            string LitJson.IJsonWrapper.GetString()
            {
                throw new NotImplementedException();
            }

            bool LitJson.IJsonWrapper.IsArray
            {
                get { throw new NotImplementedException(); }
            }

            bool LitJson.IJsonWrapper.IsBoolean
            {
                get { throw new NotImplementedException(); }
            }

            bool LitJson.IJsonWrapper.IsDouble
            {
                get { throw new NotImplementedException(); }
            }

            bool LitJson.IJsonWrapper.IsInt
            {
                get { throw new NotImplementedException(); }
            }

            bool LitJson.IJsonWrapper.IsLong
            {
                get { throw new NotImplementedException(); }
            }

            bool LitJson.IJsonWrapper.IsObject
            {
                get { throw new NotImplementedException(); }
            }

            bool LitJson.IJsonWrapper.IsString
            {
                get { throw new NotImplementedException(); }
            }

            void LitJson.IJsonWrapper.SetBoolean(bool val)
            {
                throw new NotImplementedException();
            }

            void LitJson.IJsonWrapper.SetDouble(double val)
            {
                throw new NotImplementedException();
            }

            void LitJson.IJsonWrapper.SetInt(int val)
            {
                throw new NotImplementedException();
            }

            void LitJson.IJsonWrapper.SetJsonType(LitJson.JsonType type)
            {
                throw new NotImplementedException();
            }

            void LitJson.IJsonWrapper.SetLong(long val)
            {
                throw new NotImplementedException();
            }

            void LitJson.IJsonWrapper.SetString(string val)
            {
                throw new NotImplementedException();
            }

            int System.Collections.IList.Add(object value)
            {
                throw new NotImplementedException();
            }

            void System.Collections.IList.Clear()
            {
                throw new NotImplementedException();
            }

            bool System.Collections.IList.Contains(object value)
            {
                throw new NotImplementedException();
            }

            int System.Collections.IList.IndexOf(object value)
            {
                throw new NotImplementedException();
            }

            void System.Collections.IList.Insert(int index, object value)
            {
                throw new NotImplementedException();
            }

            bool System.Collections.IList.IsFixedSize
            {
                get { throw new NotImplementedException(); }
            }

            bool System.Collections.IList.IsReadOnly
            {
                get { throw new NotImplementedException(); }
            }

            void System.Collections.IList.Remove(object value)
            {
                throw new NotImplementedException();
            }

            void System.Collections.IList.RemoveAt(int index)
            {
                throw new NotImplementedException();
            }

            object System.Collections.IList.this[int index]
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            void System.Collections.ICollection.CopyTo(Array array, int index)
            {
                throw new NotImplementedException();
            }

            int System.Collections.ICollection.Count
            {
                get { throw new NotImplementedException(); }
            }

            bool System.Collections.ICollection.IsSynchronized
            {
                get { throw new NotImplementedException(); }
            }

            object System.Collections.ICollection.SyncRoot
            {
                get { throw new NotImplementedException(); }
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }

            System.Collections.IDictionaryEnumerator System.Collections.Specialized.IOrderedDictionary.GetEnumerator()
            {
                throw new NotImplementedException();
            }

            void System.Collections.Specialized.IOrderedDictionary.Insert(int index, object key, object value)
            {
                throw new NotImplementedException();
            }

            void System.Collections.Specialized.IOrderedDictionary.RemoveAt(int index)
            {
                throw new NotImplementedException();
            }

            object System.Collections.Specialized.IOrderedDictionary.this[int index]
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            void System.Collections.IDictionary.Add(object key, object value)
            {
                throw new NotImplementedException();
            }

            void System.Collections.IDictionary.Clear()
            {
                throw new NotImplementedException();
            }

            bool System.Collections.IDictionary.Contains(object key)
            {
                throw new NotImplementedException();
            }

            System.Collections.IDictionaryEnumerator System.Collections.IDictionary.GetEnumerator()
            {
                throw new NotImplementedException();
            }

            bool System.Collections.IDictionary.IsFixedSize
            {
                get { throw new NotImplementedException(); }
            }

            bool System.Collections.IDictionary.IsReadOnly
            {
                get { throw new NotImplementedException(); }
            }

            System.Collections.ICollection System.Collections.IDictionary.Keys
            {
                get { throw new NotImplementedException(); }
            }

            void System.Collections.IDictionary.Remove(object key)
            {
                throw new NotImplementedException();
            }

            System.Collections.ICollection System.Collections.IDictionary.Values
            {
                get { throw new NotImplementedException(); }
            }

            object System.Collections.IDictionary.this[object key]
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }
            #endregion
        }

        private class JsonCategory : models.category,LitJson.IJsonWrapper
        {
            public JsonCategory(models.category cate)
                : base()
            {
                base.id = cate.id;
                base.name = cate.name;
                base.code = cate.code;
                base.level = cate.level;
                base.fullpath = cate.fullpath;
                base.parent = cate.parent;
                base.image = cate.image;
                base.description = cate.description;
                base.subCategory.AddRange(cate.subCategory);
                base.keywords = cate.keywords;
                base.content = cate.content;
                base.page = cate.page;
                base.contentpage = cate.contentpage;
            }

            void LitJson.IJsonWrapper.ToJson(LitJson.JsonWriter writer)
            {
                new LitJson.JsonData(base.fullpath).ToJson(writer);
            }

            string LitJson.IJsonWrapper.ToJson()
            {
                return new LitJson.JsonData(base.fullpath).ToJson();
            }

            #region MyRegion
            

            bool LitJson.IJsonWrapper.GetBoolean()
            {
                throw new NotImplementedException();
            }

            double LitJson.IJsonWrapper.GetDouble()
            {
                throw new NotImplementedException();
            }

            int LitJson.IJsonWrapper.GetInt()
            {
                throw new NotImplementedException();
            }

            LitJson.JsonType LitJson.IJsonWrapper.GetJsonType()
            {
                throw new NotImplementedException();
            }

            long LitJson.IJsonWrapper.GetLong()
            {
                throw new NotImplementedException();
            }

            string LitJson.IJsonWrapper.GetString()
            {
                throw new NotImplementedException();
            }

            bool LitJson.IJsonWrapper.IsArray
            {
                get { throw new NotImplementedException(); }
            }

            bool LitJson.IJsonWrapper.IsBoolean
            {
                get { throw new NotImplementedException(); }
            }

            bool LitJson.IJsonWrapper.IsDouble
            {
                get { throw new NotImplementedException(); }
            }

            bool LitJson.IJsonWrapper.IsInt
            {
                get { throw new NotImplementedException(); }
            }

            bool LitJson.IJsonWrapper.IsLong
            {
                get { throw new NotImplementedException(); }
            }

            bool LitJson.IJsonWrapper.IsObject
            {
                get { throw new NotImplementedException(); }
            }

            bool LitJson.IJsonWrapper.IsString
            {
                get { throw new NotImplementedException(); }
            }

            void LitJson.IJsonWrapper.SetBoolean(bool val)
            {
                throw new NotImplementedException();
            }

            void LitJson.IJsonWrapper.SetDouble(double val)
            {
                throw new NotImplementedException();
            }

            void LitJson.IJsonWrapper.SetInt(int val)
            {
                throw new NotImplementedException();
            }

            void LitJson.IJsonWrapper.SetJsonType(LitJson.JsonType type)
            {
                throw new NotImplementedException();
            }

            void LitJson.IJsonWrapper.SetLong(long val)
            {
                throw new NotImplementedException();
            }

            void LitJson.IJsonWrapper.SetString(string val)
            {
                throw new NotImplementedException();
            }

            int System.Collections.IList.Add(object value)
            {
                throw new NotImplementedException();
            }

            void System.Collections.IList.Clear()
            {
                throw new NotImplementedException();
            }

            bool System.Collections.IList.Contains(object value)
            {
                throw new NotImplementedException();
            }

            int System.Collections.IList.IndexOf(object value)
            {
                throw new NotImplementedException();
            }

            void System.Collections.IList.Insert(int index, object value)
            {
                throw new NotImplementedException();
            }

            bool System.Collections.IList.IsFixedSize
            {
                get { throw new NotImplementedException(); }
            }

            bool System.Collections.IList.IsReadOnly
            {
                get { throw new NotImplementedException(); }
            }

            void System.Collections.IList.Remove(object value)
            {
                throw new NotImplementedException();
            }

            void System.Collections.IList.RemoveAt(int index)
            {
                throw new NotImplementedException();
            }

            object System.Collections.IList.this[int index]
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            void System.Collections.ICollection.CopyTo(Array array, int index)
            {
                throw new NotImplementedException();
            }

            int System.Collections.ICollection.Count
            {
                get { throw new NotImplementedException(); }
            }

            bool System.Collections.ICollection.IsSynchronized
            {
                get { throw new NotImplementedException(); }
            }

            object System.Collections.ICollection.SyncRoot
            {
                get { throw new NotImplementedException(); }
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }

            System.Collections.IDictionaryEnumerator System.Collections.Specialized.IOrderedDictionary.GetEnumerator()
            {
                throw new NotImplementedException();
            }

            void System.Collections.Specialized.IOrderedDictionary.Insert(int index, object key, object value)
            {
                throw new NotImplementedException();
            }

            void System.Collections.Specialized.IOrderedDictionary.RemoveAt(int index)
            {
                throw new NotImplementedException();
            }

            object System.Collections.Specialized.IOrderedDictionary.this[int index]
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            void System.Collections.IDictionary.Add(object key, object value)
            {
                throw new NotImplementedException();
            }

            void System.Collections.IDictionary.Clear()
            {
                throw new NotImplementedException();
            }

            bool System.Collections.IDictionary.Contains(object key)
            {
                throw new NotImplementedException();
            }

            System.Collections.IDictionaryEnumerator System.Collections.IDictionary.GetEnumerator()
            {
                throw new NotImplementedException();
            }

            bool System.Collections.IDictionary.IsFixedSize
            {
                get { throw new NotImplementedException(); }
            }

            bool System.Collections.IDictionary.IsReadOnly
            {
                get { throw new NotImplementedException(); }
            }

            System.Collections.ICollection System.Collections.IDictionary.Keys
            {
                get { throw new NotImplementedException(); }
            }

            void System.Collections.IDictionary.Remove(object key)
            {
                throw new NotImplementedException();
            }

            System.Collections.ICollection System.Collections.IDictionary.Values
            {
                get { throw new NotImplementedException(); }
            }

            object System.Collections.IDictionary.this[object key]
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }
            #endregion
        }

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
    }
}
