using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace wod.lwcms.commands
{
    public class commandPool
    {
        private Dictionary<string,command> commands = new Dictionary<string, command>();

        internal void init(aliasResource resource, string path)
        {
            string[] commandFiles = System.IO.Directory.GetFiles(path, "*.xml");
            foreach (string file in commandFiles)
            {
                var cmds = LoadCommand(resource,file);
                foreach (var item in cmds.Keys)
                {
                    commands.Add(item, cmds[item]);
                }
            }
        }

        internal void addAddinCommand(Dictionary<string, command> cmds)
        {
            foreach (var item in cmds.Keys)
            {
                commands.Add(item, cmds[item]);
            }
        }

        public static Dictionary<string, command> LoadCommand(aliasResource resource, string filepath)
        {
            XmlDocument doc = new XmlDocument();
            using (XmlReader xr = XmlReader.Create(filepath))
            {
                doc.Load(xr);
            }
            Dictionary<string, command> cmds = LoadCommand(resource,doc.DocumentElement.ChildNodes);
            return cmds;
        }

        private static Dictionary<string, command> LoadCommand(aliasResource resource, XmlNodeList nodes)
        {
            Dictionary<string, command> cmds = new Dictionary<string, command>();
            foreach (XmlNode node in nodes)
            {
                if (node.NodeType == XmlNodeType.Comment) continue;
                command cmd = ParseCommand(resource,node);
                cmds.Add(cmd.id, cmd);
            }
            return cmds;
        }

        public static command ParseCommand(aliasResource resource, XmlNode node)
        {
            command cmd;
            if (node.Attributes["type"] != null && node.Attributes["type"].Value == "alias")
            {
                cmd = resource.GetCommandByAlias(node.Name);
                if (cmd == null)
                {
                    cmd = new emptyCommand();
                }
            }
            else
            {
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
                            useTransaction = node.Attributes["useTransaction"] != null && node.Attributes["useTransaction"].Value == "true",
                            isCommit = node.Attributes["isCommit"] != null && node.Attributes["isCommit"].Value == "true",
                        };
                        break;
                    case "multiCommand":
                        var cmds = new List<command>();
                        cmds.AddRange(LoadCommand(resource,node.SelectSingleNode("commands").ChildNodes).Values);
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
            }
            XmlNode filterNode = node.SelectSingleNode("actfilter");
            if (filterNode != null)
                cmd.filterExpress = BoolExpress.CreateBoolExpress("@__breakall != true && (" + filterNode.InnerText + ")");
            else
                cmd.filterExpress = BoolExpress.CreateBoolExpress("@__breakall != true");
            cmd.id = node.Attributes["id"] == null ? "" : node.Attributes["id"].Value;
            return cmd;
        }

        internal command getCommand(string id)
        {
            if (commands.ContainsKey(id))
            {
                return commands[id];
            }
            return new emptyCommand();
        }
    }
}
