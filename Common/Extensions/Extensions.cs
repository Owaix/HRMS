using System;
using System.Collections.Generic;
using System.Reflection;

namespace Common.Extensions
{
    public static class Extensions
    {
        #region ----- Common Conversion -----
        public static void SetValue(this PropertyInfo prop, object obj, object value)
        {
            prop.SetValue(obj, value, null);
        }

        /// <summary>
        /// it converts specified string into TitleCase
        /// i.e. "my title" to "My Title"
        /// </summary>
        /// <param name="objString"></param>
        /// <returns>string</returns>
        public static string ToTitleCase(this string objString)
        {
            System.Globalization.CultureInfo cultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Globalization.TextInfo textInfo = cultureInfo.TextInfo;
            return textInfo.ToTitleCase(objString);
        }

        /// <summary>
        /// Converts string into Integer(int). Returns 0 if conversion fails.         
        /// </summary>
        /// <param name="objString"></param>
        /// <returns>int</returns>
        public static int ToInt32(this object objString)
        {
            int obj = 0;
            try
            {
                obj = Convert.ToInt32(objString);
            }
            catch (Exception)
            {
                obj = 0;
            }
            return obj;
        }

        /// <summary>
        /// Converts string into two decimal point value. Returns 0 if conversion fails.         
        /// </summary>
        /// <param name="objString"></param>
        /// <returns>decimal</returns>
        public static decimal ToDecimal(this object objString)
        {
            decimal obj = 0;
            try { objString = objString.ToString().Replace("$", ""); }
            catch (Exception) { }

            try { obj = Convert.ToDecimal(objString); }
            catch (Exception) { obj = 0; }
            return Math.Round(obj, 2, MidpointRounding.ToEven);
        }

        /// <summary>
        /// Converts string into two decimal point value. Returns 0 if conversion fails.         
        /// </summary>
        /// <param name="objString"></param>
        /// <returns>decimal</returns>
        public static decimal ToDecimal(this object objString, int DecimalPlaces)
        {
            decimal obj = 0;
            try { obj = Convert.ToDecimal(objString); }
            catch (Exception) { obj = 0; }
            return Math.Round(obj, DecimalPlaces, MidpointRounding.ToEven);
        }

        /// <summary>
        /// Converts string into DateTime. Returns "1/1/1900" if conversion fails.         
        /// </summary>
        /// <param name="objString"></param>
        /// <returns>DateTime</returns>
        public static DateTime ToDateTime(this object objString)
        {
            DateTime obj = Convert.ToDateTime("1/1/1900");
            try
            {
                obj = Convert.ToDateTime(objString);
            }
            catch (Exception)
            {
                obj = Convert.ToDateTime("1/1/1900");
            }
            return obj;
        }

        /// <summary>
        /// Converts string to Bolean. i.e. "true", "false"
        /// </summary>
        /// <param name="objString"></param>
        /// <returns>DateTime</returns>
        public static bool ToBoolean(this object objString)
        {
            bool result;
            bool.TryParse(objString.ToString(), out result);
            return result;
        }

        /// <summary>
        /// Converts string to XML representation.
        /// </summary>
        /// <param name="ObjList"></param>
        /// <returns>string(XML Representation)</returns>
        public static string ToXML(this IEnumerable<object> ObjList)
        {
            string strXML = "<?xml version='1.0' encoding='UTF-8'?><NewDataSet>";

            foreach (object item in ObjList)
            {
                strXML += "<Table>";
                foreach (PropertyInfo _prop in item.GetType().GetProperties())
                {
                    strXML += "<" + _prop.Name + ">" + _prop.GetValue(item, null) + "</" + _prop.Name + ">";
                }
                strXML += "</Table>";
            }
            strXML += "</NewDataSet>";
            return strXML;
        }

        #endregion

        #region ----- DateTime -----

        /// <summary>
        /// Returns start date of the given DateTime. i.e. "1/15/1990" to "1/1/1990"
        /// </summary>
        /// <param name="objDateTime"></param>
        /// <returns>DateTime</returns>
        public static DateTime StartOfMonth(this DateTime objDateTime)
        {
            return Convert.ToDateTime(objDateTime.Month + "/1/" + objDateTime.Year);
        }

        public static DateTime StartOfWeek(this DateTime dt,DayOfWeek startOfWeek)
        {
            int diff = dt.DayOfWeek - startOfWeek;
            if (diff<0)
            {
                diff += 7;
            }
            return dt.AddDays(-1 * diff).Date;
        }

        /// <summary>
        /// Returns end date of the given DateTime. i.e. "1/15/1990" to "1/30/1990"
        /// </summary>
        /// <param name="objDateTime"></param>
        /// <returns>DateTime</returns>
        public static DateTime EndOfMonth(this DateTime objDateTime)
        {
            return objDateTime.StartOfMonth().AddMonths(1).AddDays(-1);
        }

        public static string Age(this DateTime dob)
        {
            int yrs = DateTime.Now.Year - dob.Year;
            int mts = DateTime.Now.Month - dob.Month;
            int dys = DateTime.Now.Day - dob.Day;
            if (yrs > 0) { return yrs.ToString() + " years"; }
            else if (mts > 0) { return mts.ToString() + " months"; }
            else if (dys > 0) { return dys.ToString() + " months"; }
            else { return "1 day"; }
        }

        public static DateTime GetLastDay(this DateTime objDateTime, DayOfWeek dow)
        {
            return objDateTime.AddDays(((int)objDateTime.DayOfWeek * -1) - (int)dow);
        }

        #endregion

        #region LogTypes

        #endregion
    }

    public enum LogType
    {
        SystemError = 1,
        Insert = 2,
        Update = 3,
        Delete = 4,
        Select = 5,
        Login = 6,
        Logout = 7,
        TemplateSchedulesubmit = 8,
        CustomSchedulesubmit = 9,
        SchedulePublished = 10,
        Scheduledelete = 11,
        Timeoffrequest = 12,
        Timeoffapprove = 13,
        TimeoffReject = 14,
        EmergencyOffrequest = 15,
        EmergencyOffapprove = 16,
        EmergencyOffReject = 17,
        UserAdded = 18,
        LocationAdded = 19,
        TemplateAssigned = 20,
        ScheduleTemplateUnAssigned = 21,
        TemplateCreated = 22,
        TemplateUpdated = 23,
        TemplateDeleted = 24,
        Email=25

        
    }

    public enum ScheduleType {
        Custom=1,
        Template=2

    }

}