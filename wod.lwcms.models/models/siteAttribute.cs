using System;
using System.Collections.Generic;
using System.Text;

namespace wod.lwcms.models
{
    public class siteAttribute
    {
        public List<ioc> ioc { get; set; }
        public List<addin> extensions { get; set; }
        public List<addin> plugins { get; set; }
        public svrTemplate svrTemplate { get; set; }
        public theme theme { get; set; }
        public List<svrTemplate> svrTemplates { get; set; }
        public List<theme> themes { get; set; }
    }

    public class ioc
    {
        public enum ioctype
        {
            Instance, Type
        }
        public enum iocdatatype
        {
            Int,
            String,
            DbProviderFactory,
            UnKown
        }
        public string name { get; set; }
        public ioctype type { get; set; }
        public iocdatatype datatype { get; set; }
        public string value { get; set; }

        public string target { get; set; }
        public string realize { get; set; }
    }

    public class addinSetting
    {
        public string name { get; set; }
        public string description { get; set; }
        public string value { get; set; }
    }

    public class addin
    {
        public string name { get; set; }
        public string description { get; set; }
        public string type { get; set; }
        public List<addinSetting> addinSettings { get; set; }
    }
    public class svrTemplate
    {
        public string name { get; set; }
        public string description { get; set; }
    }
    public class theme
    {
        public string name { get; set; }
        public string description { get; set; }
        public string preImage { get; set; }
    }
}
