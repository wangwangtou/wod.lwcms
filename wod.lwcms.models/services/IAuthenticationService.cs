using System;
using System.Collections.Generic;
using System.Text;

namespace wod.lwcms.services
{
    public interface IAuthenticationService
    {
        string HashPassword(string password);

        void Login(string account, string roles, bool isRemember);

        void Logout();

        string TryLogin(int validuser, string account, string roles, bool isRemember);

        string GetLoginName();

        bool IsLogin();
    }
}
