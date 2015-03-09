using System;
using System.Collections.Generic;
using System.Text;
using wod.lwcms.addin;
using System.Web.Caching;

namespace wod.lwcms.addins.extension
{
    public class cacheable: extensionBase,IAddin
    {
        private Cache _cache;

        public cacheable()
        {
            this._cache = System.Web.HttpRuntime.Cache;
        }

        public override string name
        {
            get { return "cacheable"; }
        }

        public string description
        {
            get { return "缓存"; }
        }

        public Dictionary<Type, Type> getServices()
        {
            return new Dictionary<Type, Type>();
        }

        public string initAlias()
        {
            return base.getPath("aliasResource.xml");
        }

        public Dictionary<string, commands.command> initCommands(aliasResource resource)
        {
            return new Dictionary<string, commands.command>();
        }

        public commands.command getAfterCommand(string pageCommandId)
        {
            if (base.enable)
            {
                switch (pageCommandId)
                {
                    case "index":
                    case "category":
                    case "article":
                        return new addCacheCommand(_cache);
                    default:
                        break;
                }
            }
            return new commands.emptyCommand();
        }

        public commands.command getBeforeCommand(string pageCommandId)
        {
            if (base.enable)
            {
                switch (pageCommandId)
                {
                    case "index":
                    case "category":
                    case "article":
                        return new getFromCacheCommand(_cache);
                    default:
                        break;
                }
            }
            return new commands.emptyCommand();
        }

        private abstract class cacheCommand : commands.command
        { 
            protected Cache _cache;
            public cacheCommand(Cache _cache)
            {
                this._cache = _cache;
            }

            protected string getCacheKey(commands.commandsParameter cp)
            {
                object key = cp.GetObject("requestKey");
                return key == null ? null : key.ToString();
            }
        }

        private class getFromCacheCommand : cacheCommand
        {
            public getFromCacheCommand(Cache _cache):base(_cache)
            {
            }

            protected override void excuteNoCheck(commands.commandsParameter cp)
            {
                string key = getCacheKey(cp);
                if (!string.IsNullOrEmpty(key))
                {
                    Dictionary<string, object> _cacheObject = _cache.Get(key.ToString()) as Dictionary<string, object>;
                    if (_cacheObject != null)
                    {
                        putCache2CP(_cacheObject, cp);
                        cp.AddObject("__breakall", true);
                    }
                }
            }

            private void putCache2CP(Dictionary<string, object> _cacheObject, commands.commandsParameter cp)
            {
                var opPool = cp.OP.Pool;
                string siteKey = cp.GetObject("siteKey") as string;
                foreach (string key in _cacheObject.Keys)
                {
                    if (opPool.ContainsKey(key))
                    {
#warning 把一些上下文相关也Cache了，赋值后出问题。。。  例如HttpContext错误导致登录失效
                        //opPool[key] = _cacheObject[key];
                    }
                    else
                    {
                        opPool.Add(siteKey + "_" + key.ToString(), _cacheObject[key]);
                    }
                }
            }
        }

        private class addCacheCommand : cacheCommand
        {
            public addCacheCommand(Cache _cache)
                : base(_cache)
            {
            }
            protected override void excuteNoCheck(commands.commandsParameter cp)
            {
                string key = getCacheKey(cp);
                string siteKey = cp.GetObject("siteKey") as string;
                if (!string.IsNullOrEmpty(key))
                {
                    Dictionary<string, object> _cacheObject = getFromCP(cp);
                    _cache.Insert(siteKey+ "_"+key.ToString(), _cacheObject, getDependency(cp));
                }
            }

            private CacheDependency getDependency(commands.commandsParameter cp)
            {
                cacheableResource resource = cp.GetObject(typeof(cacheableResource)) as cacheableResource;
                return new CacheDependency(null, new string[] { resource.allCategoriesKey, resource.wodSiteKey });
            }

            private Dictionary<string, object> getFromCP(commands.commandsParameter cp)
            {
                return cp.OP.Pool;
            }
        }
    }
}
