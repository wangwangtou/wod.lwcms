using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using wod.lwcms.services;

namespace wod.lwcms.web
{
    public class authModule :IHttpModule
    {
        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.AuthenticateRequest += new EventHandler(context_AuthenticateRequest);
        }

        void context_AuthenticateRequest(object sender, EventArgs e)
        {
            HttpApplication app = (sender as HttpApplication);
            if (app.Context != null)
            {
                ioc _ioc = pageIoc.getIoc(app.Context.Request);
                commands.commandsParameter cp = new commands.commandsParameter(_ioc, new  objectPool());
                cp.AddObject("context", app.Context);
                IAuthenticationService auth = cp.GetObject(typeof(IAuthenticationService)) as IAuthenticationService;
            }
        }
    }
}
