using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Drawing;

namespace wod.lwcms.services
{
    public class AuthenticationService : IAuthenticationService
    {
        private HttpContext context;
        public AuthenticationService(HttpContext context)
        {
            this.context = context;
            if (IsLogin())
            {
                //System.Web.HttpCookie cookie = context.Request.Cookies[System.Web.Security.FormsAuthentication.FormsCookieName];
                //if (cookie != null)
                //{
                    //System.Web.Security.FormsAuthenticationTicket ticket = System.Web.Security.FormsAuthentication.Decrypt(cookie.Value);
                    //this.context.User = new System.Security.Principal.GenericPrincipal(new System.Web.Security.FormsIdentity(ticket), ticket.UserData.Split(','));

                    System.Web.Security.FormsIdentity fi = (System.Web.Security.FormsIdentity)context.User.Identity;
                    System.Web.Security.FormsAuthenticationTicket ticket = fi.Ticket;
                    string userData = ticket.UserData;
                    string[] roles = userData.Split(',');
                    context.User = new System.Security.Principal.GenericPrincipal(fi, roles);
                //}
            }
        }

        public string HashPassword(string password)
        {
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(password, "MD5");
        }

        public void Login(string account, string roles, bool isRemember)
        {
            DateTime expiration = DateTime.Now.AddMinutes(20);
            if (isRemember)
            {
                expiration = DateTime.Now.AddMonths(20);
            }
            string cryptString = System.Web.Security.FormsAuthentication.Encrypt(new System.Web.Security.FormsAuthenticationTicket(1, account, DateTime.Now, expiration, isRemember, roles));
            System.Web.HttpCookie cookie = new System.Web.HttpCookie(System.Web.Security.FormsAuthentication.FormsCookieName);
            cookie.Expires = expiration;
            cookie.Value = cryptString;
            context.Response.Cookies.Add(cookie);
        }
        
        public void Logout()
        {
            System.Web.HttpCookie cookie = new System.Web.HttpCookie(System.Web.Security.FormsAuthentication.FormsCookieName);
            cookie.Expires = DateTime.Now.AddYears(-10);
            context.Response.Cookies.Add(cookie);
            context.User = null;
            context.Response.Redirect("~/index.aspx");
        }

        public string TryLogin(int validuser, string account, string roles, bool isRemember)
        {
            if (validuser == 0)
                return "用户名或密码错误！";
            else
                Login(account, roles, isRemember);
                return "";
        }


        public string GetLoginName()
        {
            if (this.context.User != null && this.context.User.Identity != null && this.context.User.Identity.IsAuthenticated)
                return this.context.User.Identity.Name;
            else
                return "";
        }

        public bool IsLogin()
        {
            return this.context.User != null && this.context.User.Identity != null && this.context.User.Identity.IsAuthenticated;
        }

    }
}
