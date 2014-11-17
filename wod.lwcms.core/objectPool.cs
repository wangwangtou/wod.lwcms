using System;
using System.Collections.Generic;
using System.Text;

namespace wod.lwcms
{
    public class objectPool : IDisposable
    {
        private Dictionary<string, object> _pool = new Dictionary<string, object>();
        public Dictionary<string, object> Pool
        { get { return _pool; } }

        public object getObject(string key)
        {
            if (_pool.ContainsKey(key))
                return _pool[key];
            else
                return null;
        }

        public T getObject<T>(string key)
        {
            if (_pool.ContainsKey(key))
            {
                object obj = _pool[key];
                if (obj == null)
                    return default(T);
                else
                {
                    if (obj.GetType() == typeof(T))
                        return (T)obj;
                    else if (obj.GetType() == typeof(string))
                        return (T)common.ChangeType(typeof(T), (string)obj);
                    else
                        return default(T);
                }
            }
            else
                return default(T);
        }

        public void setOjbect(string key, object obj)
        {
            if (_pool.ContainsKey(key))
            {
                _pool[key] = obj;
            }
            else
            {
                _pool.Add(key, obj);
            }
        }

        public void Dispose()
        {
            foreach (object item in _pool.Values)
            {
                var dis = item as IDisposable;
                if (dis != null)
                    dis.Dispose();
            }
        }
    }
}
