using System;
using System.Collections.Generic;
using System.Text;

namespace wod.lwcms.commands
{
    public class commandsParameter
    {
        private ioc ioc;
        private objectPool op;
        public objectPool OP { get { return op; } } 

        public commandsParameter(ioc _ioc,objectPool _op)
        {
            this.ioc = _ioc;
            this.op = _op;
        }

        public object GetObject(string name)
        {
            return GetObject(name, null);
        }

        public object GetObject(string name, Type type)
        {
            var obj = op.getObject(name);
            if (obj == null)
            {
                obj = (ioc.GetInstance(name) ?? (type == null ?  null : ioc.GetService(type, GetConstructorParameter)));
                op.setOjbect(name, obj);
            }
            return obj;
        }

        public object GetObject(Type type)
        {
            return ioc.GetService(type, GetConstructorParameter);
        }

        public bool GetConstructorParameter(string name, Type type, out object obj)
        {
            bool has = op.Pool.ContainsKey(name);
            if (has)
            {
                obj = op.getObject(name);
                return obj != null && type.IsAssignableFrom(obj.GetType());
            }
            else
            {
                obj = null;
                return false;
            }
        }

        public void AddObject(string id, object data)
        {
            op.setOjbect(id, data);
        }
    }
}
