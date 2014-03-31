using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;

namespace ParseToHtml
{
    public class JsonTemplateControl: UserControl
    {
        private JsonTemplateControl.TemplateData _td;

        public class TemplateData
	    {
            private LitJson.JsonData jsonData;

            internal TemplateData(LitJson.JsonData jsonData)
            {
                this.jsonData = jsonData;
            }

            public string getStr(string name) {
                return getData<string>(name);
            }
            public double getNum(string name) {
                return getData<double>(name);
            }
            public DateTime getDate(string name) {
                return getData<DateTime>(name);
            }
            public TemplateData getTd(string name)
            {
                var d = getJsonData(name);
                return new TemplateData(d);
            }

            public List<TemplateData> getList(string name) {
                var d = getJsonData(name);
                List<TemplateData> td = new List<TemplateData>();
                for (int i = 0; i < d.Count; i++)
                {
                    td.Add(new TemplateData(d[i]));
                }
                return td;
            }

            public T getData<T>(string name)
            {
                var d = getJsonData(name);
                return (T)Convert.ChangeType(d.ToString(), typeof(T));
            }

            private LitJson.JsonData getJsonData(string name)
            {
                if (string.IsNullOrEmpty(name))
                {
                    return jsonData;
                }
                string[] names = name.Split('.');
                LitJson.JsonData d = jsonData;
                for (int i = 0; i < names.Length; i++)
                {
                    d = d[names[i]];
                }
                return d;
            }
	    }

        public JsonTemplateControl.TemplateData TD
        {
            get { return _td; }
        }

        internal void SetTemplateData(LitJson.JsonData jsonData)
        {
            this._td = new TemplateData(jsonData);
        }
    }
}
