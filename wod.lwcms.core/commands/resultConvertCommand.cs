using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Collections;

namespace wod.lwcms.commands
{
    public class resultConvertCommand : command
    {
        protected override void excuteNoCheck(commandsParameter cp)
        {
            Hashtable jd = new Hashtable();
            foreach (string item in names.Keys)
            {
                jd[names[item]] = cp.GetObject(item, typeof(object));
            }
            cp.AddObject(id, jd);
        }

        private Dictionary<string, string> names;

        public override void parseProperty(System.Xml.XmlNode node)
        {
            names = new Dictionary<string, string>();
            foreach (XmlNode item in node.SelectNodes("cdata"))
            {
                string valuename = item.Attributes["valueName"].Value;
                string dataname = item.Attributes["dataName"] != null ? item.Attributes["dataName"].Value : valuename;
                names.Add(valuename, dataname);
            }
            base.parseProperty(node);
        }
    }
}
