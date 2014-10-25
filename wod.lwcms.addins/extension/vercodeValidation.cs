using System;
using System.Collections.Generic;
using System.Text;
using wod.lwcms.commands;

namespace wod.lwcms.addins.extension
{
    public class vercodeValidation : IUserValidation
    {
        private IVerCodeService authService;
        public vercodeValidation(IVerCodeService authService)
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
