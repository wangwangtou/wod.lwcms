using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using wod.lwcms.dataaccess;

namespace wod.lwcms.services
{
    public class SiteService : ISiteService
    {
        private string _siteKey;
        private ISiteDataAccess _da;

        public SiteService(string siteKey, ISiteDataAccess da)
        {
            this._siteKey = siteKey;
            this._da = da;
        }

        public models.wodsite getSite()
        {
            return _da.GetSite(_siteKey);
        }

        public models.indexData getIndexData()
        {
            throw new NotImplementedException();
        }


        public void updateSite(models.wodsite site)
        {
            _da.SaveSite(_siteKey, site);
        }


        public models.siteAttribute getCommonAttribute()
        {
            return _da.GetCommonAttribute();
        }

        public models.siteAttribute getSiteAttribute()
        {
            models.siteAttribute attrs = _da.GetCommonAttribute();
            models.siteAttribute attrs1 = _da.GetSiteAttribute(_siteKey);
            attrs1.ioc.InsertRange(0, attrs.ioc);
            attrs1.svrTemplates = attrs.svrTemplates;
            attrs1.themes = attrs.themes;
            return attrs1;
        }

        public void updateSiteAttribute(models.siteAttribute attrs)
        {
            _da.SaveSiteAttribute(_siteKey, attrs);
        }
    }
}
