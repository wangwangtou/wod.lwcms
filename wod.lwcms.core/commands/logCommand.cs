using System;
using System.Collections.Generic;
using System.Text;

namespace wod.lwcms.commands
{
    public class logCommand : command
    {
        protected override void excuteNoCheck(commandsParameter cp)
        {
            StringBuilder sb = new StringBuilder();
            var dic = cp.OP.Pool;
            foreach (string item in dic.Keys)
            {
                sb.AppendLine(item + ":" + dic[item]);
            }
            using (System.IO.StreamWriter sw = System.IO.File.CreateText(wodEnvironment.GetDataPath(DateTime.Now.Ticks + ".txt")))
            {
                sw.Write(sb.ToString());
            }
        }
    }
}
