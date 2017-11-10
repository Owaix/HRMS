using log4net;
using log4net.Config;
using System.Reflection;
using System.Data.SqlClient;
using System.IO;
using System;

namespace Common
{
    public static class Logging
    {
        private static ILog log;
        public delegate T EntityMethodDelegate<T>();
        static Logging()
        {
            XmlConfigurator.Configure();
            log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        }

        public static T ExecuteSafely<T>(EntityMethodDelegate<T> codeBlock)
        {
            T retVal = default(T);
            string expMessage = string.Empty;
            try
            {
                retVal = codeBlock();
                log.Info("Data Has Insert Successfully");
            }
            catch (SqlException ex)
            {
                log.Error($"Sql Exception occured : { ex.Message} \n Stack Trace :  { ex.StackTrace }");
                expMessage = ex.Message;
            }
            //catch (IOException ex)
            //{
            //    expMessage = ex.Message;
            //    _logger.Error($"IOException occured : { expMessage} \n Stack Trace :  { ex.StackTrace }");
            //}
            //// added this to catch exceptions other than sql and io exceptions : imran
            //catch (Exception ex)
            //{
            //    expMessage = ex.Message;
            //    _logger.Error($"Exception occured : { expMessage} \n Stack Trace :  { ex.StackTrace }");
            //}
            return (retVal);
        }
    }
}
