using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using wod.lwcms.models;

namespace wod.lwcms.dataaccess
{
    public class SiteDataAccess : ISiteDataAccess
    {
        public models.wodsite GetSite(string _siteKey)
        {
            XmlDocument siteDoc = GetSiteDoc();
            XmlNode siteNode = GetSiteNode(siteDoc, _siteKey);
            if (siteNode == null)
            {
                return null;
            }
            else
            {
                return ReadSiteFromNode(siteNode);
            }
        }

        private string GetSiteXml()
        {
            string siteXml = wodEnvironment.GetDataPath("site.xml");
            return siteXml;
        }

        private XmlDocument GetSiteDoc()
        {
            XmlDocument siteDoc = new XmlDocument();
            using (XmlReader xr = XmlReader.Create(GetSiteXml()))
            {
                siteDoc.Load(xr);
            }
            return siteDoc;
        }

        private XmlNode GetSiteNode(XmlDocument siteDoc, string _siteKey)
        {
            var sites = siteDoc.SelectNodes("/sites/site");
            foreach (XmlNode siteNode in sites)
            {
                var key = siteNode.Attributes["key"];
                if (key != null && key.Value == _siteKey)
                {
                    return siteNode;
                }
            }
            return null;
        }

        private models.wodsite ReadSiteFromNode(XmlNode siteNode)
        {
            models.wodsite site = new models.wodsite();
            foreach (XmlNode node in siteNode.ChildNodes)
            {
                switch (node.Name.ToLower())
                {
                    case "sitename":
                        site.siteName = node.InnerText.Trim();
                        break;
                    case "siteurl":
                        site.siteUrl = node.InnerText.Trim();
                        break;
                    case "description":
                        site.description = node.InnerText.Trim();
                        break;
                    case "keywords":
                        site.keywords = node.InnerText.Trim();
                        break;
                    case "copyright":
                        site.copyright = node.InnerText.Trim();
                        break;
                    case "title":
                        site.title = node.InnerText.Trim();
                        break;
                    case "navis":
                        var navLst = ReadNavLst(node.ChildNodes);
                        site.navis.Add(node.Attributes["key"].Value, navLst);
                        break;
                    default:
                        break;
                }
            }
            return site;
        }

        private List<siteNavi> ReadNavLst(XmlNodeList xmlNodeList)
        {
            List<siteNavi> navs = new List<siteNavi>();
            foreach (XmlNode node in xmlNodeList)
            {
                siteNavi nav = new siteNavi();
                nav.name = GetAttr(node, "name");
                nav.target = GetAttr(node, "target");
                nav.title = GetAttr(node, "title");
                nav.naviUrl = node.FirstChild.InnerText.Trim();

                var sub = ReadNavLst(node.SelectNodes("subNavis/navi"));
                nav.subNavis.AddRange(sub);
                navs.Add(nav);
            }
            return navs;
        }

        private string GetAttr(XmlNode node, string attrName)
        {
            var xmlAttr = node.Attributes[attrName];
            return xmlAttr == null ? "" : xmlAttr.Value;
        }

        public void SaveSite(string _siteKey, wodsite site)
        {
            XmlDocument siteDoc = GetSiteDoc();
            XmlNode siteNode = GetSiteNode(siteDoc, _siteKey);
            if (siteNode == null)
            {
                if (siteDoc.SelectSingleNode("/sites") == null)
                {
                    siteDoc.AppendChild(siteDoc.CreateElement("sites"));
                }
                siteNode = siteDoc.CreateElement("site");
                (siteNode as XmlElement).SetAttribute("key", _siteKey);
                siteDoc.SelectSingleNode("/sites").AppendChild(siteNode);
            }
            else
            {
            }
            WriteSiteNode(siteNode, site, siteDoc);
            siteDoc.Save(GetSiteXml());
        }

        private void WriteSiteNode(XmlNode siteNode, wodsite site, XmlDocument siteDoc)
        {
            siteNode.InnerXml = "<sitename></sitename><siteurl></siteurl><description></description><keywords></keywords><copyright></copyright><title></title>";
            foreach (XmlNode node in siteNode.ChildNodes)
            {
                switch (node.Name.ToLower())
                {
                    case "sitename":
                        node.InnerText = site.siteName;
                        break;
                    case "siteurl":
                         node.InnerText = site.siteUrl;
                        break;
                    case "description":
                        node.InnerText = site.description;
                        break;
                    case "keywords":
                        node.InnerText = site.keywords;
                        break;
                    case "copyright":
                        node.InnerText = site.copyright;
                        break;
                    case "title":
                        node.InnerText = site.title;
                        break;
                    default:
                        break;
                }
            }
            foreach (var key in site.navis.Keys)
            {
                var navisNode = siteDoc.CreateElement("navis");
                navisNode.SetAttribute("key", key);
                WriteNaviNode(site.navis[key], navisNode, siteDoc);
                siteNode.AppendChild(navisNode);
            }
        }

        private void WriteNaviNode(List<siteNavi> navis, XmlNode navisNode, XmlDocument siteDoc)
        {
            foreach (var navi in navis)
	        {
                var node = siteDoc.CreateElement("navi");
                node.SetAttribute("name", navi.name);
                node.SetAttribute("target", navi.target);
                node.SetAttribute("title", navi.title);
                node.InnerText = navi.naviUrl;
                if (navi.subNavis.Count > 0)
                {
                    var subNavisNode = siteDoc.CreateElement("subNavis");
                    WriteNaviNode(navi.subNavis, subNavisNode, siteDoc);
                    node.AppendChild(subNavisNode);
                }
                navisNode.AppendChild(node);
            }
        }

        private string GetSiteAttributeXml()
        {
            string siteXml = wodEnvironment.GetDataPath("attribute.xml");
            return siteXml;
        }
        private XmlDocument GetSiteAttributeXmlDoc()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(GetSiteAttributeXml());
            return doc;
        }

        public siteAttribute GetCommonAttribute()
        {
            XmlDocument doc = GetSiteAttributeXmlDoc();
            return ReadSiteAttribute(doc.SelectSingleNode("/attributes"));
        }

        private siteAttribute ReadSiteAttribute(XmlNode doc)
        {
            siteAttribute attr = new siteAttribute();
            attr.ioc = ReadIoc(doc.SelectNodes("ioc/item"));
            attr.extensions = ReadExtensions(doc.SelectNodes("extension/item"));
            attr.plugins = ReadExtensions(doc.SelectNodes("addin/item"));
            attr.svrTemplates = ReadSvrTemplates(doc.SelectNodes("svrTemplates/item"));
            attr.themes = ReadThemes(doc.SelectNodes("themes/item"));
            attr.svrTemplate = ReadSvrTemplate(doc.SelectSingleNode("svrTemplate"));
            attr.theme = ReadTheme(doc.SelectSingleNode("theme"));
            return attr;
        }

        private svrTemplate ReadSvrTemplate(XmlNode xmlNode)
        {
            return xmlNode == null ? null : new svrTemplate() { name = xmlNode.InnerText };
        }

        private theme ReadTheme(XmlNode xmlNode)
        {
            return xmlNode == null ? null : new theme() { name = xmlNode.InnerText };
        }

        private List<theme> ReadThemes(XmlNodeList xmlNodeList)
        {
            List<theme> themes = new List<theme>();

            foreach (XmlNode node in xmlNodeList)
            {
                themes.Add(new theme()
                {
                    name = GetAttr(node, "name"),
                    description = GetAttr(node, "description"),
                    preImage = node.InnerText
                });
            }
            return themes;
        }

        private List<svrTemplate> ReadSvrTemplates(XmlNodeList xmlNodeList)
        {
            List<svrTemplate> temps = new List<svrTemplate>();

            foreach (XmlNode node in xmlNodeList)
            {
                temps.Add(new svrTemplate()
                {
                    name = GetAttr(node, "name"),
                    description = GetAttr(node, "description"),
                });
            }
            return temps;
        }

        private List<models.addin> ReadExtensions(XmlNodeList xmlNodeList)
        {
            List<models.addin> exts = new List<models.addin>();
            foreach (XmlNode node in xmlNodeList)
            {
                var typeNode = node.SelectSingleNode("type");
                List<models.addinSetting> settings = new List<addinSetting>();
                foreach (XmlNode setNode in node.SelectNodes("setting"))
                {
                    settings.Add(new addinSetting()
                    {
                        name = GetAttr(node, "name"),
                        description = GetAttr(node, "description"),
                        value = node.InnerText,
                    });
                }
                exts.Add(new models.addin()
                {
                    name = GetAttr(node, "name"),
                    description = GetAttr(node, "description"),
                    type =  typeNode == null ? "" : typeNode.InnerText,
                    addinSettings = settings
                });
            }
            return exts;
        }

        private List<models.ioc> ReadIoc(XmlNodeList xmlNodeList)
        {
            List<models.ioc> ioc = new List<models.ioc>();
            foreach (XmlNode node in xmlNodeList)
            {
                var datatype = GetAttr(node, "datatype");
                var targetNode =  node.SelectSingleNode("target");
                var realizeNode =  node.SelectSingleNode("realize");
                ioc.Add(new models.ioc()
                {
                    name = GetAttr(node, "name"),
                    type = (models.ioc.ioctype)Enum.Parse(typeof(models.ioc.ioctype), GetAttr(node, "type")),
                    datatype = string.IsNullOrEmpty(datatype) ? models.ioc.iocdatatype.UnKown: (models.ioc.iocdatatype)Enum.Parse(typeof(models.ioc.iocdatatype), datatype),
                    target = targetNode == null ? "" : targetNode.InnerText,
                    realize = realizeNode == null ? "" : realizeNode.InnerText,
                    value = node.InnerText
                });
            }
            return ioc;
        }

        public siteAttribute GetSiteAttribute(string _siteKey)
        {
            XmlDocument doc = GetSiteAttributeXmlDoc();
            return ReadSiteAttribute(doc.SelectSingleNode("/attributes/siteAttribute/attribute[@siteKey=\"" + _siteKey + "\"]"));
        }

        public void SaveSiteAttribute(string _siteKey, siteAttribute attrs)
        {
            throw new NotImplementedException();
        }
    }
}
