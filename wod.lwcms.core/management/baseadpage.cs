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
            //Dictionary<string, string> hashes = new Dictionary<string, string>();
            //foreach (string key in PD.op.Pool.Keys)
            //{
            //    hashes.Add(key, common.GetHash(PD.op.Pool[key]));
            //}
            //PD.op.setOjbect("_hashes", hashes);
        }
    }
}
