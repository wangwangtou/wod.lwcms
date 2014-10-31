using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Reflection;

namespace wod.lwcms.commands
{
    public class applyEditCommand : command
    {
        public string ObjectName { get; set; }

        protected override void excuteNoCheck(commandsParameter cp)
        {
            object obj = cp.GetObject(ObjectName);

            object editObj = cp.GetObject(id+"_edit");
            object edit_hash = cp.GetObject(id+"_edit_hash");
            if (editObj != null && edit_hash!=null)
            {
                string edit_hashStr = edit_hash.ToString();
                if (edit_hashStr == common.GetHash(obj))
                {
                    List<edit> edit;
                    if (editObj.GetType() == typeof(string))
                    {
                        edit = common.FromJson<List<edit>>(editObj.ToString());
                    }
                    else
                    {
                        edit = editObj as List<edit>;
                    }
                    if (edit != null)
                    {
                        applyEdit(obj, edit);
                    }
                    cp.AddObject(id, true);
                    cp.AddObject(id + ".newhash", common.GetHash(obj));
                }
                else
                {
                    cp.AddObject(id, false);
                    cp.AddObject(id + ".errors", new List<validationError>() { new validationError(){ name = ObjectName , message = "对象已被修改"} });
                }
            }
        }


        private void applyEdit(object obj, List<edit> edit)
        {
            foreach (var item in edit)
            {
                //applyEdit(obj, item);
                item.Excute(obj);
            }
        }

        //private void applyEdit(object obj, edit edit)
        //{
        //    edit.Excute(obj);
        //}

        public class edit
        {
            public editType type { get; set; }
            public string dataexp { get; set; }
            //[index] object本身为数组
            //[index].name 数组第index个的name属性
            //[index].list[i] 数组第index个的list属性中的第i个
            //name name属性  本身为对象
            //items[key] items属性为dictionary，为key的对象
            public string value { get; set; }

            public void Excute(object obj)
            {
                var parts = dataexp.Split('.');
                object curObj = obj;
                for (int i = 0; i < parts.Length - 1; i++)
                {
                    partDefine pd = getPartType(parts[i]);
                    curObj = pd.GetValue(curObj);
                }
                partDefine lastPd = getPartType(parts[parts.Length - 1]);
                switch (type)
                {
                    case editType.insert:
                        lastPd.Insert(curObj,value);
                        break;
                    case editType.delete:
                        lastPd.Delete(curObj);
                        break;
                    case editType.edit:
                        lastPd.SetValue(curObj,value);
                        break;
                    default:
                        break;
                }
            }

            private partDefine getPartType(string p)
            {
                int arrayTagIndex = p.IndexOf("[");
                if (arrayTagIndex == 0)
                {
                    string key = p.Substring(1, p.Length - 2);
                    int index;
                    if (int.TryParse(key, out index))
                    {
                        return new partDefine()
                        {
                            type = partType.Array,
                            arrayIndex = index,
                        };
                    }
                    else
                    {
                        return new partDefine()
                        {
                            type = partType.Array,
                            dicKey = key,
                        };
                    }
                }
                else if (arrayTagIndex < 0)
                {
                    return new partDefine()
                    {
                        type = partType.Property,
                        property = p
                    };
                }
                else
                {
                    string key = p.Substring(arrayTagIndex + 1, p.Length - 2 - arrayTagIndex);
                    int index;
                    if (int.TryParse(key, out index))
                    {
                        return new partDefine()
                        {
                            type = partType.PropertyArray,
                            arrayIndex = index,
                            property = p.Substring(0,arrayTagIndex)
                        };
                    }
                    else
                    {
                        return new partDefine()
                        {
                            type = partType.PropertyDic,
                            dicKey = key,
                            property = p.Substring(0, arrayTagIndex)
                        };
                    }
                }
            }
        }

        private class partDefine
        {
            public string property { get; set; }
            public int arrayIndex { get; set; }
            public string dicKey { get; set; }
            public partType type { get; set; }

            internal object GetValue(object curObj)
            {
                object obj = null;
                switch (type)
                {
                    case partType.Array:
                        obj = GetArrayValue(curObj, arrayIndex);
                        break;
                    case partType.PropertyArray:
                        obj = GetPropertyValue(curObj, property);
                        obj = GetArrayValue(obj, arrayIndex);
                        break;
                    case partType.Property:
                        obj = GetPropertyValue(curObj, property);
                        break;
                    case partType.Dic:
                        obj = GetDicValue(curObj, dicKey);
                        break;
                    case partType.PropertyDic:
                        obj = GetPropertyValue(curObj, property);
                        obj = GetDicValue(obj, dicKey);
                        break;
                    default:
                        break;
                }
                return obj;
            }

            private object GetDicValue(object obj, string dicKey)
            {
                if(obj is IDictionary)
                {
                    IDictionary dic = (obj as IDictionary);
                    foreach (string item in dic.Keys)
                    {
                        if (dicKey == item)
                            return dic[item];
                    }
                }
                return null;
            }

            private object GetArrayValue(object obj, int arrayIndex)
            {
                if (obj is IEnumerable)
                {
                    int i = 0;
                    foreach (var item in obj as IEnumerable)
                    {
                        if (arrayIndex == i)
                            return item;
                        i++;
                    }
                }
                return null;
            }

            private object GetPropertyValue(object obj, string property)
            {
                return obj.GetType().GetProperty(property).GetValue(obj, null);
            }

            internal void SetValue(object curObj, string value)
            {
                switch (type)
                {
                    case partType.Array:
                        SetArrayValue(curObj, arrayIndex, value);
                        break;
                    case partType.PropertyArray:
                        object obj = GetPropertyValue(curObj, property);
                        SetArrayValue(obj, arrayIndex, value);
                        break;
                    case partType.Property:
                        SetPropertyValue(curObj, property, value);
                        break;
                    case partType.Dic:
                        SetDicValue(curObj, dicKey,value);
                        break;
                    case partType.PropertyDic:
                        object obj1 = GetPropertyValue(curObj, property);
                        SetDicValue(obj1, dicKey, value);
                        break;
                    default:
                        break;
                }
            }

            private void SetDicValue(object obj, string dicKey, string value)
            {
                if (obj is IDictionary)
                {
                    IDictionary dic = (obj as IDictionary);
                    Type valueType = obj.GetType().GetGenericArguments()[1];
                    object val = common.ChangeType(valueType,value);
                    foreach (string item in dic.Keys)
                    {
                        if (dicKey == item)
                        {
                            dic[item] = val;
                            return;
                        }
                    }
                    dic.Add(dicKey, val);
                }
            }

            private void SetPropertyValue(object obj, string property, string value)
            {
                PropertyInfo pi = obj.GetType().GetProperty(property) ;
                object val = common.ChangeType(pi.PropertyType, value);
                pi.SetValue(obj, val, null);
            }

            private void SetArrayValue(object obj, int arrayIndex, string value)
            {
                Type listType = obj.GetType();
                object val = common.ChangeType(listType.GetGenericArguments()[0], value);
                listType.GetMethod("Insert").Invoke(obj, new object[] { arrayIndex, val });
            }

            internal void Delete(object curObj)
            {
                switch (type)
                {
                    case partType.Array:
                        DeleteArray(curObj, arrayIndex);
                        break;
                    case partType.PropertyArray:
                        object obj = GetPropertyValue(curObj, property);
                        DeleteArray(obj, arrayIndex);
                        break;
                    case partType.Property:
                        break;
                    case partType.Dic:
                        DeleteDic(curObj, dicKey);
                        break;
                    case partType.PropertyDic:
                        object obj1 = GetPropertyValue(curObj, property);
                        DeleteDic(obj1, dicKey);
                        break;
                    default:
                        break;
                }
            }

            private void DeleteDic(object obj, string dicKey)
            {
                if (obj is IDictionary)
                {
                    IDictionary dic = (obj as IDictionary);
                    foreach (string item in dic.Keys)
                    {
                        if (dicKey == item)
                        {
                            dic.Remove(item);
                            break;
                        }
                    }
                }
            }

            private void DeleteArray(object obj, int arrayIndex)
            {
                Type listType = obj.GetType();
                listType.GetMethod("RemoveAt").Invoke(obj, new object[] { arrayIndex });
            }

            internal void Insert(object curObj,string value)
            {
                switch (type)
                {
                    case partType.Array:
                        InsertArray(curObj, arrayIndex, value);
                        break;
                    case partType.PropertyArray:
                        object obj = GetPropertyValue(curObj, property);
                        InsertArray(obj, arrayIndex, value);
                        break;
                    case partType.Property:
                        break;
                    case partType.Dic:
                        InsertDic(curObj, dicKey,value);
                        break;
                    case partType.PropertyDic:
                        object obj1 = GetPropertyValue(curObj, property);
                        InsertDic(obj1, dicKey, value);
                        break;
                    default:
                        break;
                }
            }

            private void InsertArray(object obj, int arrayIndex, string value)
            {
                SetArrayValue(obj, arrayIndex, value);
            }

            private void InsertDic(object obj, string dicKey, string value)
            {
                SetDicValue(obj, dicKey, value);
            }
        }

        private enum partType { 
            Array,
            PropertyArray,
            Property,
            Dic,
            PropertyDic
        }

        public enum editType
        {
            insert,
            delete,
            edit
        }
    }
}
