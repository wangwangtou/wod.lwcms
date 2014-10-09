using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace wod.lwcms.commands
{
    class commandPool
    {
        public static Dictionary<string,command> commands = new Dictionary<string, command>();

        internal static void init(string path)
        {
            string[] commandFiles = System.IO.Directory.GetFiles(path, "*.xml");
            foreach (string file in commandFiles)
            {
                XmlDocument doc = new XmlDocument();
                using (XmlReader xr = XmlReader.Create(file))
                {
                    doc.Load(xr);
                }
                Dictionary<string,command> cmds = LoadCommand(doc.DocumentElement.ChildNodes);
                foreach (var item in cmds.Keys)
                {
                    commands.Add(item, cmds[item]);
                }
            }
        }

        private static Dictionary<string, command> LoadCommand(XmlNodeList nodes)
        {
            Dictionary<string, command> cmds = new Dictionary<string, command>();
            foreach (XmlNode node in nodes)
            {
                command cmd = ParseCommand(node);
                cmds.Add(cmd.id, cmd);
            }
            return cmds;
        }

        private static command ParseCommand(XmlNode node)
        {
            command cmd;
            switch (node.Name)
            {
                case "assCommand":
                    cmd = new assCommand()
                    {
                        methodName = node.SelectSingleNode("methodName").InnerText,
                        typeName = node.SelectSingleNode("typeName").InnerText,
                    };
                    break;
                case "sqlCommand":
                    cmd = new sqlCommand()
                    {
                        sql = node.SelectSingleNode("sql").InnerText,
                        paramenterPrefix = node.Attributes["paramenterPrefix"].Value,
                        excuteType = node.Attributes["excuteType"].Value,
                        useTransaction = node.Attributes["useTransaction"]!=null && node.Attributes["useTransaction"].Value == "true",
                        isCommit = node.Attributes["isCommit"] != null && node.Attributes["isCommit"].Value == "true",
                    };
                    break;
                case "multiCommand":
                    var cmds = new List<command>();
                    cmds.AddRange(LoadCommand(node.SelectSingleNode("commands").ChildNodes).Values);
                    cmd = new multiCommand()
                    {
                        commands = cmds
                    };
                    break;
                default:
                    try
                    {
                        var type = wodEnvironment.GetTypeByName("wod.lwcms.commands." + node.Name);
                        cmd = common.GetInstance(type) as command;
                        cmd.parseProperty(node);
                    }
                    catch (Exception)
                    {
                        cmd = new emptyCommand();
                    }
                    break;
            } 
            XmlNode filterNode = node.SelectSingleNode("actfilter");
            if (filterNode != null)
                cmd.filterExpress = BoolExpress.CreateBoolExpress( filterNode.InnerText);
            cmd.id = node.Attributes["id"].Value;
            return cmd;
        }

        internal static command getCommand(string id)
        {
            if (commands.ContainsKey(id))
            {
                return commands[id];
            }
            return new emptyCommand();
        }

    }
}
