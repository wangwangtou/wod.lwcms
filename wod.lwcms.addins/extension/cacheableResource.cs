using System;
using System.Collections.Generic;
using System.Text;
using wod.lwcms.services;
using System.Web.Caching;
using System.Web;

namespace wod.lwcms.addins.extension
{
    public class cacheableResource
    {
        private ISiteService siteService;
        private ICategoryService categoryService;
        private Cache _cache;
        private string siteKey;

        public cacheableResource(string siteKey,ISiteService siteService, ICategoryService categoryService)
        {
            this.siteKey = siteKey;
            this.siteService = siteService;
            this.categoryService = categoryService;
            this._cache = HttpRuntime.Cache;
        }

        public string wodSiteKey { get { return siteKey + "_wod.lwcms.models.wodsite"; } }

        public wod.lwcms.models.wodsite getSite()
        {
            return GetFromKey(wodSiteKey, new CacheDependency(wodEnvironment.GetDataPath("site.xml")), () => siteService.getSite());
        }

        protected delegate T InitData<T>();

        protected T GetFromKey<T>(string key,CacheDependency dependency, InitData<T> initData) where T:class
        {
            object _cacheObject = _cache.Get(key);
            if (_cacheObject != null)
            {
            }
            else
            {
                _cacheObject = initData();
                _cache.Insert(key, _cacheObject, dependency);
            }
            return _cacheObject as T;
        }

        public string allCategoriesKey { get { return siteKey + "_wod.lwcms.models.categories"; } }

        public List<models.category> getAllCategories()
        {
            return GetFromKey(allCategoriesKey, new CacheDependency(wodEnvironment.GetDataPath(siteKey,"category.xml")), () => categoryService.getAllCategories());
        }
    }
}
