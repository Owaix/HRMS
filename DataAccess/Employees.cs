using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;

namespace DataAccess
{
    /// <summary>
    /// Employees Entity Model
    /// </summary>
    [DataContract]
    public class Employees : Entity, IEntity
    {
        #region ----- Properties -----

        [PrimaryKey]
        [DataMember]
        public Int32 empID { get; set; } = 0;
        [DataMember]
        public Int32 xEmpID { get; set; } = 0;

        [DataMember]
        public Int32 compID { get; set; } = 0;

        [DataMember]
        public Int32 locID { get; set; } = 0;

        [DataMember]
        public Int32 locTypeID { get; set; } = 0;

        [DataMember]
        public String loctName { get; set; } = "";

        [DataMember]
        public String locName { get; set; } = "";

        [DataMember]
        public String firstName { get; set; } = "";

        [DataMember]
        public String EmpName { get; set; } = "";

        [IgnoreDataMember]
        public String fullName { get { return firstName + " " + lastName; } }

        [DataMember]
        public String compName { get; set; } = "";

        [DataMember]
        public String EmpTName { get; set; } = "";

        [DataMember]
        public String lastName { get; set; } = "";

        [DataMember]
        public String email { get; set; } = "";

        [DataMember]
        public String address { get; set; } = "";

        [DataMember]
        public String city { get; set; } = "";

        [DataMember]
        public String state { get; set; } = "";

        [DataMember]
        public String zip { get; set; } = "";

        [DataMember]
        public String cell { get; set; } = "";

        [DataMember]
        public String imgURL { get; set; } = "";

        [DataMember]
        public DateTime dtJoined { get; set; } = Convert.ToDateTime("1/1/1900");

        [IgnoreDataMember]
        public Int32 tenure
        {
            get
            {
                return (DateTime.Now.Year - dtJoined.Year) * 12 + DateTime.Now.Month - dtJoined.Month; ;
            }
        }

        [DataMember]
        public DateTime dtLeft { get; set; } = Convert.ToDateTime("1/1/1900");

        [DataMember]
        public Int32 weekHours { get; set; } = 0;

        [DataMember]
        public Int32 empRating { get; set; } = 0;

        [DataMember]
        public Boolean active { get; set; } = true;

        [DataMember]
        public Boolean isApproved { get; set; } = true;

        [DataMember]
        public Boolean isCompleted { get; set; } = true;

        [DataMember]
        public Boolean isFullDay { get; set; } = false;

        [DataMember]
        public String crtBy { get; set; } = "";

        [DataMember]
        public DateTime crtDate { get; set; } = Convert.ToDateTime("1/1/1900");

        [DataMember]
        public String modBy { get; set; } = "";

        [DataMember]
        public DateTime modDate { get; set; } = Convert.ToDateTime("1/1/1900");

        [DataMember]
        public String SearchString { get; set; } = "";


        [DataMember]
        public DateTime dtFrom { get; set; } = Convert.ToDateTime("1/1/1900");

        [DataMember]
        public DateTime dtTo { get; set; } = Convert.ToDateTime("1/1/1900");

        [DataMember]
        public Int32 roleID { get; set; } = 0;

        public Int32 empRoleID { get; set; } = 0;

        [DataMember]
        public String roleName { get; set; } = "";

        [DataMember]
        public String roleType { get; set; } = "";

        [DataMember]
        public Int32 empMinHrs { get; set; } = 0;

        [DataMember]
        public Int32 empMaxHrs { get; set; } = 0;

        public Int32 parentLocID { get; set; } = 0;

        public Int32 SMEmpID { get; set; } = 0;

        public Int32 DMEmpID { get; set; } = 0;

        public Int32 customLocID { get; set; } = 0;


        [IgnoreDataMember]
        public EmpHoursList objEmpHoursList { get; set; } = new EmpHoursList();

        [IgnoreDataMember]
        public EmpNotificationsList objEmpNotificationsList { get; set; } = new EmpNotificationsList();

        [IgnoreDataMember]
        public EmpRolesList empRolesList { get; set; } = new EmpRolesList();


        [IgnoreDataMember]
        public EmpRolesList DefaultEmpRolesList { get; set; } = new EmpRolesList();

        public Int32 parentRoleID { get; set; } = 0;

        #endregion

        #region ----- Construct ------

        /// <summary>
        /// Default constructor that takes no arguments
        /// </summary>
        /// <returns>Employees object</returns>
        public Employees() { }

        /// <summary>
        /// Overload construct that takes one row from Employees table 
        /// and convert it into Employees object
        /// </summary>
        /// <param name="EmployeesDataRow">DataRow one row from Employees table</param>
        /// <returns>Employees object</returns>
        public Employees(DataRow Row)
        {
            try
            {
                foreach (var item in GetType().GetProperties().ToList().FindAll(x => x.CanWrite))
                {
                    try
                    {
                        if (Row != null && Row.Table.Columns.Contains(item.Name) && !string.IsNullOrEmpty(Row[item.Name].ToString()))
                        { item.SetValue(this, Convert.ChangeType(Row[item.Name], item.PropertyType)); }
                    }
                    catch (Exception) { }
                }

                //if (ToString() == new Employees().ToString()) { throw new Exception("Unable to process while provided table was not in expected format."); }
            }
            catch (Exception ex) { throw ex; }
        }
        #endregion

        #region ----- Finalize -------

        ~Employees() { Dispose(false); }

        #endregion

        #region ----- Helper Methods -

        /// <summary>
        /// Helper method that takes Primary Key and set all fields with value
        /// </summary>
        /// <param name="PK">int PrimaryKey of Employees</param>  
        /// <returns>Employees object</returns>
        public void FromPrimaryKey(int PK)
        {
            if (PK < 1) { throw new ArgumentNullException("empID"); }

            Employees obj = new Employees { empID = PK };
            DataSet ds = obj.Select();

            if (ds.Tables[0].Rows.Count > 0) { obj = new Employees(ds.Tables[0].Rows[0]); }

            obj.Clone(this);
        }

        public void FromUserName(string email)
        {
            if (email.Length < 1) { throw new ArgumentNullException("userName"); }

            Employees obj = new Employees { email = email };
            DataSet ds = obj.Select("active=true");

            if (ds.Tables[0].Rows.Count > 0) { obj = new Employees(ds.Tables[0].Rows[0]); }

            obj.Clone(this);
        }

        public DataSet SelectParent(int empID, int locID, bool allParents)
        {
            if (empID < 1) { throw new ArgumentNullException("empID"); }
            return DBContexts.EmployeesContext.SelectParent(empID, locID, allParents);
        }



        public DataSet SelectParentEmailTemplate(int empID, int locID, bool allParents)
        {
            if (empID < 1) { throw new ArgumentNullException("empID"); }
            return DBContexts.EmployeesContext.SelectParentEmailTemplate(empID, locID, allParents);
        }



        public DataSet SelectDistrict(int compID, int locID)
        {
            if (compID < 1 || locID < 1) { throw new ArgumentNullException("compID"); }
            return DBContexts.EmployeesContext.SelectDistrict(compID, locID);
        }

        public DataSet SelectShiftEligible(int empID, int locID, DateTime dtFrom, DateTime dtTo)
        {

            if (empID < 1 || locID < 1) { throw new ArgumentNullException("empID"); }
            return DBContexts.EmployeesContext.SelectShiftEligible(empID, locID, dtFrom, dtTo);
        }

        public DataSet SelectPublishedSchedule(int compID, int locID, DateTime dtSchedule)
        {
            if (compID < 1) { throw new ArgumentNullException("compID=" + compID.ToString()); }
            if (locID < 1) { throw new ArgumentNullException("locID=" + locID.ToString()); }
            if (dtSchedule < DateTime.Today.AddDays(-7)) { throw new ArgumentNullException("dtSchedule=" + dtSchedule.ToShortDateString()); }
            return DBContexts.EmployeesContext.SelectPublishedSchedule(compID, locID, dtSchedule);
        }

        public DataSet SelectCustomEmployees(Employees obj)
        {
            return DBContexts.EmployeesContext.SelectCustomEmployees(obj);
        }

        public void SelectEmpRolesList()
        {
            if (empID > 0)
                empRolesList = new EmpRolesList(new EmpRoles { empID = empID }.Select());
            if (active == false)
                DefaultEmpRolesList = Com.LT.Scheduler.Controllers.SchedulerController.GetEmpDefaultRole(empID);
            if (DefaultEmpRolesList != null && DefaultEmpRolesList.Count > 0)
            {
                roleID = DefaultEmpRolesList[0].roleID;
                locID = DefaultEmpRolesList[0].locID;
                empRoleID = DefaultEmpRolesList[0].empRoleID;
            }
        }

        public void SelectEmpHoursList()
        {
            if (empID > 0)
            {
                objEmpHoursList = new EmpHoursList(new EmpHours() { empID = this.empID }.Select());
            }
        }

        /// <summary>
        /// Helper method that takes Foreign Key and set all fields with value
        /// </summary>
        /// <param name="FK">int ForeignKey of Employees</param>  
        /// <returns>Employees object</returns>
        //public DataSet FromForeignKey(int FK)
        //{
        //    if (FK < 1) { throw new ArgumentNullException("FK < 1"); }

        //    Employees obj = new Employees { FKID = FK };
        //    return obj.Select();
        //}

        /// <summary>
        /// Perform different validation checksome
        /// </summary>
        /// <param name="sqlOperation">SqlOperation mode</param>
        public void Validate(SqlOperation operation)
        {
            switch (operation)
            {
                case SqlOperation.Insert:
                    if (empID > 0)
                        validationResult.Errors.Add("empID should be zero.");
                    break;

                case SqlOperation.Select:
                    validationResult.Errors.Clear();
                    if (empID < 0)
                        validationResult.Errors.Add("empID should be zero or higher.");
                    break;

                case SqlOperation.Update:
                    if (empID < 1)
                        validationResult.Errors.Add("empID should be higher than zero.");
                    break;

                case SqlOperation.Delete:
                    if (empID < 1)
                        validationResult.Errors.Add("empID should be higher than zero.");
                    break;

                default:
                    break;
            }
            if (validationResult.HasError) { throw new InvalidOperationException(validationResult.Errors.Count.ToString() + " errors occured. " + validationResult.ErrorSummary); }
        }

        /// <summary>
        /// Convert current Employees object into string and return
        /// </summary>
        /// <returns>string source of current Employees object</returns>
        public override string ToString()
        {
            string Mystring = "Employees Information\r\n";
            Mystring += $"empID={empID.ToString()}\r\n";
            Mystring += $"xEmpID={xEmpID.ToString()}\r\n";
            Mystring += $"compID={compID.ToString()}\r\n";
            Mystring += $"firstName={firstName.ToString()}\r\n";
            Mystring += $"lastName={lastName.ToString()}\r\n";
            Mystring += $"email={email.ToString()}\r\n";
            Mystring += $"address={address.ToString()}\r\n";
            Mystring += $"city={city.ToString()}\r\n";
            Mystring += $"state={state.ToString()}\r\n";
            Mystring += $"zip={zip.ToString()}\r\n";
            Mystring += $"cell={cell.ToString()}\r\n";
            Mystring += $"imgURL={imgURL.ToString()}\r\n";
            Mystring += $"dtJoined={dtJoined.ToString()}\r\n";
            Mystring += $"dtLeft={dtLeft.ToString()}\r\n";
            Mystring += $"weekHours={weekHours.ToString()}\r\n";
            Mystring += $"empRating={empRating.ToString()}\r\n";
            Mystring += $"active={active.ToString()}\r\n";
            Mystring += $"crtBy={crtBy.ToString()}\r\n";
            Mystring += $"crtDate={crtDate.ToString()}\r\n";
            Mystring += $"modBy={modBy.ToString()}\r\n";
            Mystring += $"modDate={modDate.ToString()}\r\n";
            return Mystring;
        }

        /// <summary>
        /// Convert current Employees object into Html source and return
        /// </summary>
        /// <returns>Html source of current Employees object</returns>
        public string ToHTML()
        {
            string Htmlstring = "<div class='divObjectWrapper'>" + Environment.NewLine;
            Htmlstring += "<div class='divTitle'>Employees Details</div>" + Environment.NewLine;
            Htmlstring += "<div class='divContent'>" + Environment.NewLine;
            Htmlstring += $"<div class='divRow'> <div class='divCellLabel'>empID</div><div class='divCellValue'>{empID.ToString()}</div></div>" + Environment.NewLine;
            Htmlstring += $"<div class='divRow'> <div class='divCellLabel'>xEmpID</div><div class='divCellValue'>{xEmpID.ToString()}</div></div>" + Environment.NewLine;
            Htmlstring += $"<div class='divRow'> <div class='divCellLabel'>compID</div><div class='divCellValue'>{compID.ToString()}</div></div>" + Environment.NewLine;
            Htmlstring += $"<div class='divRow'> <div class='divCellLabel'>firstName</div><div class='divCellValue'>{firstName.ToString()}</div></div>" + Environment.NewLine;
            Htmlstring += $"<div class='divRow'> <div class='divCellLabel'>lastName</div><div class='divCellValue'>{lastName.ToString()}</div></div>" + Environment.NewLine;
            Htmlstring += $"<div class='divRow'> <div class='divCellLabel'>email</div><div class='divCellValue'>{email.ToString()}</div></div>" + Environment.NewLine;
            Htmlstring += $"<div class='divRow'> <div class='divCellLabel'>address</div><div class='divCellValue'>{address.ToString()}</div></div>" + Environment.NewLine;
            Htmlstring += $"<div class='divRow'> <div class='divCellLabel'>city</div><div class='divCellValue'>{city.ToString()}</div></div>" + Environment.NewLine;
            Htmlstring += $"<div class='divRow'> <div class='divCellLabel'>state</div><div class='divCellValue'>{state.ToString()}</div></div>" + Environment.NewLine;
            Htmlstring += $"<div class='divRow'> <div class='divCellLabel'>zip</div><div class='divCellValue'>{zip.ToString()}</div></div>" + Environment.NewLine;
            Htmlstring += $"<div class='divRow'> <div class='divCellLabel'>cell</div><div class='divCellValue'>{cell.ToString()}</div></div>" + Environment.NewLine;
            Htmlstring += $"<div class='divRow'> <div class='divCellLabel'>imgURL</div><div class='divCellValue'>{imgURL.ToString()}</div></div>" + Environment.NewLine;
            Htmlstring += $"<div class='divRow'> <div class='divCellLabel'>dtJoined</div><div class='divCellValue'>{dtJoined.ToString()}</div></div>" + Environment.NewLine;
            Htmlstring += $"<div class='divRow'> <div class='divCellLabel'>dtLeft</div><div class='divCellValue'>{dtLeft.ToString()}</div></div>" + Environment.NewLine;
            Htmlstring += $"<div class='divRow'> <div class='divCellLabel'>weekHours</div><div class='divCellValue'>{weekHours.ToString()}</div></div>" + Environment.NewLine;
            Htmlstring += $"<div class='divRow'> <div class='divCellLabel'>empRating</div><div class='divCellValue'>{empRating.ToString()}</div></div>" + Environment.NewLine;
            Htmlstring += $"<div class='divRow'> <div class='divCellLabel'>active</div><div class='divCellValue'>{active.ToString()}</div></div>" + Environment.NewLine;
            Htmlstring += $"<div class='divRow'> <div class='divCellLabel'>crtBy</div><div class='divCellValue'>{crtBy.ToString()}</div></div>" + Environment.NewLine;
            Htmlstring += $"<div class='divRow'> <div class='divCellLabel'>crtDate</div><div class='divCellValue'>{crtDate.ToString()}</div></div>" + Environment.NewLine;
            Htmlstring += $"<div class='divRow'> <div class='divCellLabel'>modBy</div><div class='divCellValue'>{modBy.ToString()}</div></div>" + Environment.NewLine;
            Htmlstring += $"<div class='divRow'> <div class='divCellLabel'>modDate</div><div class='divCellValue'>{modDate.ToString()}</div></div>" + Environment.NewLine;
            Htmlstring += "</div>" + Environment.NewLine;
            Htmlstring += "</div>" + Environment.NewLine;
            return Htmlstring;
        }

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            ///ToDo: Dispose off properties here, if needed
            Dispose(true);
        }

        #endregion
    }

    /// <summary>
    /// Generic list class of Employees object
    /// </summary>
    [CollectionDataContract(ItemName = "Employees", Name = "EmployeesList")]
    public class EmployeesList : List<Employees>, IDisposable
    {
        #region ----- Construct -----

        /// <summary>
        /// Default constructor that takes no arguments and initialize all fields with default value
        /// 
        /// returns new EmployeesList object
        /// </summary>
        /// <returns>Employees object</returns>
        public EmployeesList()
        {
        }

        /// <summary>
        /// Overloaded constructor that takes Employees object and initialize List(Employees) object
        /// 
        /// returns EmployeesList object with one list item
        /// </summary>
        /// <param name="objEmployees">Employees object</param>
        /// <returns>EmployeesList object</returns>
        public EmployeesList(Employees obj)
        {
            Add(obj);
        }

        /// <summary>
        /// Overloaded constructor that takes Employees Enumeration and initialize List(Employees) object
        /// 
        /// returns EmployeesList object with one list item
        /// </summary>
        /// <param name="objEmployees">Employees object</param>
        /// <returns>EmployeesList object</returns>
        public EmployeesList(IEnumerable<Employees> obj)
        {
            AddRange(obj);
        }

        /// <summary>
        /// Overloaded constructor that accpets DataSet containing Employees table and convert it into List of Employees object
        /// 
        /// returns EmployeesList object with all list items from provided DataTable
        /// </summary>
        /// <param name="EmployeesDataSet">DataSet containing Employees table</param>
        /// <returns>EmployeesList object</returns>
        public EmployeesList(DataSet ds)
        {
            try { AddRange(new EmployeesList(ds.Tables[0])); }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// Overloaded constructor that accpets Employees table and convert it into List of Employees object
        /// 
        /// returns EmployeesList object with all list items from provided DataTable
        /// </summary>
        /// <param name="EmployeesDataTable">DataTable of Employees</param>
        /// <returns>EmployeesList object</returns>
        public EmployeesList(DataTable dt)
        {
            try
            {
                foreach (DataRow row in dt.Rows)
                {
                    Add(new Employees(row));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Overloaded constructor that return all rows from Employees table
        /// </summary>
        /// <param name="SelectAll">true if you want to get complete list of Employees</param>
        /// <returns>EmployeesList object</returns>
        public EmployeesList(Boolean SelectAll)
        {
            if (SelectAll)
            {
                try { AddRange(new EmployeesList(new Employees().Select())); }
                catch (Exception ex) { throw ex; }
            }
        }

        #endregion

        #region ----- Destruct ------

        ~EmployeesList() { }

        #endregion

        #region ----- Helper Methods-

        public void FillEmpRoles()
        {
            this.ForEach(x => { x.SelectEmpRolesList(); });
        }

        public void FillEmpRolesListFull()
        {

            EmpRolesList empRoles = new EmpRolesList(new EmpRoles() { compID = new Sessions().companyID, pageSize = int.MaxValue }.Select());
            this.ForEach(x =>
            {
                if (x.empID > 0)
                    x.empRolesList = new EmpRolesList(empRoles.FindAll(r => r.empID == x.empID));
                if (x.active == false)
                    x.DefaultEmpRolesList = Com.LT.Scheduler.Controllers.SchedulerController.GetEmpDefaultRole(x.empID);
                if (x.DefaultEmpRolesList != null && x.DefaultEmpRolesList.Count > 0)
                {
                    x.roleID = x.DefaultEmpRolesList[0].roleID;
                    x.locID = x.DefaultEmpRolesList[0].locID;
                    x.empRoleID = x.DefaultEmpRolesList[0].empRoleID;
                }
            });
        }

        public void FillEmpHoursList()
        {
            this.ForEach(x => x.SelectEmpHoursList());
        }
        public void FillEmpHoursListFull()
        {
            EmpHoursList empHoursList = new EmpHoursList(new EmpHours() { pageSize = int.MaxValue }.Select());
            this.ForEach(x =>
            {
                if (x.empID > 0)
                {
                    x.objEmpHoursList = new EmpHoursList(empHoursList.FindAll(h => h.empID == x.empID));
                }
            });
        }
        public void Dispose()
        {
            Clear();
            ///ToDo: Dispose off properties here, if needed
        }

        #endregion
    }
}