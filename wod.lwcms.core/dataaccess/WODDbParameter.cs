using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace wod.lwcms.dataaccess
{
    public class WODDbParameter
    {

        public WODDbParameter(string parameterName, DbType dbType, object parameterValue, ParameterDirection parameterDirection = System.Data.ParameterDirection.Input)
        {
            // TODO: Complete member initialization
            this.ParameterName = parameterName;
            this.ParameterType = dbType;
            this.ParameterValue = parameterValue;
            this.ParameterDirection = parameterDirection;
        }
        public string ParameterName { get; set; }
        public ParameterDirection ParameterDirection { get; set; }
        public object ParameterValue { get; set; }
        public DbType ParameterType { get; set; }
    }
}
