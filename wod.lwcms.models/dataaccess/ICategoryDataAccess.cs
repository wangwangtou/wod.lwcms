using System;
using System.Collections.Generic;
using System.Text;
using wod.lwcms.models;

namespace wod.lwcms.dataaccess
{
    public interface ICategoryDataAccess
    {
        List<category> GetAllCategories();

        void SaveCategories(List<category> allCats);
    }
}
