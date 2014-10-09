using System;
using System.Collections.Generic;
using System.Text;

namespace wod.lwcms.services
{
    public class GenerateService:IGenerateService
    {
        public string GenerateCode()
        {
            string code = DateTime.Now.ToString("yyyyMMddHH");

            code += (DateTime.Now.TimeOfDay.Ticks % 1000000);
            return code;
        }
    }
}
