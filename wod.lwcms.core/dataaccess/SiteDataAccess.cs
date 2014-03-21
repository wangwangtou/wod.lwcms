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
            string siteXml = wodEnvironment.GetDataPath("site.xml");
            using (XmlReader xr = XmlReader.Create(siteXml))
            {
                XmlDocument siteDoc = new XmlDocument();
                siteDoc.Load(xr);
                var sites = siteDoc.SelectNodes("/sites/site");
                foreach (XmlNode siteNode in sites)
                {
                    var key = siteNode.Attributes["key"];
                    if (key != null && key.Value == _siteKey)
                    {
                        return ReadSiteFromNode(siteNode);
                    }
                    
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
    }
}
