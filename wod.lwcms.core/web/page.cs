using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using wod.lwcms.services;
using wod.lwcms.dataaccess;
using System.IO;

namespace wod.lwcms.web
{
    public class wodPagebase : System.Web.UI.Page
    { 
        protected ioc getIoc()
        {
            return pageIoc.getIoc(this.Request);
        }
    }

    public class page : wodPagebase
    {
        public string tempName { get; private set; }

        protected string path;

        public pageData PD { get; set; }

        protected override void OnInit(EventArgs e)
        {
            this.path = Request.QueryString["path"];
            ioc _ioc = base.getIoc();

            tempName = _ioc.GetInstance<string>("siteKey");

            commands.commandPool pool = _ioc.GetInstance<commands.commandPool>("__commandPool");
            objectPool po = _ioc.GetService<objectPool>();
            pageParameter pParameter = pageParameter.Parse(Request, ref po);


            commands.commandsParameter cp = new commands.commandsParameter(_ioc, po);
            cp.AddObject("cp", cp);
            cp.AddObject("context", Context);
            cp.AddObject("pageCommandId", pParameter.pageCommandId);
            var cmd = pool.getCommand(pParameter.pageCommandId);
            cmd.excute(cp);

            PD = new pageData(po) { pageType = pParameter.pageType };
            PDLoad();

            if (!string.IsNullOrEmpty(PD.pageTransferName))
            {
                Server.Transfer(string.Format("~/svrtheme/{0}/{1}.aspx", tempName, PD.pageTransferName));
            }
            else
            {
                ajaxResult result = new ajaxResult();
                try
                {
                    result.status = true;
                    result.result = po.getObject("result");
                }
                catch (Exception ex)
                {
                    result.status = false;
                    result.message = ex.Message;
                }
                Response.Clear();
                Response.ContentType = "text/json";
                Response.Write(common.ToJson(result));
                Response.End();
            }
        }

        protected virtual void PDLoad()
        {
        }
    }

    public class tempPage : Page
    {
        protected override void OnPreInit(EventArgs e)
        {
            var page = this.PreviousPage as page;
            if (page != null)
            {
                PD = page.PD;
            }
            else
            {
                throw new System.Web.HttpException(404, "不存在该页面");
            }
            if (PD.op.getObject<bool>("_editable"))
            {
                this.MasterPageFile = "~/svrtheme/" + tempName + "/editorSite.master";
            }
            base.OnPreInit(e);
        }

        public string tempName
        {
            get { return (this.PreviousPage as page).tempName; }
        }

        public pageData PD { get; set; }

        public string EcHtml(string content)
        {
            return Server.HtmlEncode(content);
        }
        public string EcUrl(string content)
        {
            return Server.UrlEncode(content);
        }

        public string RenderPartView(partView view)
        {
            switch (view.type)
            {
                case partView.viewType.control:
                    return RenderControlPartView(view.data);
                default:
                    break;
            }
            return "";
        }

        private string RenderControlPartView(string subPath)
        {
            Control control = this.LoadControl("~/svrtheme/" + tempName + "/" + subPath);
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                control.RenderControl(hw);
                return sw.ToString();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        public override void Dispose()
        {
            PD.op.Dispose();
            base.Dispose();
        }
    }
}
