using System;
using System.Collections.Generic;
using System.Text;

namespace wod.lwcms.commands
{
    public class BoolExpress
    {
        public bool GetExpressResult(commandsParameter cp)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            foreach (string key in keys.Keys)
            {
                object data = cp.GetObject(key,typeof(object));
                parameters.Add(key, data);
            }
            return GetExpressResult(parameters, _filter);
        }

        public bool GetExpressResult(Dictionary<string, object> parameters)
        {
            return GetExpressResult(parameters, _filter);
        }

        private bool GetExpressResult(Dictionary<string,object> parameters,filter filter)
        {
            if (filter.type == filterType.combi)
            {
                return GetCombiExpressResult(parameters, filter);
            }
            else
            {
                return GetSimpleExpressResult(parameters, filter);
            }
        }

        private bool GetCombiExpressResult(Dictionary<string, object> parameters, filter filter)
        {
            if (filter.logicaloper == "&&")
            {
                bool result = true;
                foreach (filter item in filter.filters)
                {
                    result = result && GetExpressResult(parameters, item);
                    if (!result)
                        break;
                }
                return result;
            }
            else if (filter.logicaloper == "||")
            {
                bool result = false;
                foreach (filter item in filter.filters)
                {
                    result = result || GetExpressResult(parameters, item);
                    if (result)
                        break;
                }
                return result;
            }
            return false;
        }

        private bool GetSimpleExpressResult(Dictionary<string, object> parameters, filter filter)
        {
            object left = GetObject(parameters, filter.left);
            object right = GetObject(parameters, filter.right);
            if (filter.left.isKeyName && !filter.right.isKeyName)
            {
                Type objType = left.GetType();
                try
                {
                    System.Reflection.MethodInfo parse = objType.GetMethod("Parse", new Type[] { typeof(string) });
                    if (parse != null)
                    {
                        right = parse.Invoke(null, new object[] { right });
                    }
                    else
                    {
                        right = Convert.ChangeType(right, objType);
                    }
                }
                catch (Exception)
                {
                }
            }
            else if (!filter.left.isKeyName && filter.right.isKeyName)
            {
                Type objType = right.GetType();
                try
                {
                    System.Reflection.MethodInfo parse = objType.GetMethod("Parse", new Type[] { typeof(string) });
                    if (parse != null)
                    {
                        left = parse.Invoke(null, new object[] { left });
                    }
                    else
                    {
                        left = Convert.ChangeType(left, objType);
                    }
                }
                catch (Exception)
                {
                }
            }
            return filter.oper.Compare(left, right);
        }

        private object GetObject(Dictionary<string, object> parameters, compareData data)
        {
            if (data.isKeyName)
            {
                return parameters[data.keyName];
            }
            else
            {
                return data.data;
            }
        }

        private BoolExpress(string exp)
        {
            _filter = new filter(exp);
            _filter.Parse();
            keys = new Dictionary<string, object>();
            getAllKeys(_filter);
        }

        private void getAllKeys(filter filter)
        {
            if (_filter.type == filterType.combi)
            {
                foreach (filter item in filter.filters)
                {
                    getAllKeys(item);
                }
            }
            else
            {
                if (filter.left.isKeyName && !keys.ContainsKey(filter.left.keyName))
                {
                    keys.Add(filter.left.keyName, null);
                }
                if (filter.right.isKeyName && !keys.ContainsKey(filter.right.keyName))
                {
                    keys.Add(filter.right.keyName, null);
                }
            }
        }

        private Dictionary<string, object> keys { get; set; }

        private filter _filter;

        public static BoolExpress CreateBoolExpress(string exp)
        {
            BoolExpress express = new BoolExpress(exp);
            return express;
        }

        public class compareData
        {
            public bool isKeyName { get; set; }

            public string keyName { get; set; }

            public object data { get; set; }
        }

        internal class filter
        {
            private string express;
            public filter(string express)
            {
                this.express = express.Trim();
                this.filters = new List<filter>();
            }

            //类型
            internal filterType type { get; set; }
            //left
            internal compareData left { get; set; }
            /// <summary>
            /// operator
            /// </summary>
            internal OperBase oper { get; set; }
            //right
            internal compareData right { get; set; }

            internal List<filter> filters { get; set; }
            internal string logicaloper { get; set; }

            internal void Parse()
            {
                string logicaloper;
                List<string> substrings = GetSubstring(express,out logicaloper);
                this.logicaloper = logicaloper;
                if (substrings.Count == 1)
                {
                    ParseSimple(express);
                }
                else
                {
                    type = filterType.combi;
                    foreach (var item in substrings)
                    {
                        string subExp = item.Trim();
                        if (subExp.StartsWith("(") && subExp.EndsWith(")"))
                        { 
                            subExp = subExp.Substring(1,subExp.Length - 2);
                        }
                        filter subFilter = new filter(subExp);
                        subFilter.Parse();
                        this.filters.Add(subFilter);
                    }
                }
            }

            private void ParseSimple(string express)
            {
                type = filterType.simple;
                foreach (string key in OperLst.Keys)
                {
                    int start = express.IndexOf(key);
                    if (start >= 0)
                    {
                        string leftdata = express.Substring(0, start - 1).Trim();
                        if (leftdata.StartsWith("@"))
                        {
                            this.left = new compareData(){
                                isKeyName = true,
                                keyName = leftdata.TrimStart('@')
                            };
                        }
                        else
                        {
                            this.left = new compareData(){
                                isKeyName = false,
                                data = leftdata
                            };
                        }
                        this.oper = OperLst[key];
                        string rightdata = express.Substring(start + key.Length).Trim();
                        if (rightdata.StartsWith("@"))
                        {
                            this.right = new compareData()
                            {
                                isKeyName = true,
                                keyName = rightdata.TrimStart('@')
                            };
                        }
                        else
                        {
                            this.right = new compareData()
                            {
                                isKeyName = false,
                                data = rightdata
                            };
                        }
                        break;
                    }
                }
            }

            //获取第一及括号的几组表达式，用&& 和 || 分隔，如果同一级没有括号，用了不同的逻辑运算符，取第一个逻辑运算符
            private List<string> GetSubstring(string express, out string logicaloper)
            {
                logicaloper = "&&";//
                int search = 0;
                int level = 0;
                bool isFirstLogic = true;
                List<string> str = new List<string>();
                try
                {

                    for (int i = 0; i < express.Length; i++)
                    {
                        switch (express[i])
                        {
                            case '&':
                                if (express[i + 1] == '&')
                                {
                                    if (level == 0)
                                    {
                                        str.Add(express.Substring(search, i - search - 1));
                                        search = i+2;
                                    }
                                    i++;
                                }
                                break;
                            case '|':
                                if (express[i + 1] == '|')
                                {
                                    if (level == 0)
                                    {
                                        str.Add(express.Substring(search, i - search - 1));
                                        search = i + 2;
                                        if (isFirstLogic)
                                        {
                                            logicaloper = "||";
                                            isFirstLogic = false;
                                        }
                                    }
                                    i++;
                                }
                                break;
                            case '(':
                                level++;
                                break;
                            case ')':
                                level--;
                                break;
                            default:
                                break;
                        }
                    }
                    if (search != express.Length)
                    {
                        str.Add(express.Substring(search, express.Length - search));
                    }
                    return str;
                }
                catch (Exception)
                {
                    throw new Exception("表达式错误！");
                }
            }
        }

        internal enum filterType
        {
            simple,
            combi
        }

        internal static Dictionary<string, OperBase> OperLst = new Dictionary<string, OperBase> { 
                //{"contains"                  ,  new Contains()},
                //{"doesnotcontains"           ,  new DoesNotContains       ()},
                //{"iscontainedin"             ,  new IsContainedIn         ()},
                //{"isnotcontainedin"          ,  new IsNotContainedIn      ()},
                //{"endswith"                  ,  new EndsWith              ()},
                //{"startswith"                ,  new StartsWith            ()},
                {"=="                 ,  new IsEqualTo             ()},
                {"!="              ,  new IsNotEqualTo          ()},
                {">="    ,  new IsGreaterThanOrEqualTo()},
                {">"             ,  new IsGreaterThan         ()},
                {"<="       ,  new IsLessThanOrEqualTo   ()},
                {"<"                ,  new IsLessThan            ()},
                //{"isempty"                   ,  new IsEmpty               ()},
                //{"isnotempty"                ,  new IsNotEmpty            ()},
                {"is null"                    ,  new IsNull                ()},
                {"is not null"                 ,  new IsNotNull             ()},
        };

        internal static Dictionary<string, string> LogicOperLst = new Dictionary<string, string>()
        {
            {"&&" ,"and"},
            {"||" ,"or"}
        };

        #region OperBase

        public abstract class OperBase
        {
            public abstract bool Compare(object left, object right);
        }

        public class IsEqualTo : OperBase
        {
            public override bool Compare(object left, object right)
            {
                return object.Equals(left, right);
            }
        }
        public class IsNotEqualTo : OperBase
        {
            public override bool Compare(object left, object right)
            {
                return !object.Equals(left, right);
            }
        }
        public class IsGreaterThan : OperBase
        {
            public override bool Compare(object left, object right)
            {
                return (left as IComparable).CompareTo(right) > 0;
            }
        }
        public class IsGreaterThanOrEqualTo : OperBase
        {
            public override bool Compare(object left, object right)
            {
                return (left as IComparable).CompareTo(right) >= 0;
            }
        }
        public class IsLessThan : OperBase
        {
            public override bool Compare(object left, object right)
            {
                return (left as IComparable).CompareTo(right) < 0;
            }
        }
        public class IsLessThanOrEqualTo : OperBase
        {
            public override bool Compare(object left, object right)
            {
                return (left as IComparable).CompareTo(right) <= 0;
            }
        }
        public class IsNull : OperBase
        {
            public override bool Compare(object left, object right)
            {
                return (left == null) || (left is DBNull);
            }
        }
        public class IsNotNull : OperBase
        {
            public override bool Compare(object left, object right)
            {
                return (left != null) && !(left is DBNull);
            }
        }
        #endregion
    }
}
