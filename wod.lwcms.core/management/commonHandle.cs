using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using wod.lwcms.web;

namespace wod.lwcms.management
{
    /// <summary>
    /// common 的摘要说明
    /// </summary>
    public class commonHandle : wodPagebase, IHttpHandler
    {
        protected override void OnLoad(EventArgs e)
        {
            var context = Context;
            var cmdId = context.Request.QueryString["command"];

            objectPool po = _ioc.GetService<objectPool>();

            foreach (string key in context.Request.QueryString)
            {
                po.setOjbect(key, context.Request.QueryString[key]);
            }
            foreach (string key in context.Request.Form)
            {
                po.setOjbect(key, context.Request.Form[key]);
            }

            commands.commandsParameter cp = new commands.commandsParameter(_ioc, po);
            cp.AddObject("cp", cp);
            var cmd = commands.commandPool.getCommand(cmdId);
            ajaxResult result = new ajaxResult();
            try
            {
                cmd.excute(cp);
                result.status = true;
                result.result = po.getObject("result");
            }
            catch (Exception ex)
            {
                result.status = false;
                result.message = ex.Message;
            }
            po.Dispose();
            context.Response.Clear();
            context.Response.ContentType = "text/json";
            context.Response.Write(common.ToJson(result));
            context.Response.End();
        }
    }
}
