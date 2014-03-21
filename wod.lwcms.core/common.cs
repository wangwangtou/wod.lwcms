using System;
using System.Collections.Generic;
using System.Text;
using LitJson;

namespace wod.lwcms
{
    public static class common
    {
        public static string ToJson<T>(T obj)
        {
            return JsonMapper.ToJson(obj);
        }
    }
}
