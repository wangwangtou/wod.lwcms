using System;
using System.Collections.Generic;
using System.Text;

namespace wod.lwcms.management
{
    public class baseadpage : wod.lwcms.web.page
    {
        protected override void PDLoad()
        {
            base.PDLoad();
            PD.op.setOjbect("_editable", true);
        }
    }
}
