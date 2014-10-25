using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace wod.lwcms.dataaccess
{
    public class CategoryDataAccess : ICategoryDataAccess
    {
        private string siteKey;
        public CategoryDataAccess(string siteKey)
        {
            this.siteKey = siteKey;
        }

        public List<models.category> GetAllCategories()
        {
            XmlDocument cateDoc = GetCategoriesDoc();
            var categories = cateDoc.SelectNodes("/categories/category");
            List<models.category> categoriesLst = ReadCategories(categories,1,null);
            return categoriesLst;
        }

        private string GetCategoriesDocPath()
        {
            string cateXml = wodEnvironment.GetDataPath(siteKey,"category.xml");
            return cateXml;
        }

        private XmlDocument GetCategoriesDoc()
        {
            string cateXml = GetCategoriesDocPath();
            XmlDocument cateDoc = new XmlDocument();
            using (XmlReader xr = XmlReader.Create(cateXml))
            {
                cateDoc.Load(xr);
            }
            return cateDoc;
        }

        private List<models.category> ReadCategories(XmlNodeList categories,int level,models.category parent)
        {
            List<models.category> categoriesLst = new List<models.category>();
            foreach (XmlNode cateNode in categories)
            {
                categoriesLst.Add(ReadCategory(cateNode, level, parent));
            }
            return categoriesLst;
        }

        private models.category ReadCategory(XmlNode cateNode,int level,models.category parent)
        {
            models.category cate = new models.category();
            cate.level = level;
            cate.parent = parent;
            foreach (XmlNode valNode in cateNode.ChildNodes)
            {
                switch (valNode.Name.ToLower())
                {
                    case "name": cate.name = valNode.InnerText; break;
                    case "code": cate.code = valNode.InnerText; break;
                    case "fullpath": cate.fullpath = valNode.InnerText; break;
                    case "description": cate.description = valNode.InnerText; break;
                    case "keywords": cate.keywords = valNode.InnerText; break;
                    case "page": cate.page = valNode.InnerText; break;
                    case "contentpage": cate.contentpage = valNode.InnerText; break;
                    case "extendform": cate.extendform = valNode.InnerText; break;
                    case "subcategories": cate.subCategory.AddRange(ReadCategories(valNode.ChildNodes, level + 1, cate)); break;
                    default:
                        break;
                }
            }
            return cate;
        }

        public void SaveCategories(List<models.category> allCats)
        {
            XmlDocument cateDoc = GetCategoriesDoc();
            var categories = cateDoc.SelectSingleNode("/categories");
            categories.RemoveAll();
            WriteCategories(allCats, categories, cateDoc);
            cateDoc.Save(GetCategoriesDocPath());
        }

        private void WriteCategories(List<models.category> categories, XmlNode parent, XmlDocument cateDoc)
        {
            foreach (models.category cate in categories)
            {
                parent.AppendChild(WriteCategory(cate,cateDoc));
            }
        }

        private XmlNode WriteCategory(models.category cate, XmlDocument cateDoc)
        {
            var node = cateDoc.CreateElement("category");
            node.InnerXml = "<name></name><code></code><fullpath></fullpath><description></description><keywords></keywords><page></page><contentpage></contentpage><extendform></extendform><subcategories></subcategories>";

            foreach (XmlNode valNode in node.ChildNodes)
            {
                switch (valNode.Name.ToLower())
                {
                    case "name": valNode.InnerText = cate.name ; break;
                    case "code": valNode.InnerText = cate.code; break;
                    case "fullpath": valNode.InnerText = cate.fullpath; break;
                    case "description": valNode.InnerText = cate.description; break;
                    case "keywords": valNode.InnerText = cate.keywords; break;
                    case "page": valNode.InnerText = cate.page; break;
                    case "contentpage": valNode.InnerText =  cate.contentpage; break;
                    case "extendform": valNode.InnerText = cate.extendform; break;
                    case "subcategories": WriteCategories(cate.subCategory,valNode,cateDoc); break;
                    default:
                        break;
                }
            }
            return node;
        }
    }
}
