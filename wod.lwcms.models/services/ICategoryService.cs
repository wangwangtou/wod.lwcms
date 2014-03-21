using System;
using System.Collections.Generic;
using System.Text;

namespace wod.lwcms.services
{
    public interface ICategoryService
    {
        List<models.category> getAllCategories();

        models.category getCurCategory(List<models.category> allCats, string categoryPath);
    }
}
