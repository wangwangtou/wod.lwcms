using System;
using System.Collections.Generic;
using System.Text;

namespace wod.lwcms.dataaccess
{
    public interface ISiteDataAccess
    {
        models.wodsite GetSite(string _siteKey);

        void SaveSite(string _siteKey, models.wodsite site);
    }
}
