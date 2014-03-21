using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using wod.lwcms.services;
using wod.lwcms.dataaccess;

namespace wod.lwcms.web
{
    public class wodPagebase : System.Web.UI.Page
    { 
        protected static ioc _ioc;

        static wodPagebase()
        {
            _ioc = new ioc();

            _ioc.RegistInstance("siteKey", wodEnvironment.siteKey);
            _ioc.RegistInstance("pageSize", 20);
            _ioc.RegistInstance("pageIndex", 0);
            _ioc.RegistInstance("connectionString", "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + wodEnvironment.GetDataPath("lwcms.mdb")+";User Id=;Password=;");
            _ioc.RegistInstance("dbProviderFactory", System.Data.Common.DbProviderFactories.GetFactory("System.Data.OleDb"));

            _ioc.Regist(typeof(ISiteService), typeof(SiteService));
            _ioc.Regist(typeof(ICategoryService), typeof(CategoryService));

            _ioc.Regist(typeof(ISiteDataAccess), typeof(SiteDataAccess));
            _ioc.Regist(typeof(ICategoryDataAccess), typeof(CategoryDataAccess));
            _ioc.Regist(typeof(ICommonDataAccess), typeof(CommonDataAccess));

            commands.commandPool.init(wodEnvironment.GetDataPath("commands"));
        }
    }

    public class page : wodPagebase
    {
        public static string tempName { get { return "default"; } }

        protected string path;

        public pageData PD { get; set; }

        protected override void OnInit(EventArgs e)
        {
            this.path = Request.QueryString["path"];
            objectPool po = _ioc.GetService<objectPool>();
            pageParameter pParameter = pageParameter.Parse(Request, ref po);

            commands.commandsParameter cp = new commands.commandsParameter(_ioc, po);
            cp.AddObject("cp", cp);
            var cmd = commands.commandPool.getCommand(pParameter.pageCommandId);
            cmd.excute(cp);

            PD = new pageData(po) { pageType = pParameter.pageType };
            PDLoad();
            Server.Transfer(string.Format("~/svrtheme/{0}/{1}.aspx", tempName, PD.pageTransferName));
        }

        protected virtual void PDLoad()
        {
        }
    }

    public class tempPage : Page
    {
        public pageData PD { get; set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            var page = this.PreviousPage as page;
            if (page != null)
            {
                PD = page.PD;
            }
            else
            {
                throw new System.Web.HttpException(404, "不存在该页面");
            }
        }

        public override void Dispose()
        {
            PD.op.Dispose();
            base.Dispose();
        }
    }
}
