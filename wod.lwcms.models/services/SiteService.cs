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
    }
}
