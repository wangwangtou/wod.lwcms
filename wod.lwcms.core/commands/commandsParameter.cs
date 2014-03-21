using System;
using System.Collections.Generic;
using System.Text;

namespace wod.lwcms.commands
{
    public class commandsParameter
    {
        private ioc ioc;
        private objectPool op;
        public commandsParameter(ioc _ioc,objectPool _op)
        {
            this.ioc = _ioc;
            this.op = _op;
        }

        internal object GetObject(string name, Type type)
        {
            var obj = op.getObject(name);
            if (obj == null)
            {
                obj = (ioc.GetInstance(name) ?? ioc.GetService(type));
                op.setOjbect(name, obj);
            }
            return obj;
        }

        internal object GetObject(Type type)
        {
            return ioc.GetService(type);
        }

        internal void AddObject(string id, object data)
        {
            op.setOjbect(id, data);
        }
    }
}
