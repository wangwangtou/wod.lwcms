using System;
using System.Collections.Generic;
using System.Text;

namespace wod.lwcms.services
{
    public class AuthenticationService : IAuthenticationService
    {
        public string HashPassword(string password)
        {
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(password, "MD5");
        }

        public void Login(string userName, string roles, bool isRemember)
        {
            DateTime expiration = DateTime.Now.AddMinutes(20);
            if (isRemember)
            {
                expiration = DateTime.Now.AddMonths(20);
            }
            string cryptString = System.Web.Security.FormsAuthentication.Encrypt(new System.Web.Security.FormsAuthenticationTicket(1, userName, DateTime.Now, expiration, isRemember, roles));
            System.Web.HttpCookie cookie = new System.Web.HttpCookie(System.Web.Security.FormsAuthentication.FormsCookieName);
            cookie.Expires = expiration;
            cookie.Value = cryptString;
            System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
        }


        public string TryLogin(int validuser, string userName, string roles, bool isRemember
            )
        {
            if (validuser == 0)
                return "用户名或密码错误！";
            else
                Login(userName, roles, isRemember);
                return "";
        }
    }
}
