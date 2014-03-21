using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace wod.lwcms.dataaccess
{
    public class CategoryDataAccess : ICategoryDataAccess
    {
        public List<models.category> GetAllCategories()
        {
            string cateXml = wodEnvironment.GetDataPath("category.xml");
            XmlDocument cateDoc = new XmlDocument();
            using (XmlReader xr = XmlReader.Create(cateXml))
            {
                cateDoc.Load(xr);
            }
            var categories = cateDoc.SelectNodes("/categories/category");
            List<models.category> categoriesLst = ReadCategories(categories,1,null);
            return categoriesLst;
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
                    case "subcategories": cate.subCategory.AddRange(ReadCategories(valNode.ChildNodes, level + 1, cate)); break;
                    default:
                        break;
                }
            }
            return cate;
        }
    }
}
