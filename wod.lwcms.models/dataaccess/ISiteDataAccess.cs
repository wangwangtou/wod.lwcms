using System;
using System.Collections.Generic;
using System.Text;

namespace wod.lwcms.dataaccess
{
    public interface ISiteDataAccess
    {
        models.wodsite GetSite(string _siteKey);

        void SaveSite(string _siteKey, models.wodsite site);

        models.siteAttribute GetCommonAttribute();

        models.siteAttribute GetSiteAttribute(string _siteKey);

        void SaveSiteAttribute(string _siteKey, models.siteAttribute attrs);
    }
}
