using System;
using System.Collections.Generic;
using System.Text;

namespace wod.lwcms.dataaccess
{
    public class pagerHelper
    {
        public int getStartRowIndex(int totalCount,int pageIndex, int pageSize,commands.commandsParameter cp)
        {
            pageIndex = pageIndex - 1;

            if (pageIndex < 0)
                pageIndex = 0;
            if (pageIndex * pageSize > totalCount)
            {
                pageIndex = (int)Math.Ceiling((double)totalCount / pageSize);
            }
            cp.AddObject("pageIndex", pageIndex);
            return pageIndex * pageSize + 1;
        }

        public int getEndRowIndex(int totalCount,int pageIndex, int pageSize)
        {
            return Math.Max( Math.Min(totalCount, (pageIndex + 1) * pageSize) , 1);
        }
    }
}
