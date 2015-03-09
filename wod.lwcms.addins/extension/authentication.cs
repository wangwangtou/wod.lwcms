﻿using System;
using System.Collections.Generic;
using System.Text;
using wod.lwcms.addin;
using wod.lwcms.services;

namespace wod.lwcms.addins.extension
{
    public class authentication : extensionBase,IAddin
    {
        public override string name
        {
            get { return "authentication"; }
        }

        public string description
        {
            get { return "用户注册"; }
        }

        public Dictionary<string, commands.command> initCommands(aliasResource resource)
        {
            return commands.commandPool.LoadCommand(resource,base.getPath("cmds.xml"));
        }

        public string initAlias()
        {
            return null;
        }

        public commands.command getBeforeCommand(string pageCommandId)
        {
            return base.enable ? 
                new loginLinkCommand(AddPartView) as commands.command : 
                new commands.emptyCommand();
        }

        public commands.command getAfterCommand(string commandName)
        {
            return new commands.emptyCommand();
        }

        private class loginLinkCommand : commands.command
        {
            private AddPartViewDelegate addpartview;

            public loginLinkCommand(AddPartViewDelegate addpartview)
            {
                this.addpartview = addpartview;
            }

            private static readonly web.partView authpartView = new web.partView()
            {
                data = "authentication/auth_status.ascx",
                name = "authentication",
                type = web.partView.viewType.control,
                cssClass = ""
            };

            public override bool canExcute(commands.commandsParameter cp)
            {
                //return base.canExcute(cp) && cp.GetObject("ws") != null;
                return true;//不会受breakall影响
            }
            protected override void excuteNoCheck(commands.commandsParameter cp)
            {
                IAuthenticationService service = cp.GetObject(typeof(IAuthenticationService)) as IAuthenticationService;

                addpartview(cp, authpartView, web.partView.viewPos.statusbar);

                if (!service.IsLogin())
                {
                    cp.AddObject("auth_islogin", false);
                    //ws.navis["top"].AddRange(new List<models.siteNavi> 
                    //{
                    //    new models.siteNavi(){ name="登录", naviUrl="index.aspx?path=/common/authentication/login", target="" },
                    //    new models.siteNavi(){ name="注册", naviUrl="index.aspx?path=/common/authentication/regist", target="" },
                    //});
                }
                else
                {
                    cp.AddObject("auth_islogin", true);
                    cp.AddObject("auth_uname", service.GetLoginName());
                    //ws.navis["top"].AddRange(new List<models.siteNavi> 
                    //{
                    //    new models.siteNavi(){ name=service.GetLoginName(), naviUrl="index.aspx?path=/common/authentication/userinfo", target="" },
                    //    new models.siteNavi(){ name="退出", naviUrl="index.aspx?path=/common/authentication/logout", target="" },
                    //});
                }
            }

        }

        public Dictionary<Type, Type> getServices()
        {
            return new Dictionary<Type, Type>()
            {
                {typeof(IVerCodeService),typeof(VerCodeService)}
            };
        }

    }
}
