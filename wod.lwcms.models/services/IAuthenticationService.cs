using System;
using System.Collections.Generic;
using System.Text;

namespace wod.lwcms.services
{
    public interface IAuthenticationService
    {
        string HashPassword(string password);

        void Login(string userName, string roles, bool isRemember);

        string TryLogin(int validuser,string userName, string roles, bool isRemember);
    }
}
