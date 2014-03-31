using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;

namespace wod.lwcms.management
{
    public class pagebase : wod.lwcms.web.wodPagebase
    {
        public pagebase()
        {
            ServerParams = new Dictionary<string, object>();
        }

        protected override void OnInit(EventArgs e)
        {
            string fn = new System.IO.FileInfo(Request.FilePath).Name;
            string pagename = fn.Substring(0, fn.Length - 5);
            ajaxCmd = pagename;
            loadCmd = pagename;
        }

        public Dictionary<string, object> ServerParams { get; private set; }

        public virtual string ajaxCmd { get; set; }
        public virtual string loadCmd { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (Request.Form.Count > 0)
            {
                PreCommand();
                ExcuteCommand();
                AfterCommand();
            }
            else
            {
                PreLoadCommand();
                LoadCommand();
                AfterLoadCommand();
            }
        }

        public objectPool PD { get; set; }
        private void LoadCommand()
        {
            if (!string.IsNullOrEmpty(loadCmd))
            {
                objectPool po = _ioc.GetService<objectPool>();
                InitPo(po, Request.QueryString, ServerParams);
                commands.commandsParameter cp = new commands.commandsParameter(_ioc, po);
                cp.AddObject("cp", cp);
                var cmd = commands.commandPool.getCommand("management_"+loadCmd);
                cmd.excute(cp);
                PD = po;
            }
        }

        protected virtual void PreLoadCommand()
        {
        }
        protected virtual void AfterLoadCommand()
        {
        }

        private void ExcuteCommand()
        {
            objectPool po = _ioc.GetService<objectPool>();
            InitPo(po, Request.Form,ServerParams);
            commands.commandsParameter cp = new commands.commandsParameter(_ioc, po);
            cp.AddObject("cp", cp);
            var cmd = commands.commandPool.getCommand("management_" + ajaxCmd);
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
            Response.Clear();
            Response.ContentType = "text/json";
            Response.Write(common.ToJson(result));
            Response.End();
        }

        private void InitPo(objectPool po, System.Collections.Specialized.NameValueCollection formData, Dictionary<string, object> serverParams)
        {
            foreach (string key in formData.AllKeys)
            {
                po.setOjbect(key, GetCorrectTypeObj(formData, key));
            }
            foreach (var key in serverParams.Keys)
            {
                po.setOjbect(key, serverParams[key]);
            }
        }

        protected virtual void PreCommand()
        {
        }
        protected virtual void AfterCommand()
        {
        }

        private object GetCorrectTypeObj(System.Collections.Specialized.NameValueCollection items, string item)
        {
            if (itemTypes.ContainsKey(item))
            {
                return Convert.ChangeType(items[item], itemTypes[item]);
            }
            else
            {
                return items[item];
            }
        }

        protected readonly Dictionary<string, Type> itemTypes = new Dictionary<string, Type>()
        {
            {"pageIndex",typeof(int)}
        };

        public override void Dispose()
        {
            base.Dispose();
            if (PD != null)
                PD.Dispose();
        }
    }



    public class tempControl : UserControl
    {
        public static string ResourceUrl(string path)
        {
            return System.Web.VirtualPathUtility.GetDirectory("~/") + path;
        }

        public objectPool PD
        {
            get { return (Page as pagebase).PD; }
        }
    }

    public class tempMaster : MasterPage
    {
        public static string ResourceUrl(string path)
        {
            return System.Web.VirtualPathUtility.GetDirectory("~/") + path;
        }

        public objectPool PD
        {
            get { return (Page as pagebase).PD; }
        }
    }
}
