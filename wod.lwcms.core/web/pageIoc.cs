using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web;
using wod.lwcms.services;
using wod.lwcms.dataaccess;
using System.Xml;

namespace wod.lwcms.web
{
    static class pageIoc
    {
        static pageIoc()
        {
            initIoc();
        }

        private static string getSiteKey(HttpRequest request)
        {
            string host = request.Headers["Host"] ?? "";
            string key = host.Split('.')[0];
            if (siteKeys.IndexOf(key) > -1)
                return key;
            else
                return siteKeys[0];
        }

        private static List<string> siteKeys = new List<string>();
        private static List<ioc> _iocs = new List<ioc>();

        private static void initIoc()
        {
            string keys = System.Configuration.ConfigurationManager.AppSettings["siteKeys"];
            if (!string.IsNullOrEmpty(keys))
            {
                foreach (var item in keys.Split(','))
                {
                    siteKeys.Add(item);
                    _iocs.Add(createIoc(item));
                }
            }
            if (siteKeys.Count == 0)
            {
                siteKeys.Add(wodEnvironment.siteKey);
                _iocs.Add(createIoc(wodEnvironment.siteKey));
            }
        }
        
        private static ioc createIoc(string siteKey)
        {
            ioc _ioc = new ioc();

            _ioc.RegistInstance("siteKey", siteKey);
            var appConfigXml = wodEnvironment.GetDataPath("application/app.xml");

            LoadApplicationConfig(siteKey,_ioc,appConfigXml);


            commands.commandPool pool = new commands.commandPool();
            aliasResource resource = new aliasResource(pool);
            resource.init(wodEnvironment.GetDataPath("alias"));
            if (siteKey != wodEnvironment.siteKey)
                resource.init(wodEnvironment.GetDataPath(siteKey, "alias"));

            _ioc.Regist(typeof(ISiteService), typeof(SiteService));
            _ioc.Regist(typeof(ISiteDataAccess), typeof(SiteDataAccess));

            ISiteService siteService = _ioc.GetService<ISiteService>();
            wod.lwcms.models.siteAttribute attr = siteService.getSiteAttribute();
            initIoc(_ioc,siteKey, attr.ioc, resource);

            _ioc.RegistInstance("__aliasResource", resource);

            #region 扩展
            var exts = loadExtensions(_ioc, attr.extensions);
            _ioc.RegistInstance("extensions", exts);

            foreach (var ext in exts)
            {
                //加载_ioc
                var svs = ext.getServices();
                if (svs != null)
                {
                    foreach (var item in svs.Keys)
                    {
                        _ioc.Regist(item, svs[item]);
                    }
                }
                //加载别名，插件中会重写别名
                string aliasFile = ext.initAlias();
                if (!string.IsNullOrEmpty(aliasFile))
                {
                    resource.addAlias(aliasFile);
                }
                pool.addAddinCommand(ext.initCommands(resource));
            }
            #endregion
            #region 插件
            
            var plugins = loadPlugins(_ioc, attr.plugins);
            _ioc.RegistInstance("plugins", plugins);

            foreach (var plugin in plugins)
            {
                //加载_ioc
                var svs = plugin.getServices();
                if (svs != null)
                {
                    foreach (var item in svs.Keys)
                    {
                        _ioc.Regist(item, svs[item]);
                    }
                }
                //加载别名，插件中会重写别名
                string aliasFile = plugin.initAlias();
                if (!string.IsNullOrEmpty(aliasFile))
                {
                    resource.addAlias(aliasFile);
                }
                pool.addAddinCommand(plugin.initCommands(resource));
            }
            #endregion

            resource.distinctAlias();
            pool.init(resource, wodEnvironment.GetDataPath("commands"));
            if (siteKey != wodEnvironment.siteKey)
                pool.init(resource, wodEnvironment.GetDataPath(siteKey, "commands"));

            _ioc.RegistInstance("__commandPool", pool);
            return _ioc;
        }

        private static void LoadApplicationConfig(string siteKey, ioc _ioc, string appConfigXml)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(appConfigXml);
            config.applicationParse.ParseIocsNode(_ioc, doc.SelectSingleNode("/app/defaults/icos"), (ins) => _ioc.RegistInstance(ins.name, ins.value), (abstractType) => _ioc.Regist(abstractType.abstractType, abstractType.realizeType));

            config.applicationParse.ParseIocsNode(_ioc, doc.SelectSingleNode("/app/site[@key=\"" + siteKey + "\"]/icos"), (ins) => _ioc.RegistInstance(ins.name, ins.value), (abstractType) => _ioc.Regist(abstractType.abstractType, abstractType.realizeType));
        }

        private static void initIoc(ioc _ioc,string siteKey, List<models.ioc> list, aliasResource resource)
        {
            foreach (var item in list)
            {
                switch (item.type)
                {
                    case wod.lwcms.models.ioc.ioctype.Instance:
                        _ioc.RegistInstance(item.name, ParseData(item.value, siteKey, item.datatype));
                        break;
                    case wod.lwcms.models.ioc.ioctype.Type:
                        Type target = resource.GetTypeByAlias(item.target);
                        if(target == null)
                            target = Type.GetType(item.target);
                        Type realize = resource.GetTypeByAlias(item.realize);
                        if(realize == null)
                            realize = Type.GetType(item.realize);
                        _ioc.Regist(target, realize);
                        break;
                    default:
                        break;
                }
            }
        }

        private static object ParseData(string value,string siteKey, models.ioc.iocdatatype iocdatatype)
        {
            object data = null;
            switch (iocdatatype)
            {
                case wod.lwcms.models.ioc.iocdatatype.Int:
                    data = int.Parse(value);
                    break;
                case wod.lwcms.models.ioc.iocdatatype.String:
                    System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("\\[rel:([^\\]]+)\\]");
                    value = reg.Replace(value, (mache) =>
                    {
                        return wodEnvironment.GetDataPath(siteKey, mache.Groups[1].Value);
                    });
                    data = value;
                    break;
                case wod.lwcms.models.ioc.iocdatatype.DbProviderFactory:
                    data = System.Data.Common.DbProviderFactories.GetFactory(value);
                    break;
                case wod.lwcms.models.ioc.iocdatatype.UnKown:
                    data = value;
                    break;
                default:
                    break;
            }
            return data;
        }

        private static List<addin.IAddin> loadPlugins(ioc _ioc, List<models.addin> plugins)
        {
            var plugin = new List<addin.IAddin>();
            foreach (var item in plugins)
            {
                Type extType = Type.GetType(item.type);
                addin.IAddin addin = _ioc.GetService(extType) as addin.IAddin;
                addin.setSetting(item.addinSettings);
                plugin.Add(addin);
            }
            return plugin;
        }

        private static List<addin.IAddin> loadExtensions(ioc _ioc, List<models.addin> extensions)
        {
            var exts = new List<addin.IAddin>();
            var ass = System.Reflection.Assembly.Load("wod.lwcms.addins");//.GetTypes();

            foreach (var item in extensions)
            {
                Type extType = ass.GetType("wod.lwcms.addins.extension." + item.name);
                addin.IAddin ext = _ioc.GetService(extType) as addin.IAddin;
                ext.setSetting(item.addinSettings);
                item.description = ext.description;
                exts.Add(ext);
            }

            //foreach (var type in types)
            //{
            //    if (extensions.IndexOf(type.Name) > -1
            //        && typeof(addin.IAddin).IsAssignableFrom(type)
            //        && type.Namespace == "wod.lwcms.addins.extension")
            //    {
            //        addin.IAddin ext = _ioc.GetService(type) as addin.IAddin;
            //        exts.Add(ext);
            //    }
            //}
            //exts.Sort((a1, a2) => extensions.IndexOf(a1.GetType().Name).CompareTo(extensions.IndexOf(a2.GetType().Name)));
            return exts;
        }

        internal static ioc getIoc(HttpRequest request)
        {
            string key = getSiteKey(request);
            return _iocs[siteKeys.IndexOf(key)];
        }
    }
}
