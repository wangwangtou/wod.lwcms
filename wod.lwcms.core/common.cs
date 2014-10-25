using System;
using System.Collections.Generic;
using System.Text;
using LitJson;

namespace wod.lwcms
{
    public static class common
    {
        static common()
        {
            LitJson.JsonMapper.RegisterExporter<models.category>(
                (c, w) =>
                {
                    w.WriteObjectStart();
                    w.WritePropertyName("id");
                    w.Write(c.id);
                    w.WritePropertyName("image");
                    JsonMapper.ToJson(c.image, w);
                    w.WritePropertyName("keywords");
                    w.Write(c.keywords);
                    w.WritePropertyName("name");
                    w.Write(c.name);
                    w.WritePropertyName("page");
                    w.Write(c.page);
                    w.WritePropertyName("code");
                    w.Write(c.code);
                    w.WritePropertyName("content");
                    w.Write(c.content);
                    w.WritePropertyName("contentpage");
                    w.Write(c.contentpage);
                    w.WritePropertyName("description");
                    w.Write(c.description);
                    w.WritePropertyName("extendform");
                    w.Write(c.extendform);
                    w.WritePropertyName("fullpath");
                    w.Write(c.fullpath);
                    
                    w.WritePropertyName("subCategory");
                    JsonMapper.ToJson(c.subCategory, w);
                    w.WriteObjectEnd();
                });

            LitJson.JsonMapper.RegisterExporter<models.article>(
                (a, w) =>
                {
                    w.WriteObjectStart();
                    w.WritePropertyName("id");
                    w.Write(a.id);
                    w.WritePropertyName("image");
                    JsonMapper.ToJson(a.image, w);
                    w.WritePropertyName("keywords");
                    w.Write(a.keywords);
                    w.WritePropertyName("name");
                    w.Write(a.name);
                    w.WritePropertyName("page");
                    w.Write(a.page);
                    w.WritePropertyName("code");
                    w.Write(a.code);
                    w.WritePropertyName("content");
                    w.Write(a.content);
                    w.WritePropertyName("preContent");
                    w.Write(a.preContent);
                    w.WritePropertyName("description");
                    w.Write(a.description);
                    w.WritePropertyName("category");
                    w.Write(a.category == null ? "" : a.category.fullpath);

                    w.WriteObjectEnd();
                });
        }    

        public static object GetInstance(Type type)
        {
            return type.GetConstructor(new Type[0]).Invoke(new object[0]);
        }

        public static string ToJson<T>(T obj)
        {
            JsonWriter jw = new JsonWriter();
            JsonMapper.ToJson(obj, jw);
            return jw.ToString();
        }

        internal static object FromJson(Type targetType, string json)
        {
            return typeof(common).GetMethod("FromJson", new Type[] { typeof(string) }).MakeGenericMethod(targetType).Invoke(null, new object[] { json });
        }

        public static T FromJson<T>(string json)
        {
            if (string.IsNullOrEmpty(json))
                return default(T);
            try
            {
                return JsonMapper.ToObject<T>(json);
            }
            catch (Exception)
            {
                return default(T);
            }
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

        internal static object ChangeType(Type valueType, string value)
        {
            switch (valueType.Name)
            {
                case "System.String":
                    return value;
                case "System.Int32":
                case "System.Int64":
                case "System.Decimal":
                case "System.Double":
                    double d;
                    double.TryParse(value, out d);
                    return Convert.ChangeType(d, valueType);
                default:
                    try
                    {
                        return Convert.ChangeType(value, valueType);
                    }
                    catch (Exception)
                    {
                        return common.GetInstance(valueType);
                    }
                    break;
            }
        }
    }

    public sealed class TypeHelper
    {
        public static Dictionary<string, System.Reflection.PropertyInfo> CanStringTypeWriteableProperty(Type type,Type subClass)
        {
            if (subClass == null) subClass = typeof(object);
            System.Reflection.PropertyInfo[] infos = type.GetProperties();
            Dictionary<string, System.Reflection.PropertyInfo> attributes = new Dictionary<string, System.Reflection.PropertyInfo>();
            foreach (var item in infos)
            {
                if (item.CanWrite
                    && item.PropertyType == typeof(string)
                    && item.GetIndexParameters().Length == 0
                    && item.DeclaringType.IsSubclassOf(subClass))
                    attributes.Add(item.Name, item);
            }
            return attributes;
        }

        public static Dictionary<string, System.Reflection.PropertyInfo> CanWriteableProperty(Type type, Type subClass)
        {
            if (subClass == null) subClass = typeof(object);
            System.Reflection.PropertyInfo[] infos = type.GetProperties();
            Dictionary<string, System.Reflection.PropertyInfo> attributes = new Dictionary<string, System.Reflection.PropertyInfo>();
            foreach (var item in infos)
            {
                if (item.CanWrite
                    && item.GetIndexParameters().Length == 0
                    && item.DeclaringType.IsSubclassOf(subClass))
                    attributes.Add(item.Name, item);
            }
            return attributes;
        }
    }
}
