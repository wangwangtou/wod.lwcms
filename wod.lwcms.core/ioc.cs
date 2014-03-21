using System;
using System.Collections.Generic;
using System.Text;
using wod.lwcms.dataaccess;
using wod.lwcms.services;

namespace wod.lwcms
{
    public class ioc : IServiceProvider
    {
        public ioc()
        {
            dicType = new Dictionary<Type, Type>();
            dicInstance = new Dictionary<string, object>();
        }

        public void Regist(Type target, Type real)
        {
            dicType.Add(target, real);
        }

        public void RegistInstance(string name, object obj)
        {
            dicInstance.Add(name, obj);
        }

        private Dictionary<Type, Type> dicType;
        private Dictionary<string, object> dicInstance;

        public object GetService(Type serviceType)
        {
            Type type;
            if (!dicType.ContainsKey(serviceType))
                type = serviceType;
            else
                type = dicType[serviceType];
            var cons = type.GetConstructors();
            if (cons.Length >= 1)
            {
                var con = cons[0];
                var obj = con.GetParameters();
                var pars = new object[obj.Length];
                for (int i = 0; i < obj.Length; i++)
                {
                    pars[i] = GetInstance(obj[i].Name) ?? GetService(obj[i].ParameterType);
                }
                return con.Invoke(pars);
            }
            else
            {
                return null;
            }
        }

        public object GetInstance(string name)
        {
            return dicInstance.ContainsKey(name) ? dicInstance[name] : null;
        }

        public T GetInstance<T>(string name)
        {
            return (T)GetInstance(name);
        }

        public T GetService<T>()
        {
            return (T)GetService(typeof(T));
        }

        internal void Regist(ioc _ioc)
        {
            foreach (string item in _ioc.dicInstance.Keys)
            {
                if (!this.dicInstance.ContainsKey(item))
                    this.RegistInstance(item, _ioc.dicInstance[item]);
            }
            foreach (Type item in _ioc.dicType.Keys)
            {
                if (!this.dicType.ContainsKey(item))
                    this.Regist(item, _ioc.dicType[item]);
            }
        }
    }
}
