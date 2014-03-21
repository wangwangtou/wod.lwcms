using System;
using System.Collections.Generic;
using System.Text;

namespace wod.lwcms.services
{
    public class CategoryService:ICategoryService
    {
        private dataaccess.ICategoryDataAccess dataaccess;

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
                    if (child != null)
                        return child;
                }
            }
            return null;
        }
    }
}
