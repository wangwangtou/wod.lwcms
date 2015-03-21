#region Version Info
/* ========================================================================
* 【本类功能概述】
* 
* 作者：王星 时间：2015-02-05 15:25:19
* 文件名：applicationParse
* 版本：V1.0.1
*
* 修改者： 时间： 
* 修改说明：
* ========================================================================
*/
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace wod.lwcms.config
{
    enum iocInternalItemType
    {
        instance, dynamicInstance, abstractTypes
    }

    enum iocItemType
    {
        instance, abstractTypes
    }

    abstract class iocInstanceItem
    {
        internal abstract string name { get; }
        internal abstract object value { get; }
    }

    abstract class iocAbstractTypesItem
    {
        internal abstract Type abstractType { get; }
        internal abstract Type realizeType { get; }
    }

    interface IIocXmlItem
    {
        void ParseXml(XmlNode node);
    }

    class commonIocInstanceItem : iocInstanceItem, IIocXmlItem
    {
        private object _value;
        private string _name;

        #region IIocXmlItem 成员

        public void ParseXml(XmlNode node)
        {
            string name = node.SelectSingleNode("name").InnerText;
            string value = node.SelectSingleNode("value").InnerText;
            var dataTypeAttr = node.Attributes["dataType"];
            this._name = name;
            if (dataTypeAttr != null)
            {
                this._value = common.ChangeType(Type.GetType(dataTypeAttr.Value), value);
            }
            else
            {
                this._value = value;
            }
        }

        #endregion

        internal override string name
        {
            get { return this._name; }
        }

        internal override object value
        {
            get { return this._value; }
        }
    }

    class dynamicInstanceParameter
    {
        public string name { get; set; }
        public bool rel { get; set; }
        public string value { get; set; }
    }

    class dynamicIocInstanceItem : iocInstanceItem, IIocXmlItem
    {
        public dynamicIocInstanceItem(ioc _ioc)
        {
            this._ioc = _ioc;
        }

        private object _value;
        private string _name;
        private ioc _ioc;

        #region IIocXmlItem 成员

        public void ParseXml(XmlNode node)
        {
            string name = node.SelectSingleNode("name").InnerText;
            this._name = name;
            string dynamicType = node.SelectSingleNode("dynamicType").InnerText;
            string dynamicMethod = node.SelectSingleNode("dynamicMethod").InnerText;
            List<dynamicInstanceParameter> parameters = parseParameter(node.SelectSingleNode("parameters"));
            Type dType = Type.GetType(dynamicType);
            var methods = dType.GetMethods();
            for (int i = 0; i < methods.Length; i++)
            {
                var method = methods[i];
                if (method.Name == dynamicMethod)
                {
                    var mps = method.GetParameters();
                    if (mps.Length == parameters.Count)
                    {
                        bool isMatch = true;
                        object[] objs = new object[parameters.Count];
                        for (int mi = 0; mi < mps.Length; mi++)
                        {
                            if (mps[mi].Name != parameters[mi].name)
                            {
                                isMatch = false;
                                break;
                            }
                            if (parameters[mi].rel)
                            {
                                objs[mi] = _ioc.GetInstance(parameters[mi].value);
                            }
                            else
                            {
                                objs[mi] = (common.ChangeType(mps[mi].ParameterType, parameters[mi].value));
                            }
                        }
                        if (isMatch)
                        {
                            if (method.IsStatic)
                            {
                                this._value = method.Invoke(null, objs);
                            }
                            else
                            {
                                this._value = method.Invoke(_ioc.GetService(dType), objs);
                            }
                            break;
                        }
                    }
                }
            }
        }

        private List<dynamicInstanceParameter> parseParameter(XmlNode xmlNode)
        {
            List<dynamicInstanceParameter> parameters = new List<dynamicInstanceParameter>();
            if (xmlNode != null)
            {
                foreach (XmlNode item in xmlNode.SelectNodes("parameter"))
                {
                    dynamicInstanceParameter p = new dynamicInstanceParameter();
                    p.value = item.InnerText;
                    p.name = item.Attributes["name"].Value;
                    p.rel = !(item.Attributes["rel"] == null || item.Attributes["rel"].Value != "true");
                    parameters.Add(p);
                }
            }
            return parameters;
        }

        #endregion

        internal override string name
        {
            get { return this._name; }
        }

        internal override object value
        {
            get { return this._value; }
        }
    }

    class commonIocAbstractTypesItem : iocAbstractTypesItem, IIocXmlItem
    {
        Type _abstractType;
        Type _realizeType;
        internal override Type abstractType
        {
            get { return this._abstractType; }
        }

        internal override Type realizeType
        {
            get { return this._realizeType; }
        }

        #region IIocXmlItem 成员

        public void ParseXml(XmlNode node)
        {
            string _type1 = node.SelectSingleNode("abstract").InnerText;
            string _type2 = node.SelectSingleNode("realize").InnerText;
            _abstractType = Type.GetType(_type1);
            _realizeType = Type.GetType(_type2);
        }

        #endregion
    }

    class applicationParse
    {
        internal static void ParseIocsNode(ioc _ioc, XmlNode iocs, Action<iocInstanceItem> instanceRegister, Action<iocAbstractTypesItem> abstractTypesRegister)
        {
            if (iocs != null)
            {
                foreach (XmlNode item in iocs.SelectNodes("item"))
                {
                    iocInternalItemType itemType = (iocInternalItemType)Enum.Parse(typeof(iocInternalItemType), item.Attributes["type"].Value);

                    IIocXmlItem iocitem = GetIocItem(_ioc, itemType, item);
                    if (iocitem is iocInstanceItem)
                    {
                        instanceRegister(iocitem as iocInstanceItem);
                    }
                    else if (iocitem is iocAbstractTypesItem)
                    {
                        abstractTypesRegister(iocitem as iocAbstractTypesItem);
                    }
                }
            }
        }

        private static IIocXmlItem GetIocItem(ioc _ioc, iocInternalItemType itemType, XmlNode item)
        {
            IIocXmlItem iocitem = null;
            switch (itemType)
            {
                case iocInternalItemType.instance:
                    iocitem = new commonIocInstanceItem();
                    break;
                case iocInternalItemType.dynamicInstance:
                    iocitem = new dynamicIocInstanceItem(_ioc);
                    break;
                case iocInternalItemType.abstractTypes:
                    iocitem = new commonIocAbstractTypesItem();
                    break;
                default:
                    break;
            }
            iocitem.ParseXml(item);
            return iocitem;
        }
    }
}
