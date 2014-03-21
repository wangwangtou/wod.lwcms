using System;
using System.Collections.Generic;
using System.Text;

namespace wod.lwcms.models
{
    public interface ISeoObject
    {
        string description { get; set; }
        string keywords { get; set; }
        string seotitle { get; set; }
    }
}
