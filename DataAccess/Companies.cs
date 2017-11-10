//using Com.LT.Scheduler.Entities;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.Entity;

namespace DataAccess
{
    /// <summary>
    /// Companies DBContext class that execute methods against database
    /// </summary>
    class CompaniesContext : DbContext
    {
        #region ----- Stored Procedures -----

        /// <summary>
        /// Accept Companies class parameters and insert a new row in Companies table
        /// 
        /// return compID of newly inserted row(s)
        /// </summary>
        /// <param name="@compID">Int32 compID of Companies</param>
        /// <param name="@compName">String compName of Companies</param>
        /// <param name="@compDescr">String compDescr of Companies</param>
        /// <param name="@active">Boolean active of Companies</param>
        /// <param name="@crtBy">String crtBy of Companies</param>
        /// <param name="@crtDate">DateTime crtDate of Companies</param>
        /// <param name="@modBy">String modBy of Companies</param>
        /// <param name="@modDate">DateTime modDate of Companies</param>
        /// <returns>compID of newly inserted row(s)</returns>
        private static readonly string Companies_Insert = "Companies_Insert";

        /// <summary>
        /// Accept Companies class parameters and update selected row in Companies table
        /// 
        /// return compID of updated row(s)
        /// </summary>
        /// <param name="@compID">Int32 compID of Companies</param>
        /// <param name="@compName">String compName of Companies</param>
        /// <param name="@compDescr">String compDescr of Companies</param>
        /// <param name="@active">Boolean active of Companies</param>
        /// <param name="@crtBy">String crtBy of Companies</param>
        /// <param name="@crtDate">DateTime crtDate of Companies</param>
        /// <param name="@modBy">String modBy of Companies</param>
        /// <param name="@modDate">DateTime modDate of Companies</param>
        /// <returns>compID of updated row(s)</returns>
        private static readonly string Companies_Update = "Companies_Update";

        /// <summary>
        /// Accepts Companies object, delete row(s) from Companies table
		///
        /// return compID of deleted row(s)
        /// </summary>
        /// <param name="@compID">Int32 compID of Companies</param>
        /// <returns>compID of deleted row(s)</returns>
        private static readonly string Companies_Delete = "Companies_Delete";

        /// <summary>
        /// Accepts Companies object, attach all properties as SP parameters
        /// executes Companies_Select stores procedure and 
        /// return filtered row(s) from Companies table
        /// </summary>
        /// <param name="@compID">Int32 compID of Companies</param>
        /// <param name="@compName">String compName of Companies</param>
        /// <param name="@compDescr">String compDescr of Companies</param>
        /// <param name="@active">Boolean active of Companies</param>
        /// <param name="@crtBy">String crtBy of Companies</param>
        /// <param name="@crtDate">DateTime crtDate of Companies</param>
        /// <param name="@modBy">String modBy of Companies</param>
        /// <param name="@modDate">DateTime modDate of Companies</param>
        /// <returns>Filtered row(s) from Companies table</returns>
        private static readonly string Companies_Select = "Companies_Select";

        #endregion

        #region ----- CURD Methods ----------

        /// <summary>
        /// Accepts Companies object, insert into Companies table
        /// 
        /// return compID of newly inserted row(s)
        /// </summary>
        /// <param name="objCompanies">Companies object</param>
        /// <returns>compID of newly inserted row(s)</returns>
        public static Int32 Insert(Companies objCompanies)
        {
            SqlCommand sqlCmd = new SqlCommand(Companies_Insert, SqlHelper.DefaultSqlConnection);

            try
            {
                sqlCmd.Connection.Open();
                sqlCmd.Transaction = sqlCmd.Connection.BeginTransaction();
                objCompanies.compID = Insert(objCompanies, sqlCmd);
                sqlCmd.Transaction.Commit();
                return objCompanies.compID;
            }
            catch (Exception ex)
            {
                sqlCmd.Transaction.Rollback();
                throw ex;
            }
            finally { sqlCmd.Connection.Close(); }
        }

        /// <summary>
        /// Accepts Companies and SqlCommand objects, insert into Companies table
        /// 
        /// return compID of newly inserted row(s)
        /// </summary>
        /// <param name="Companies">Companies object</param>
        /// <param name="sqlCmd">SqlCommand object</param>
        /// <returns>compID of newly inserted row(s)</returns>
        public static Int32 Insert(Companies objCompanies, SqlCommand sqlCmd)
        {
            // Validating provided SqlCommand
            if (sqlCmd.Connection == null || sqlCmd.Connection.State == ConnectionState.Closed) throw new Exception("Error in provided Connection. Please set SqlCommand.Connection = new SqlConnection(string ConnectionString); or in other ways.");
            if (sqlCmd.Transaction == null) throw new Exception("Error in provided SqlTransaction. Please set sqlCmd.Transaction = sqlCmd.Connection.BeginTransaction(); or in other ways.");

            objCompanies.Validate(SqlOperation.Insert);
            sqlCmd = AttachParameters(sqlCmd, objCompanies, SqlOperation.Insert);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = Companies_Insert;

            try
            {
                objCompanies.compID = (Int32)SqlHelper.ExecuteNonQuery(sqlCmd);

                /// Validating if any required field does not have initial value
                if (objCompanies.compID < 1)
                    throw new Exception("Could not insert Companies. Please check your Data Access block or Companies_Insert Stored Procedure.");
                else
                {
                    //Save objEmailConfigList data
                    objCompanies.objEmailConfigList.SetProperty("compID", objCompanies.compID);
                    EmailConfigContext.Insert(objCompanies.objEmailConfigList, sqlCmd);

                    //Save objCompanySSOList data
                    objCompanies.objCompanySSOList.SetProperty("compID", objCompanies.compID);
                    CompanySSOContext.Insert(objCompanies.objCompanySSOList, sqlCmd);

                    //Save objCompanySettingsList data
                    objCompanies.objCompanySettingsList.SetProperty("compID", objCompanies.compID);
                    CompanySettingsContext.Insert(objCompanies.objCompanySettingsList, sqlCmd);

                    return objCompanies.compID;
                }
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// Accepts CompaniesList object, insert into Companies table
        /// 
        /// return  total number of rows affected
        /// </summary>
        /// <param name="objCompaniesList">CompaniesList object</param>
        /// <returns>Total number of rows affected</returns>
        public static Int32 Insert(CompaniesList objCompaniesList)
        {
            SqlCommand sqlCmd = new SqlCommand(Companies_Insert, SqlHelper.DefaultSqlConnection);
            try
            {
                sqlCmd.Connection.Open();
                sqlCmd.Transaction = sqlCmd.Connection.BeginTransaction();
                Insert(objCompaniesList, sqlCmd);
                sqlCmd.Transaction.Commit();
                return objCompaniesList.Count;
            }
            catch (Exception ex)
            {
                sqlCmd.Transaction.Rollback();
                throw ex;
            }
            finally { sqlCmd.Connection.Close(); }
        }

        /// <summary>
        /// Accepts CompaniesList and SqlCommand object , insert into Companies table
        /// 
        /// return  total number of rows affected
        /// </summary>
        /// <param name="objCompaniesList">CompaniesList object</param>
        /// <param name="sqlCmd">SqlCommand object</param>
        /// <returns>Total number of rows affected</returns>
        public static Int32 Insert(CompaniesList objCompaniesList, SqlCommand sqlCmd)
        {
            try
            {
                foreach (Companies item in objCompaniesList)
                {
                    if (item.compID == 0)
                    { item.compID = Insert(item, sqlCmd); }
                    else if (item.compID > 0) { Update(item, sqlCmd); }
                    else { throw new InvalidOperationException("Companies object was not in specified format"); }

                }
                return objCompaniesList.Count;
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// Accepts Companies object, updated row(s) into Companies table
		///
        /// return compID of updated row(s)
        /// </summary>
        /// <param name="Companies">Companies object</param>
        /// <returns>compID of updated row(s)</returns>
        public static Int32 Update(Companies objCompanies)
        {
            SqlCommand sqlCmd = new SqlCommand(Companies_Update, SqlHelper.DefaultSqlConnection);

            try
            {
                sqlCmd.Connection.Open();
                sqlCmd.Transaction = sqlCmd.Connection.BeginTransaction();

                if (objCompanies.compID == 0)
                { objCompanies.compID = Insert(objCompanies, sqlCmd); }
                else if (objCompanies.compID > 0) { Update(objCompanies, sqlCmd); }
                sqlCmd.Transaction.Commit();
                return objCompanies.compID;
            }
            catch (Exception ex)
            {
                sqlCmd.Transaction.Rollback();
                throw ex;
            }
            finally { sqlCmd.Connection.Close(); }
        }

        /// <summary>
        /// Accepts Companies object, updated row(s) into Companies table
		///
        /// return compID of updated row(s)
        /// </summary>
        /// <param name="Companies">Companies object</param>
        /// <param name="sqlCmd">SqlCommand object</param>
        /// <returns>compID of updated row(s)</returns>
        public static Int32 Update(Companies objCompanies, SqlCommand sqlCmd)
        {
            // Validating provided SqlCommand
            if (sqlCmd.Connection == null || sqlCmd.Connection.State == ConnectionState.Closed) throw new Exception("Error in provided Connection. Please set SqlCommand.Connection = new SqlConnection(string ConnectionString); or in other ways.");
            if (sqlCmd.Transaction == null) throw new Exception("Error in provided SqlTransaction. Please set sqlCmd.Transaction = sqlCmd.Connection.BeginTransaction(); or in other ways.");

            objCompanies.Validate(SqlOperation.Update);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = Companies_Update;
            sqlCmd = AttachParameters(sqlCmd, objCompanies, SqlOperation.Update);

            try
            {
                Int32 retval = (Int32)SqlHelper.ExecuteNonQuery(sqlCmd);
                return objCompanies.compID;
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// Accepts CompaniesList object, batch updated row(s) into Companies table
		///
        /// return compID of updated row(s)
        /// </summary>
        /// <param name="CompaniesList">CompaniesList object</param>
        /// <returns>compID of updated row(s)</returns>
        public static Int32 Update(CompaniesList objCompaniesList)
        {
            SqlCommand sqlCmd = new SqlCommand(Companies_Update, SqlHelper.DefaultSqlConnection);
            try
            {
                sqlCmd.Connection.Open();
                sqlCmd.Transaction = sqlCmd.Connection.BeginTransaction();
                Update(objCompaniesList, sqlCmd);
                sqlCmd.Transaction.Commit();
                return objCompaniesList.Count;
            }
            catch (Exception ex)
            {
                sqlCmd.Transaction.Rollback();
                throw ex;
            }
            finally { sqlCmd.Connection.Close(); }
        }

        /// <summary>
        /// Accepts CompaniesList object, batch updated row(s) into Companies table
		///
        /// return compID of updated row(s)
        /// </summary>
        /// <param name="Companies">Companies object</param>
        /// <param name="sqlCmd">SqlCommand object</param>
        /// <returns>compID of updated row(s)</returns>
        public static Int32 Update(CompaniesList objCompaniesList, SqlCommand sqlCmd)
        {
            try
            {
                foreach (Companies item in objCompaniesList)
                {
                    if (item.compID == 0)
                    { item.compID = Insert(item, sqlCmd); }
                    else if (item.compID > 0) { Update(item, sqlCmd); }
                    else { throw new InvalidOperationException("Companies object was not in specified format"); }

                }
                return objCompaniesList.Count;
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// Accepts Companies object, delete row(s) from Companies table
		///
        /// return compID of deleted row(s)
        /// </summary>
        /// <param name="Companies">Companies object</param>
        /// <returns>compID of deleted row(s)</returns>
        public static Int32 Delete(Companies objCompanies)
        {
            SqlCommand sqlCmd = new SqlCommand(Companies_Delete, SqlHelper.DefaultSqlConnection);

            try
            {
                sqlCmd.Connection.Open();
                sqlCmd.Transaction = sqlCmd.Connection.BeginTransaction();
                Int32 retval = Delete(objCompanies, sqlCmd);
                sqlCmd.Transaction.Commit();
                return objCompanies.compID;
            }
            catch (Exception ex)
            {
                sqlCmd.Transaction.Rollback();
                throw ex;
            }
            finally { sqlCmd.Connection.Close(); }
        }

        /// <summary>
        /// Accepts Companies object, delete row(s) from Companies table
		///
        /// return compID of deleted row(s)
        /// </summary>
        /// <param name="Companies">Companies object</param>
        /// <param name="sqlCmd">SqlCommand object</param>
        /// <returns>compID of deleted row(s)</returns>
        public static Int32 Delete(Companies objCompanies, SqlCommand sqlCmd)
        {
            // Validating provided SqlCommand
            if (sqlCmd.Connection == null || sqlCmd.Connection.State == ConnectionState.Closed) throw new Exception("Error in provided Connection. Please set SqlCommand.Connection = new SqlConnection(string ConnectionString); or in other ways.");
            if (sqlCmd.Transaction == null) throw new Exception("Error in provided SqlTransaction. Please set sqlCmd.Transaction = sqlCmd.Connection.BeginTransaction(); or in other ways.");

            objCompanies.Validate(SqlOperation.Delete);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = Companies_Delete;
            sqlCmd = AttachParameters(sqlCmd, objCompanies, SqlOperation.Delete);

            try
            {
                Int32 retval = (Int32)SqlHelper.ExecuteNonQuery(sqlCmd);
                return objCompanies.compID;
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// Accepts Companies object, batch delete row(s) from Companies table
		///
        /// return compID of deleted row(s)
        /// </summary>
        /// <param name="Companies">Companies object</param>
        /// <returns>compID of deleted row(s)</returns>
        public static Int32 Delete(CompaniesList objCompaniesList)
        {
            SqlCommand sqlCmd = new SqlCommand(Companies_Delete, SqlHelper.DefaultSqlConnection);
            try
            {
                sqlCmd.Connection.Open();
                sqlCmd.Transaction = sqlCmd.Connection.BeginTransaction();
                Delete(objCompaniesList, sqlCmd);
                sqlCmd.Transaction.Commit();
                return objCompaniesList.Count;
            }
            catch (Exception ex)
            {
                sqlCmd.Transaction.Rollback();
                throw ex;
            }
            finally { sqlCmd.Connection.Close(); }
        }

        /// <summary>
        /// Accepts Companies object, batch delete row(s) from Companies table
		///
        /// return compID of deleted row(s)
        /// </summary>
        /// <param name="Companies">Companies object</param>
        /// <param name="sqlCmd">SqlCommand object</param>
        /// <returns>compID of deleted row(s)</returns>
        public static Int32 Delete(CompaniesList objCompaniesList, SqlCommand sqlCmd)
        {
            try
            {
                foreach (Companies item in objCompaniesList) { Delete(item, sqlCmd); }
                return objCompaniesList.Count;
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// Accepts Companies object, attach all properties as SP parameters
        /// executes Companies_Select stores procedure and 
        /// return filtered row(s) from Companies table
        /// </summary>
        /// <param name="objCompanies">Companies to filter the results</param>
        /// <returns>Filtered row(s) from Companies table</returns>
        public static DataSet Select(Companies objCompanies)
        {
            SqlCommand sqlCommand = new SqlCommand(Companies_Select, SqlHelper.DefaultSqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand = AttachParameters(sqlCommand, objCompanies, SqlOperation.Select);

            try
            {
                DataSet Ds1 = SqlHelper.ExecuteDataset(sqlCommand);

                if (Ds1.Tables.Count < 1)
                    throw new Exception("There was an error executing SqlCommand. No data table was returned.");

                return Ds1;
            }
            catch (Exception ex) { throw ex; }
        }

        #endregion

        #region ----- Helper Methods --------

        /// <summary>
        /// Prepare SqlCommand parameters for Insert, update and delete operation
        /// </summary>
        /// <param name="sqlCommand">sqlCommand object</param>
        /// <param name="Companies">Companies object</param>
        /// <param name="sqlOperation">Operation perform(Insert / Select / Update / Delete) </param>
        /// <returns>sqlCommand object</returns>
        private static SqlCommand AttachParameters(SqlCommand sqlCommand, Companies objCompanies, SqlOperation sqlOperation)
        {
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@compID", objCompanies.compID);
            sqlCommand.Parameters.AddWithValue("@compName", objCompanies.compName);
            sqlCommand.Parameters.AddWithValue("@compDescr", objCompanies.compDescr);
            sqlCommand.Parameters.AddWithValue("@active", objCompanies.active);
            sqlCommand.Parameters.AddWithValue("@crtBy", objCompanies.crtBy);
            sqlCommand.Parameters.AddWithValue("@crtDate", objCompanies.crtDate);
            sqlCommand.Parameters.AddWithValue("@modBy", objCompanies.modBy);
            sqlCommand.Parameters.AddWithValue("@modDate", objCompanies.modDate);

            if (sqlOperation == SqlOperation.Insert)
            {
                sqlCommand.Parameters["@compID"].Direction = ParameterDirection.ReturnValue;
            }

            if (sqlOperation == SqlOperation.Delete)
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.AddWithValue("@compID", objCompanies.compID);
            }

            DBContext.AttachParameters(sqlCommand, objCompanies, sqlOperation);
            return sqlCommand;
        }

        #endregion
    }
}