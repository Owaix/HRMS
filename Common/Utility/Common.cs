using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utility
{
    public class Common
    {
        public static T GetConfigValue<T>(string key, T defaultValue)
        {
            object retVal = string.Empty;
            retVal = (null != ConfigurationManager.AppSettings[key]) ? ConfigurationManager.AppSettings[key].ToString() : defaultValue.ToString();
            return (T)retVal;
        }
    }
}
