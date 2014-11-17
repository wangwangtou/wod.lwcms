using System;
using System.Collections.Generic;
using System.Text;

namespace wod.lwcms.services
{
    public class CategoryService:ICategoryService
    {
        static CategoryService()
	    {
            undefineCategory = new models.category();
            undefineCategory.code = "wodcateg0ry";
            undefineCategory.name = "未分类";
            undefineCategory.page = "category";
            undefineCategory.content = "未分类";
            undefineCategory.contentpage = "article";
            undefineCategory.level = 1;
        }

        private dataaccess.ICategoryDataAccess dataaccess;

        private static readonly models.category undefineCategory;

        public CategoryService(dataaccess.ICategoryDataAccess _dataaccess)
        {
            this.dataaccess = _dataaccess;
        }

        public List<models.category> getAllCategories()
        {
            return dataaccess.GetAllCategories();
        }

        public models.category getCurCategory(List<models.category> allCats, string categoryPath)
        {
            foreach (var item in allCats)
            {
                if (item.fullpath == categoryPath)
                    return item;
                if (item.subCategory.Count > 0)
                {
                    var child = getCurCategory(item.subCategory, categoryPath);
                    if (child != undefineCategory)
                        return child;
                }
            }
            return undefineCategory;
        }

        public void updateCategories(List<models.category> allCats)
        {
            updateCategoriesPath(allCats, "");
            dataaccess.SaveCategories(allCats);
        }

        private void updateCategoriesPath(List<models.category> cats, string path)
        {
            foreach (var cat in cats)
            {
                cat.fullpath = path + "/" + cat.code;
                updateCategoriesPath(cat.subCategory, cat.fullpath);
            }
        }
    }
}
