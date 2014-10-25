using System;
using System.Collections.Generic;
using System.Text;

namespace wod.lwcms.addins.extension
{
    public interface IVerCodeService
    {
        void GenerateVerCode();

        void ClearVerCode();

        bool CheckVerCode(string vercode); 
    }
}
