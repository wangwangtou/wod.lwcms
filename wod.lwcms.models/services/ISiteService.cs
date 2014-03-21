using System;
using System.Collections.Generic;
using System.Text;

namespace wod.lwcms.services
{
    public interface ISiteService
    {
        models.wodsite getSite();

        models.indexData getIndexData();
    }
}
