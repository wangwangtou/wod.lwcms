using System;
using System.Collections.Generic;
using System.Text;

namespace wod.lwcms.services
{
    public interface ISiteService
    {
        models.wodsite getSite();

        void updateSite(models.wodsite site);

        models.indexData getIndexData();
    }
}
