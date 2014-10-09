using System;
using System.Collections.Generic;
using System.Text;

namespace wod.lwcms.commands
{
    public abstract class command
    {
        public command()
        {
            attributes = TypeHelper.CanStringTypeWriteableProperty(this.GetType(), typeof(command));
        }

        public string id { get; set; }
        public string type { get; set; }

        public BoolExpress filterExpress { get; set; }

        public virtual bool canExcute(commandsParameter cp)
        {
            return filterExpress == null || filterExpress.GetExpressResult(cp);
        }
        public virtual void excute(commandsParameter cp)
        {
            if (canExcute(cp))
            {
                excuteNoCheck(cp);
            }
        }

        protected abstract void excuteNoCheck(commandsParameter cp);

        private readonly Dictionary<string,System.Reflection.PropertyInfo> attributes;

        public virtual void parseProperty(System.Xml.XmlNode node)
        {
            foreach (System.Xml.XmlAttribute item in node.Attributes)
            {
                if (attributes.ContainsKey(item.Name))
                {
                    attributes[item.Name].SetValue(this, item.Value, null);
                }
            }
        }
    }

    public class assCommand : command
    {
        public string typeName { get; set; }
        public string methodName { get; set; }

        protected override void excuteNoCheck(commandsParameter cp)
        {
            Type type = Type.GetType(typeName);
            var obj = cp.GetObject(type);

            var method = type.GetMethod(methodName);

            var objTypes = method.GetParameters();
            var pars = new object[objTypes.Length];
            for (int i = 0; i < objTypes.Length; i++)
            {
                pars[i] = cp.GetObject(objTypes[i].Name,objTypes[i].ParameterType);
            }
            var data = method.Invoke(obj, pars);

            cp.AddObject(id, data);
        }
    }

    public class sqlCommand : command
    {
        public string sql { get; set; }

        private string sqlExc { get; set; }
        public string paramenterPrefix { get; set; }

        public string excuteType { get; set; }
        public bool useTransaction { get; set; }
        public bool isCommit { get; set; }

        protected override void excuteNoCheck(commandsParameter cp)
        {
            var dataAcc = cp.GetObject("dataaccess",typeof(wod.lwcms.dataaccess.DataAccessContext)) as wod.lwcms.dataaccess.DataAccessContext;
            dynamicSQL(cp);
            var parameters = getParameters(cp);
            if (!dataAcc.IsOpen)
            {
                dataAcc.Open();
            }
            if (useTransaction)
            {
                dataAcc.BeginTransaction();
            }
            excute(dataAcc, sqlExc, parameters, cp);
            if (isCommit)
            {
                dataAcc.Commit();
            }
        }

        private void dynamicSQL(commandsParameter cp)
        {
            System.Text.RegularExpressions.Regex replaceReg = new System.Text.RegularExpressions.Regex(
                string.Format(@"[^{0}{0}](?<p>{0}\w+)", "#"));

            sqlExc = replaceReg.Replace(sql, new System.Text.RegularExpressions.MatchEvaluator((m) =>
            {
                var paraName = m.Groups["p"].Value.Substring(1);
               return m.Value.Replace("#"+paraName,
                 string.Format("{0}", cp.GetObject(paraName, null)));
            }));
        }

        protected virtual List<wod.lwcms.dataaccess.WODDbParameter> getParameters(commandsParameter cp) 
        {
            var parameters = new List<wod.lwcms.dataaccess.WODDbParameter>();
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(
                string.Format(@"[^{0}{0}](?<p>{0}\w+)", paramenterPrefix));
            var matches = reg.Matches(String.Concat(sql, " "));
            foreach (System.Text.RegularExpressions.Match m in matches)
            {
                var paraName = m.Groups["p"].Value.Substring(1);
                var obj =cp.GetObject(paraName,null) ?? "";
                parameters.Add(new dataaccess.WODDbParameter(paraName,
                     GetDbType(obj), obj));
            }
            return parameters;
        }

        private System.Data.DbType GetDbType(object obj)
        {
            switch (obj.GetType().FullName)
            {
                case "System.Int32":
                    return System.Data.DbType.Int32;
                case "System.Int64":
                    return System.Data.DbType.Int64;
                case "System.Single":
                    return System.Data.DbType.Single;
                case "System.Decimal":
                    return System.Data.DbType.Decimal;
                case "System.Double":
                    return System.Data.DbType.Double;
                case "System.DateTime":
                    DateTime dt = (DateTime)obj;
                        return System.Data.DbType.Date;
                    //if (dt.TimeOfDay == TimeSpan.Zero)
                    //{
                    //}
                    //else
                    //{
                    //    return System.Data.DbType.DateTime;
                    //}
                case "System.String":
                default:
                    return System.Data.DbType.String;
            }
        }

        protected virtual void excute(dataaccess.DataAccessContext dataAcc, string sql, List<dataaccess.WODDbParameter> parameters
            ,commandsParameter cp)
        {
            object obj = null;
            switch (excuteType)
            {
                case "datareader":
                    obj = dataAcc.QueryDataReader(sql, System.Data.CommandType.Text, parameters);
                    break;
                case "excute":
                    obj = dataAcc.ExecuteNonQuery(sql, System.Data.CommandType.Text, parameters);
                    break;
                case "scalar":
                default:
                    obj = dataAcc.QueryScalar(sql, System.Data.CommandType.Text, parameters);
                    break;
            }
            cp.AddObject(id, obj);
        }
    }

    //public class dataAccessCommand : command
    //{
    //    public string providerType { get; set; }
    //    public bool useTransaction { get; set; }

    //    public override void excute(commandsParameter cp)
    //    {
    //        System.Data.Common.DbProviderFactories.GetFactory(
    //    }
    //}

    public class multiCommand : command
    {
        public List<command> commands { get; set; }

        protected override void excuteNoCheck(commandsParameter cp)
        {
            foreach (var item in commands)
            {
                item.excute(cp);
            }
        }
    }

    public class emptyCommand : command
    {
        protected override void excuteNoCheck(commandsParameter cp)
        {
        }
    }
}
