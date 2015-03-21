using System;
using System.Collections.Generic;
using System.Text;
using ComLib;
using System.Xml;

namespace wod.lwcms.commands
{
    public class validCommand : command
    {
        //private string 
        protected override void excuteNoCheck(commandsParameter cp)
        {
            List<validationError> errors = new List<validationError>();
            foreach (validation item in validations)
            {
                if (!validate(item, cp))
                {
                    errors.Add(new validationError() { message = item.errorMessage, name = item.valueName });
                }
                else
                {
                    if(item.convertType && string.IsNullOrEmpty( item.valueType))
                        convert(item, cp);
                }
            }
            cp.AddObject(id, errors.Count == 0);
            cp.AddObject(id+".errors", errors);
        }

        private void convert(validation item, commandsParameter cp)
        {
        }

        private bool validate(validation item, commandsParameter cp)
        {
            object val = cp.GetObject(item.valueName,null);
            bool result;
            switch (item.validType)
            {
                case "IsEquals": result = string.Format("{0}", val) == item.validParameters[0]; break;
                case "IsNotNull": result = !string.IsNullOrEmpty(string.Format("{0}", val).Trim()); break;
                case "IsStringLengthMatch": result = Validation.IsStringLengthMatch(string.Format("{0}", val).Trim(), false, true, true, int.Parse(item.validParameters[0]), int.Parse(item.validParameters[1])); break;
                case "IsStringLengthMatch?": result = Validation.IsStringLengthMatch(string.Format("{0}", val).Trim(), true, true, true, int.Parse(item.validParameters[0]), int.Parse(item.validParameters[1])); break;
                case "IsStringLengthMatchNoTrim": result = Validation.IsStringLengthMatch(string.Format("{0}", val), false, true, true, int.Parse(item.validParameters[0]), int.Parse(item.validParameters[1])); break;
                case "IsStringLengthMatchNoTrim?": result = Validation.IsStringLengthMatch(string.Format("{0}", val), true, true, true, int.Parse(item.validParameters[0]), int.Parse(item.validParameters[1])); break;
                case "IsDate": result = Validation.IsDate(string.Format("{0}", val).Trim()); break;
                case "IsDate?": result = string.IsNullOrEmpty(string.Format("{0}", val).Trim()) || Validation.IsDate(string.Format("{0}", val).Trim()); break;
                //case "IsMatchRegEx": result = Validation.IsDate(string.Format("{0}", val).Trim()); break;
                case "IsNumeric": result = Validation.IsNumeric(string.Format("{0}", val),false, null, ""); break;
                case "IsNumeric?": result = Validation.IsNumeric(string.Format("{0}", val),  true, null, ""); break;
                case "IsEmail": result = Validation.IsEmail(string.Format("{0}", val), false); break;
                case "IsEmail?": result = Validation.IsEmail(string.Format("{0}", val), true); break;
                default:
                    try
                    {
                        var type = wodEnvironment.GetTypeByName(item.validType);
                        var validation = cp.GetObject(type) as IUserValidation;
                        result = validation.validate(string.Format("{0}", val), item.validParameters, cp);
                    }
                    catch (Exception)
                    {
                        result = false;
                    }
                    break;
            }
            return result;
            //public static bool IsAlpha(string text, bool allowNull);
            //public static bool IsAlpha(string text, bool allowNull, IErrors errors, string tag);
            //public static bool IsAlphaNumeric(string text, bool allowNull);
            //public static bool IsAlphaNumeric(string text, bool allowNull, IErrors errors, string tag);
            //public static bool IsBetween(int val, bool checkMinLength, bool checkMaxLength, int minLength, int maxLength);
            //public static bool IsDate(string text);
            //public static bool IsDate(string text, IErrors errors, string tag);
            //public static bool IsDateWithinRange(DateTime date, bool checkMinBound, bool checkMaxBound, DateTime minDate, DateTime maxDate);
            //public static bool IsDateWithinRange(string text, bool checkMinBound, bool checkMaxBound, DateTime minDate, DateTime maxDate);
            //public static bool IsDateWithinRange(DateTime date, bool checkMinBound, bool checkMaxBound, DateTime minDate, DateTime maxDate, IErrors errors, string tag);
            //public static bool IsDateWithinRange(string text, bool checkMinBound, bool checkMaxBound, DateTime minDate, DateTime maxDate, IErrors errors, string tag);
            //public static bool IsEmail(string text, bool allowNull);
            //public static bool IsEmail(string text, bool allowNull, IErrors errors, string tag);
            //public static bool IsMatchRegEx(string text, bool allowNull, string regExPattern);
            //public static bool IsNumeric(string text);
            //public static bool IsNumeric(string text, bool allowNull, IErrors errors, string tag);
            //public static bool IsNumericWithinRange(double num, bool checkMinBound, bool checkMaxBound, double min, double max);
            //public static bool IsNumericWithinRange(string text, bool checkMinBound, bool checkMaxBound, double min, double max);
            //public static bool IsNumericWithinRange(double num, bool checkMinBound, bool checkMaxBound, double min, double max, IErrors errors, string tag);
            //public static bool IsNumericWithinRange(string text, bool checkMinBound, bool checkMaxBound, double min, double max, IErrors errors, string tag);
            //public static bool IsPhoneUS(int phone);
            //public static bool IsPhoneUS(string text, bool allowNull);
            //public static bool IsPhoneUS(int phone, IErrors errors, string tag);
            //public static bool IsPhoneUS(string text, bool allowNull, IErrors errors, string tag);
            //public static bool IsSizeBetween(int val, bool checkMinLength, bool checkMaxLength, int minKilobytes, int maxKilobytes);
            //public static bool IsSsn(int ssn);
            //public static bool IsSsn(string text, bool allowNull);
            //public static bool IsSsn(int ssn, IErrors errors, string tag);
            //public static bool IsSsn(string text, bool allowNull, IErrors errors, string tag);
            //public static bool IsStringIn(string text, bool allowNull, bool compareCase, string[] allowedValues, IErrors errors, string tag);
            //public static bool IsStringLengthMatch(string text, bool allowNull, bool checkMinLength, bool checkMaxLength, int minLength, int maxLength);
            //public static bool IsStringLengthMatch(string text, bool allowNull, bool checkMinLength, bool checkMaxLength, int minLength, int maxLength, IErrors errors, string tag);
            //public static bool IsStringRegExMatch(string text, bool allowNull, string regEx);
            //public static bool IsStringRegExMatch(string text, bool allowNull, string regEx, IErrors errors, string tag);
            //public static bool IsTimeOfDay(string time);
            //public static bool IsTimeOfDay(string time, IErrors errors, string tag);
            //public static bool IsTimeOfDayWithinRange(string time, bool checkMinBound, bool checkMaxBound, TimeSpan min, TimeSpan max);
            //public static bool IsTimeOfDayWithinRange(TimeSpan time, bool checkMinBound, bool checkMaxBound, TimeSpan min, TimeSpan max);
            //public static bool IsTimeOfDayWithinRange(string time, bool checkMinBound, bool checkMaxBound, TimeSpan min, TimeSpan max, IErrors errors, string tag);
            //public static bool IsTimeOfDayWithinRange(TimeSpan time, bool checkMinBound, bool checkMaxBound, TimeSpan min, TimeSpan max, IErrors errors, string tag);
            //public static bool IsUrl(string text, bool allowNull);
            //public static bool IsUrl(string text, bool allowNull, IErrors errors, string tag);
            //public static bool IsZipCode(string text, bool allowNull);
            //public static bool IsZipCode(string text, bool allowNull, IErrors errors, string tag);
            //public static bool IsZipCodeWith4Char(string text, bool allowNull);
            //public static bool IsZipCodeWith4Char(string text, bool allowNull, IErrors errors, string tag);
            //public static bool IsZipCodeWith4CharOptional(string text, bool allowNull, IErrors errors, string tag);
            

            //Validation.IsAlpha("123abc", false);
            //Validation.IsAlphaNumeric("123abc", false);
            //Validation.IsDate("08-29-2009");
            //Validation.IsNumeric("asdklf");
            //Validation.IsNumeric("-234.23");
            //Validation.IsPhoneUS("800-456-7890", false);
            //Validation.IsSsn("123-45-7890", false);
            //Validation.IsTimeOfDay("7:45 am");
            //Validation.IsStringLengthMatch("user01", false, true, true, 2, 12);
        }

        public List<validation> validations { get; set; }

        public override void parseProperty(System.Xml.XmlNode node)
        {
            validations = new List<validation>();
            foreach (XmlNode item in node.SelectNodes("validation"))
            {
                validation va = new validation();
                if (item.Attributes["validType"] == null)
                {
                    va.validType = item.SelectSingleNode("validType").InnerText;
                }
                else
                {
                    va.validType = item.Attributes["validType"].Value;
                }
                va.validParameters = new List<string>();
                foreach (XmlNode para in item.SelectNodes("validParameter"))
                {
                    va.validParameters.Add(para.InnerText);
                }
                va.valueName = item.Attributes["valueName"] == null ? "" : item.Attributes["valueName"].Value;
                va.valueType = item.Attributes["valueType"] == null ? "" : item.Attributes["valueType"].Value;
                va.errorMessage = item.Attributes["errorMessage"] == null ? "" : item.Attributes["errorMessage"].Value;
                va.convertType = item.Attributes["convertType"] == null || item.Attributes["convertType"].Value == "true";
                validations.Add(va);
            }
            base.parseProperty(node);
        } 
    }

    public class validationError
    {
        public string name { get; set; }
        public string message { get; set; } 
    }

    public class validation
    {
        public string validType { get; set; }
        public List<string> validParameters { get; set; }
        public string valueName { get; set; }
        public bool convertType { get; set; }
        public string valueType { get; set; }
        public string errorMessage { get; set; }
    }

    public interface IUserValidation
    {
        bool validate(string value,List<string> validParameters, commandsParameter cp);

        object convert(string value,string convertType, commandsParameter cp);
    }
}
