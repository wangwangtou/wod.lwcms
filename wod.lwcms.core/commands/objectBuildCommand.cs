using System;
using System.Collections.Generic;
using System.Text;

namespace wod.lwcms.commands
{
    public class objectBuildCommand : command
    {
        protected override void excuteNoCheck(commandsParameter cp)
        {
            var instance = cp.GetObject(id, targetType);

            if (instance != null && instance.GetType() == typeof(string))
            {
                instance = common.FromJson(targetType, instance.ToString());
            }

            Dictionary<string, System.Reflection.PropertyInfo> attributes = TypeHelper.CanWriteableProperty(targetType, null);
            foreach (var item in attributes.Keys)
            {
                object data = cp.GetObject(item, attributes[item].PropertyType);
                if (data != null)
                {
                    if (data.GetType() == attributes[item].PropertyType)
                    {
                        attributes[item].SetValue(instance, data, null);
                    }
                    else if (data.GetType() == typeof(string))
                    {
                        data = common.FromJson(attributes[item].PropertyType, data.ToString());
                        attributes[item].SetValue(instance, data, null);
                    }
                }
            }
            cp.AddObject(id, instance);
        }

        public Type targetType { get; set; }

        public override void parseProperty(System.Xml.XmlNode node)
        {
            base.parseProperty(node);
            string typeName = node.SelectSingleNode("targetType").InnerText;
            targetType = Type.GetType(typeName);
        }    
    }
}
