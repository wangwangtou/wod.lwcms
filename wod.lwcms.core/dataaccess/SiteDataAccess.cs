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
    }
}
