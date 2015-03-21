using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace wod.lwcms
{
    public class aliasResource
    {
        private commands.commandPool pool;
        public aliasResource(commands.commandPool pool)
        {
            this.pool = pool;
        }

        private static XmlDocument GetDoc(string path)
        {
            XmlDocument doc = new XmlDocument();
            using (XmlReader xr = XmlReader.Create(path))
            {
                doc.Load(xr);
            }
            return doc;
        }

        internal void init(string path)
        {
            SortedList<string, string> sortFile = new SortedList<string, string>();
            foreach (string item in System.IO.Directory.GetFiles(path, "*.xml"))
            {
                sortFile.Add(item, item);
            }
            foreach (var filepath in sortFile.Keys)
            {
                addAlias(filepath);
            }
        }

        internal void addAlias(string aliasFile)
        {
            var doc = GetDoc(aliasFile);
            aliasTypes.AddRange(ParseNodes<Type>(doc.SelectNodes("/aliasResource/types/type"), new TypeConvert()));
            aliasCommands.AddRange(ParseNodes<commandCreator>(doc.SelectSingleNode("/aliasResource/commands").ChildNodes, new CommandConvert(this)));
        }

        internal void distinctAlias()
        {
            distinctAlias(aliasTypes);
            distinctAlias(aliasCommands);
        }

        private void distinctAlias<T1>(List<alias<T1>> aliass)
        {
            aliass.Sort((at1, at2) => {
                if (at1 == at2) return 0;
                int r = at1.AliasName.CompareTo(at2.AliasName); 
                return r == 0 ? -1 : r; 
            });
            for (int i = 1; i < aliass.Count; i++)
            {
                if (aliass[i - 1].AliasName == aliass[i].AliasName)
                {
                    aliass.RemoveAt(i);
                    i--;
                }
            }
        }

        private List<alias<Type>> aliasTypes = new List<alias<Type>>();
        private List<alias<commandCreator>> aliasCommands = new List<alias<commandCreator>>();

        public List<alias<T>> ParseNodes<T>(XmlNodeList nodes, IConvert<T> convert)
        {
            List<alias<T>> lst = new List<alias<T>>();
            foreach (XmlNode node in nodes)
            {
                if (node.NodeType == XmlNodeType.Comment) continue;
                lst.Add(new alias<T>
                {
                    AliasName = node.Attributes["alias"].Value,
                    Instance = convert.Parse(node)
                });
            }
            return lst;
        }

        internal commands.command GetCommandByAlias(string aliasName)
        {
            commandCreator creator = GetObjectByAlias(aliasName, aliasCommands);
            return creator == null ? null : creator.Parse();
        }

        internal Type GetTypeByAlias(string aliasName)
        {
            return GetObjectByAlias(aliasName, aliasTypes);
        }

        private T GetObjectByAlias<T>(string aliasName, List<alias<T>> pool)
        {
            foreach (var item in pool)
            {
                if (item.AliasName == aliasName)
                {
                    return item.Instance;
                }
            }
            return default(T);
        }


        private class commandCreator
        {
            private XmlNode node;
            private aliasResource resource;
            public commandCreator(XmlNode node, aliasResource resource)
            {
                this.node = node;
                this.resource = resource;
            }

            public commands.command Parse()
            {
                return commands.commandPool.ParseCommand(resource, node);
            }
        }

        private class CommandConvert : IConvert<commandCreator>
        {
            private aliasResource resource;
            public CommandConvert(aliasResource resource)
            {
                this.resource = resource;
            }  
            #region IConvert<Type> 成员

            public commandCreator Parse(XmlNode node)
            {
                return new commandCreator(node, resource);
            }

            #endregion
        }
    }

    public class alias<T>
    {
        public string AliasName { get; set; }

        public T Instance { get; set; }
    }

    public interface IConvert<T>
    {
        T Parse(XmlNode node);
    }

    public class TypeConvert : IConvert<Type>
    {
        #region IConvert<Type> 成员

        public Type Parse(XmlNode node)
        {
            return Type.GetType(node.InnerText);
        }

        #endregion
    }
}
