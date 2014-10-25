using System;
using System.Collections.Generic;
using System.Text;

namespace wod.lwcms.commands
{
    public class addinBeforeCommand : command
    {
        protected override void excuteNoCheck(commandsParameter cp)
        {
            List<addin.IAddin> addins = cp.GetObject("extensions", null) as List<addin.IAddin>;
            var pageCommandId = cp.GetObject("pageCommandId", null) as string;

            for (int i = 0; i < addins.Count; i++)
            {
                var cmd = addins[i].getBeforeCommand(pageCommandId);
                if (cmd != null && cmd.canExcute(cp))
                {
                    cmd.excute(cp);
                }
            }
        }
    }

    public class addinAfterCommand : command
    {
        protected override void excuteNoCheck(commandsParameter cp)
        {
            List<addin.IAddin> addins = cp.GetObject("extensions", null) as List<addin.IAddin>;
            var pageCommandId = cp.GetObject("pageCommandId", null) as string;

            for (int i = 0; i < addins.Count; i++)
            {
                var cmd = addins[i].getAfterCommand(pageCommandId);
                if (cmd != null && cmd.canExcute(cp))
                {
                    cmd.excute(cp);
                }
            }
        }
    }
}
