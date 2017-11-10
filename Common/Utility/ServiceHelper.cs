
using NLog;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utility
{
    public static class ServiceHelper
    {

        public delegate T EntityMethodDelegate<T>();
        private static readonly ILogger _logger;

        static ServiceHelper()
        {
            _logger = LogManager.GetCurrentClassLogger();
        }
        public static T ExecuteSafely<T>(EntityMethodDelegate<T> codeBlock)
        {
            T retVal = default(T);
            string expMessage = string.Empty;
            try
            {
                retVal = codeBlock();
            }
            catch (SqlException ex)
            {

                _logger.Error($"Sql Exception occured : { ex.Message} \n Stack Trace :  { ex.StackTrace }");
                // expMessage = ex.Message;
            }
            catch (IOException ex)
            {
                expMessage = ex.Message;
                _logger.Error($"IOException occured : { expMessage} \n Stack Trace :  { ex.StackTrace }");
            }
            // added this to catch exceptions other than sql and io exceptions : imran
            catch (Exception ex)
            {
                expMessage = ex.Message;
                _logger.Error($"Exception occured : { expMessage} \n Stack Trace :  { ex.StackTrace }");
            }

            return (retVal);
        }
    }
}
