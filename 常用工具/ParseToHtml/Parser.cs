using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using LitJson;
using System.Collections;

namespace ParseToHtml
{
    public class Parser
    {
        public JsonData ParseForm(NameValueCollection nameValueCollection)
        {
            JsonData data = new JsonData();
            foreach (string key in nameValueCollection.Keys)
            {
                string[] keys = ParseKeys(key);
                JsonData subData = data;
                for (int i = 0; i < keys.Length; i++)
                {
                    if (i == keys.Length - 1)
                    {
                        subData[keys[i]] = nameValueCollection[key];
                    }
                    else
                    {
                        if (!(subData as IDictionary).Contains(keys[i]))
                        {
                            subData[keys[i]] = new JsonData();
                        }
                        subData = subData[keys[i]];
                    }
                }
            }
            return data;
        }

        protected virtual string[] ParseKeys(string key)
        {
            List<string> keys = new List<string>();
            int s ;
            int e ;
            do
            {
                s = key.IndexOf("[");
                e = key.IndexOf("]");
                if (s < 0)
                {
                    keys.Add(key);
                }
                else if (s == 0)
                {
                    keys.Add(key.Substring(1, e - 1));
                    key = key.Remove(0, e+1);
                }
                else
                {
                    keys.Add(key.Substring(0, s));
                    key = key.Remove(0, s);
                }
            } while (s >= 0 && key.Length > 0);
            return keys.ToArray();
        }

        public string ParseToHtml(JsonData jsonData, string tempPath)
        {
            System.Web.UI.Page p = new System.Web.UI.Page();
            var control = p.LoadControl(tempPath) as JsonTemplateControl;
            control.SetTemplateData(jsonData);
            using (System.IO.StringWriter sw = new System.IO.StringWriter())
            {
                var htw = new System.Web.UI.HtmlTextWriter(sw);
                control.RenderControl(htw);
                return sw.ToString();
            }
        }

        public string ParseJsonFileToHtml(string jsonFile, string tempPath)
        {
            if(jsonFile.StartsWith("~/"))
            {
                jsonFile = System.Web.HttpContext.Current.Server.MapPath(jsonFile);
            }
            string jsonDataStr = System.IO.File.ReadAllText(jsonFile);
            return ParseToHtml(jsonDataStr, tempPath);
        }

        public string ParseToHtml(string jsonDataStr, string tempPath)
        {
            LitJson.JsonData jsonData = LitJson.JsonMapper.ToObject(jsonDataStr);
            return ParseToHtml(jsonData, tempPath);
        }

        public string ParseToHtml(object data, string tempPath)
        {
            LitJson.JsonData jsonData = new LitJson.JsonData(data);
            return ParseToHtml(jsonData, tempPath);
        }
    }
}
