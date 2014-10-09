using System;
using System.Collections.Generic;
using System.Text;
using wod.lwcms.commands;

namespace wod.lwcms.addin
{
    public class vercodeValidation : IUserValidation
    {
        private services.IAuthenticationService authService;
        public vercodeValidation(services.IAuthenticationService authService)
        {
            this.authService = authService;
        }

        public bool validate(string value, List<string> validParameters, commandsParameter cp)
        {
            return authService.CheckVerCode(value);
        }

        public object convert(string value, string convertType, commandsParameter cp)
        {
            return value;
        }
    }
}
