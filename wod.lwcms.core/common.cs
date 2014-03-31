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
            JsonWriter jw = new JsonWriter();
            JsonMapper.ToJson(obj, jw);
            return jw.ToString();
        }

        public static T FromJson<T>(string json)
        {
            if (string.IsNullOrEmpty(json))
                return default(T);
            return JsonMapper.ToObject<T>(json);
        }

        public static string GetRemovedParamUrl(string url, string paramName)
        {
            if (url.IndexOf("?") < 0)
                return url;
            int pindex = url.ToLower().IndexOf("?" + paramName.ToLower());
            bool isfirst = true;
            if (pindex < 0)
            {
                pindex = url.ToLower().IndexOf("&" + paramName.ToLower());
                isfirst = false;
            }
            if (pindex < 0)
            {
                return url;
            }
            else
            {
                int next = url.IndexOf("&", pindex + 1);
                if (next < 0)
                    return url.Substring(0, pindex).TrimEnd('?');
                else
                {
                    if (isfirst)
                    {
                        next++;
                        pindex++;
                    }
                    return url.Substring(0, pindex) + url.Substring(next, url.Length - next);
                }
            }
        }
    }
}
