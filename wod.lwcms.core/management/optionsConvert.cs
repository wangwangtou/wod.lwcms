using System;
using System.Collections.Generic;
using System.Text;

namespace wod.lwcms.management
{

    public class optionsConvert
    {
        public class option
        {
            public option(string _name, string _value)
            {
                this.name = _name;
                this.value = _value;
            }
            public string name { get; set; }
            public string value { get; set; }
        }

        public List<option> convertCategory(List<models.category> cates)
        {
            List<option> option = new List<option>();
            foreach (var item in cates)
            {
                option.Add(new option(item.name, item.fullpath));
            }
            return option;
        }
    }
}
