using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Common.Extensions
{
    public static class SqlOperation
    {
        public static String Insert { get; set; }
        public static String Delete { get; set; }
        public static String Select { get; set; }
    }

    public static class EntityExtensions
    {
        #region ----- EntityList -----

        public static void SetProperty<T>(this List<T> obj, string propName, object value)
        {
            if (obj == null || propName == null || propName.Length < 1 || value == null || value.ToString().Length < 1) { return; }
            PropertyInfo pi = null;
            foreach (T item in obj)
            {
                try
                {
                    if (item == null) { continue; }
                    if (pi == null) { pi = item.GetType().GetProperty(propName); }
                    if (pi != null && pi.CanWrite) { pi.SetValue(item, Convert.ChangeType(value, pi.PropertyType), null); }
                }
                catch (Exception) { }
            }
        }

        public static string ToJSON<T>(this List<T> obj) => new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(obj);

        public static string ToHtmlTable<T>(this List<T> obj, bool selectAll, string tableAttr = "", string tableStyle = "border-collapse: collapse; font-family:Arial;", string rowStyle = "", string thStyle = "background-color:#006CC2; color:white; padding:10px;", string tdStyle = "border-bottom:solid 1px #aaa; padding: 10px;")
        {
            System.Text.StringBuilder htmlTable = new System.Text.StringBuilder();
            htmlTable.Append($"<table {tableAttr} style=\" { tableStyle } \">");

            foreach (PropertyInfo rowHeader in obj.FirstOrDefault().GetType().GetProperties().Where(prop => selectAll ? true : Attribute.IsDefined(prop, typeof(Com.LT.Scheduler.Utilities.AddToTableAttribute))))
            {
                DisplayNameAttribute displayAttribute = ((DisplayNameAttribute)rowHeader.GetCustomAttribute(typeof(DisplayNameAttribute), true));
                htmlTable.Append($"<th style=\"{ thStyle}\">{(displayAttribute == null ? rowHeader.Name : displayAttribute.DisplayName)}</th>");
            }

            foreach (T row in obj)
            {
                htmlTable.Append($"<tr style=\" { rowStyle } \">");
                foreach (PropertyInfo td in row.GetType().GetProperties().Where(prop => selectAll ? true : Attribute.IsDefined(prop, typeof(Com.LT.Scheduler.Utilities.AddToTableAttribute))))
                {
                    htmlTable.Append($"<td style=\"{ tdStyle}\">{td.GetValue(row, null).ToString()}</td>");
                }
                htmlTable.Append("</tr>");
            }
            htmlTable.Append("</table>");
            return htmlTable.ToString();
        }


        public static string ToEmailTable<T>(this List<T> obj, bool selectAll, string headerDetails, string tableAttr = "", string tableStyle = "font-family:Arial;text-align:center;border:solid 2px #aaa;", string rowStyle = "background-color:lightgray;", string thStyle = "background-color:#006cc2;color:white;padding:20px;font-size:18px;", string tdStyle = "border-bottom:solid 1px #aaa; padding: 10px;")
        {
            int counter = 0;
            System.Text.StringBuilder htmlTable = new System.Text.StringBuilder();
            htmlTable.Append($"<table {tableAttr} style=\" { tableStyle } \" border=\"1\" cellspacing=\"0\">");
            htmlTable.Append($"<tbody><tr><th style=\" { thStyle } \" colspan = \"3\" > {headerDetails} </th ></tr > ");
            //htmlTable.Append($"<th colspan=\"3\" style=\"{ thStyle}\">{(displayAttribute == null ? rowHeader.Name : displayAttribute.DisplayName)}</th>");
            htmlTable.Append("<tr>");
            foreach (PropertyInfo rowHeader in obj.FirstOrDefault().GetType().GetProperties().Where(prop => selectAll ? true : Attribute.IsDefined(prop, typeof(Com.LT.Scheduler.Utilities.AddToTableAttribute))))
            {
                counter++;
                DisplayNameAttribute displayAttribute = ((DisplayNameAttribute)rowHeader.GetCustomAttribute(typeof(DisplayNameAttribute), true));
                if (counter == 1)
                {
                    htmlTable.Append($"<th  style=\"border-right:0px;color:black;padding:20px;\">{(displayAttribute == null ? rowHeader.Name : displayAttribute.DisplayName)}</th>");
                }
                if (counter == 2)
                {
                    htmlTable.Append($"<th  style=\"border-left:0px;border-right:0px;color:black;padding:10px;\">{(displayAttribute == null ? rowHeader.Name : displayAttribute.DisplayName)}</th>");
                }
                if (counter == 3)
                {
                    htmlTable.Append($"<th  style=\"border-left:0px;color:black;padding:10px;\">{(displayAttribute == null ? rowHeader.Name : displayAttribute.DisplayName)}</th>");
                }
            }
            htmlTable.Append("</tr>");
            foreach (T row in obj)
            {
                htmlTable.Append($"<tr style=\" { rowStyle } \">");
                foreach (PropertyInfo td in row.GetType().GetProperties().Where(prop => selectAll ? true : Attribute.IsDefined(prop, typeof(Com.LT.Scheduler.Utilities.AddToTableAttribute))))
                {
                    htmlTable.Append($"<td style=\"{ tdStyle}\">{td.GetValue(row, null).ToString()}</td>");
                }
                htmlTable.Append("</tr>");
            }
            htmlTable.Append("</tbody>");
            htmlTable.Append("</table>");
            return htmlTable.ToString();
        }

        #endregion

        #region ----- Entity CRUD -----

        /// <summary>
        /// Accepts Entity or its inherited object, insert into table
        /// 
        /// return ObjectId of newly inserted row(s)
        /// </summary>
        /// <returns>objectId of newly inserted row(s)</returns>
        public static int Insert(this Entity Model)
        {
            try
            {
                ///----- Referencing ValidateThis method from Model Class
                MethodInfo ValidateModel = Model.GetType().GetMethods().ToList().Find(x => x.Name == "Validate");
                if (ValidateModel == null) { throw new Exception("Unable to find Validate method in entity."); }

                ///----- Invoking ValidateThis too validate whather the Model is ready to be inserted or not?
                ValidateModel.Invoke(Model, new object[] { SqlOperation.Insert });

                string ModelName = Model.GetType().FullName.Split('.').Last();
                string FullControllerName = Model.GetType().FullName.Replace(".Entities.", ".DBContexts.") + "Context";
                string Namespace = FullControllerName.Substring(0, FullControllerName.IndexOf(".DBContexts."));

                ///----- Referencing Controller Class of same object
                System.Runtime.Remoting.ObjectHandle ControllerWrapper = Activator.CreateInstance(Namespace, FullControllerName);
                var Controller = ControllerWrapper.Unwrap();
                ControllerWrapper = null;

                ///----- Referencing Insert method from Controller Class
                MethodInfo InsertController = Controller.GetType().GetMethods().ToList().Find(x => x.Name == "Insert" && x.GetParameters().Count() == 1 && x.GetParameters()[0].Name == "obj" + ModelName);
                if (InsertController == null) { throw new Exception("Unable to find Insert method in Controller class which accepts " + ModelName + " as parameter."); }

                ///----- Invoking Insert and returning the Primary Key of newly inserted object
                return (int)InsertController.Invoke(Controller, new Object[] { Model });
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// Accepts Entity object or its inherited object, update into table
        /// 
        /// return ObjectId of updated row(s)
        /// </summary>
        /// <returns>ObjectId of updated row(s)</returns>
        public static int Update(this Entity Model)
        {
            try
            {
                /////----- Referencing ValidateThis method from Model Class
                //MethodInfo ValidateModel = Model.GetType().GetMethods().ToList().Find(x => x.Name == "Validate");
                //if (ValidateModel == null) { throw new Exception("Unable to find Validate method in Entity."); }

                /////----- Invoking ValidateThis too validate whather the Model is ready to be inserted or not?
                //ValidateModel.Invoke(Model, new object[] { SqlOperation.Update });

                string ModelName = Model.GetType().FullName.Split('.').Last();
                string FullControllerName = Model.GetType().FullName.Replace(".Entities.", ".DBContexts.") + "Context";
                string Namespace = FullControllerName.Substring(0, FullControllerName.IndexOf(".DBContexts."));

                ///----- Referencing Controller Class of same object
                System.Runtime.Remoting.ObjectHandle ControllerWrapper = Activator.CreateInstance(Namespace, FullControllerName);
                var Controller = ControllerWrapper.Unwrap();
                ControllerWrapper = null;

                ///----- Referencing Update method from Controller Class
                MethodInfo updateController = Controller.GetType().GetMethods().ToList().Find(x => x.Name == "Update" && x.GetParameters().Count() == 1 && x.GetParameters()[0].Name == "obj" + ModelName);
                if (updateController == null) { throw new Exception("Unable to find Update method in Controller class which accepts " + ModelName + " as parameter."); }

                ///----- Invoking Update and returning the Primary Key of updated object
                return (int)updateController.Invoke(Controller, new Object[] { Model });
            }
            catch (Exception ex) { throw ex; }
        }

        public static int Save(this Entity Model) => Model.Update();

        /// <summary>
        /// Accepts Entity object or its inherited object, delete row(s) from table
        /// 
        /// return ObjectId of deleted row(s)
        /// </summary>
        /// <returns>ObjectId of deleted row(s)</returns>
        public static int Delete(this Entity Model)
        {
            try
            {
                ///----- Referencing ValidateThis method from Model Class
                MethodInfo ValidateModel = Model.GetType().GetMethods().ToList().Find(x => x.Name == "Validate");
                if (ValidateModel == null) { throw new Exception("Unable to find Validate method in Entity."); }

                ///----- Invoking ValidateThis too validate whather the Model is ready to be inserted or not?
                ValidateModel.Invoke(Model, new object[] { SqlOperation.Delete });

                string ModelName = Model.GetType().FullName.Split('.').Last();
                string FullControllerName = Model.GetType().FullName.Replace(".Entities.", ".DBContexts.") + "Context";
                string Namespace = FullControllerName.Substring(0, FullControllerName.IndexOf(".DBContexts."));

                ///----- Referencing Controller Class of same object
                System.Runtime.Remoting.ObjectHandle ControllerWrapper = Activator.CreateInstance(Namespace, FullControllerName);
                var Controller = ControllerWrapper.Unwrap();
                ControllerWrapper = null;

                ///----- Referencing Delete method from Controller Class
                MethodInfo deleteController = Controller.GetType().GetMethods().ToList().Find(x => x.Name == "Delete" && x.GetParameters().Count() == 1 && x.GetParameters()[0].Name == "obj" + ModelName);
                if (deleteController == null) { throw new Exception("Unable to find Update method in Controller class which accepts " + ModelName + " as parameter."); }

                ///----- Invoking Delete and returning the Primary Key of updated object
                return (int)deleteController.Invoke(Controller, new Object[] { Model });
            }
            catch (Exception ex) { throw ex; }
        }

        ///// <summary>
        ///// Calls Object's Controller's Select method
        ///// if object has no property set, it will return all row(s) from Object table
        ///// if object has/ve property/ies set manually, it will return all row(s) from Object table
        ///// </summary>
        ///// <returns>All row(s) from ACTIONS table</returns>
        public static DataSet Select(this Entity Model)
        {
            try
            {
                ///----- Referencing ValidateThis method from Model Class
                MethodInfo ValidateModel = Model.GetType().GetMethods().ToList().Find(x => x.Name == "Validate");
                if (ValidateModel == null) { throw new Exception("Unable to find Validate method in Entity."); }

                ///----- Invoking ValidateThis too validate whather the Model is ready to be inserted or not?
                ValidateModel.Invoke(Model, new object[] { SqlOperation.Select });

                string ModelName = Model.GetType().FullName.Split('.').Last();
                string FullControllerName = Model.GetType().FullName.Replace(".Entities.", ".DBContexts.") + "Context";
                string Namespace = FullControllerName.Substring(0, FullControllerName.IndexOf(".DBContexts."));

                ///----- Referencing Controller Class of same object
                System.Runtime.Remoting.ObjectHandle ControllerWrapper = Activator.CreateInstance(Namespace, FullControllerName);
                var Controller = ControllerWrapper.Unwrap();
                ControllerWrapper = null;

                ///----- Referencing Delete method from Controller Class
                MethodInfo selectController = Controller.GetType().GetMethods().ToList().Find(x => x.Name == "Select" && x.GetParameters().Count() == 1 && x.GetParameters()[0].Name == "obj" + ModelName);
                if (selectController == null) { throw new Exception("Unable to find Update method in Controller class which accepts " + ModelName + " as parameter."); }

                ///----- Invoking Delete and returning the Primary Key of updated object
                DataSet ds = (DataSet)selectController.Invoke(Controller, new Object[] { Model });
                ds.Tables[0].TableName = ModelName;
                return ds;
            }
            catch (Exception ex) { throw ex; }
        }

        public static DataSet Select(this Entity Model, string FilterString)
        {
            try
            {
                if (string.IsNullOrEmpty(FilterString)) { FilterString = ""; }

                ///----- Invoking Delete and returning the Primary Key of updated object
                DataTable dt = Model.Select().Tables[0];
                DataSet ds = new DataSet();
                dt.DefaultView.RowFilter = FilterString;
                ds.Tables.Add(dt.DefaultView.ToTable());
                dt.Dispose();
                return ds;
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// it will select all data from specific table
        /// convert it into Model Object 
        /// and overwrite current Instance
        /// </summary>
        /// <typeparam name="T">Name of Model Object</typeparam>
        /// <param name="obj"></param>
        public static void Select<T>(this List<T> obj)
        {
            ///----- Remove all objects from list
            obj.Clear();

            ///----- Internal Variables
            string FullModelName = obj.GetType().FullName.Replace("List", "");
            string ModelName = FullModelName.Split('.').Last();
            string AssemblyName = FullModelName.Substring(0, FullModelName.IndexOf(".Entities."));
            System.Runtime.Remoting.ObjectHandle ModelWrapper = Activator.CreateInstance(AssemblyName, FullModelName);
            var Model = ModelWrapper.Unwrap();
            ModelWrapper = null;

            /////----- Gathering method which will select data from DB
            //MethodInfo SelectModel = Model.GetType().GetMethods().ToList().Find(x => x.Name == "Select" && x.GetParameters().Count() == 0);
            //if (SelectModel == null) { return; }

            ///----- Gathering Model's Construct that will parse DataRow into Model Object
            ConstructorInfo cnstrDataRow = Model.GetType().GetConstructors().ToList().Find(x => x.GetParameters().Count() == 1 && x.GetParameters()[0].Name == "Row");
            if (cnstrDataRow == null) { return; }

            ///----- [1].Getting data from DB
            ///----- [2].Parsing into Model Object
            ///----- [3].And adding into collection

            ///----- [1]
            System.Data.DataSet ds = ((Entity)Model).Select();
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (System.Data.DataRow row in ds.Tables[0].Rows)
                {
                    ///----- [2]
                    T newRow = (T)cnstrDataRow.Invoke(new object[] { row });

                    ///----- [3]
                    obj.Add(newRow);
                }
                ds.Dispose();
            }

            ///----- Clearing up Memory
            Model = null;
            ModelName = null;
            FullModelName = null;
        }

        /// <summary>
        /// it will select all data from specific table
        /// with provided filter criteria
        /// convert it into Model Object 
        /// and overwrite current Instance
        /// </summary>
        /// <typeparam name="T">Name of Model Object</typeparam>
        /// <param name="obj"></param>
        /// <param name="FilterString">string for additional filter the results</param>
        public static void Select<T>(this List<T> obj, string FilterString)
        {
            ///----- Remove all objects from list
            obj.Clear();

            ///----- Internal Variables
            string FullModelName = obj.GetType().FullName.Replace("List", "");
            string ModelName = FullModelName.Split('.').Last();
            string AssemblyName = FullModelName.Substring(0, FullModelName.IndexOf(".Entities."));
            System.Runtime.Remoting.ObjectHandle ModelWrapper = Activator.CreateInstance(AssemblyName, FullModelName);
            var Model = ModelWrapper.Unwrap();
            ModelWrapper = null;

            ///----- Gathering Model's Construct that will parse DataRow into Model Object
            ConstructorInfo cnstrDataRow = Model.GetType().GetConstructors().ToList().Find(x => x.GetParameters().Count() == 1 && x.GetParameters()[0].Name == "Row");
            if (cnstrDataRow == null) { return; }

            ///----- [1].Getting data from DB
            ///----- [2].Parsing into Model Object
            ///----- [3].And adding into collection

            ///----- [1]
            System.Data.DataSet ds = ((Entity)Model).Select(FilterString);
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (System.Data.DataRow row in ds.Tables[0].Rows)
                {
                    ///----- [2]
                    T newRow = (T)cnstrDataRow.Invoke(new object[] { row });

                    ///----- [3]
                    obj.Add(newRow);
                }
                ds.Dispose();
            }

            ///----- Clearing up Memory
            Model = null;
            ModelName = null;
            FullModelName = null;
        }

        /// <summary>
        /// it will select all data from specific table
        /// with provided filter criteria
        /// convert it into Model Object 
        /// and overwrite current Instance
        /// </summary>
        /// <typeparam name="T">Name of Model Object</typeparam>
        /// <param name="obj"></param>
        /// <param name="objModel">Model Object to filter the results</param>
        public static void Select<T>(this List<T> obj, Entity objModel)
        {
            ///----- Remove all objects from list
            obj.Clear();

            ///----- Internal Variables
            string FullModelName = obj.GetType().FullName.Replace("List", "");
            string ModelName = FullModelName.Split('.').Last();

            ///----- Validation
            if (FullModelName != objModel.GetType().FullName) { throw new InvalidCastException("Could not convert " + objModel.GetType().FullName + " into object type " + FullModelName); }

            ///----- Gathering Model's Construct that will parse DataRow into Model Object
            ConstructorInfo cnstrDataRow = objModel.GetType().GetConstructors().ToList().Find(x => x.GetParameters().Count() == 1 && x.GetParameters()[0].Name == "Row");
            if (cnstrDataRow == null) { return; }

            ///----- [1].Getting data from DB
            ///----- [2].Parsing into Model Object
            ///----- [3].And adding into collection

            ///----- [1]
            System.Data.DataSet ds = objModel.Select();
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (System.Data.DataRow row in ds.Tables[0].Rows)
                {
                    ///----- [2]
                    T newRow = (T)cnstrDataRow.Invoke(new object[] { row });

                    ///----- [3]
                    obj.Add(newRow);
                }
                ds.Dispose();
            }

            ///----- Clearing up Memory
            ModelName = null;
            FullModelName = null;
        }

        /// <summary>
        /// it will select all data from specific table
        /// with provided filter criteria
        /// convert it into Model Object 
        /// and overwrite current Instance
        /// </summary>
        /// <typeparam name="T">Name of Model Object</typeparam>
        /// <param name="obj"></param>
        /// <param name="objModel">Model Object to filter the results</param>
        /// <param name="FilterString">string for additional filter the results</param>
        public static void Select<T>(this List<T> obj, Entity objModel, string FilterString)
        {
            ///----- Remove all objects from list
            obj.Clear();

            ///----- Internal Variables
            string FullModelName = obj.GetType().FullName.Replace("List", "");
            string ModelName = FullModelName.Split('.').Last();

            ///----- Validation
            if (FullModelName != objModel.GetType().FullName) { throw new InvalidCastException("Could not convert " + objModel.GetType().FullName + " into object type " + FullModelName); }

            ///----- Gathering Model's Construct that will parse DataRow into Model Object
            ConstructorInfo cnstrDataRow = objModel.GetType().GetConstructors().ToList().Find(x => x.GetParameters().Count() == 1 && x.GetParameters()[0].Name == "Row");
            if (cnstrDataRow == null) { return; }

            ///----- [1].Getting data from DB
            ///----- [2].Parsing into Model Object
            ///----- [3].And adding into collection

            ///----- [1]
            System.Data.DataSet ds = objModel.Select(FilterString);
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (System.Data.DataRow row in ds.Tables[0].Rows)
                {
                    ///----- [2]
                    T newRow = (T)cnstrDataRow.Invoke(new object[] { row });

                    ///----- [3]
                    obj.Add(newRow);
                }
                ds.Dispose();
            }

            ///----- Clearing up Memory
            ModelName = null;
            FullModelName = null;
        }

        /// <summary>
        /// it will insert all objects specified by given obj list, into database
        /// </summary>
        /// <typeparam name="T">Name of Model Object</typeparam>
        /// <param name="obj"></param>
        /// <returns>Number of the objects inserted</returns>
        public static int Insert<T>(this List<T> obj)
        {
            return PerformActionList<T>(obj, "Insert");
        }

        /// <summary>
        /// it will update all objects specified in given obj list into database
        /// </summary>
        /// <typeparam name="T">Name of Model Object</typeparam>
        /// <param name="obj"></param>
        /// <returns>Number of the objects updated</returns>
        public static int Update<T>(this List<T> obj)
        {
            return PerformActionList<T>(obj, "Update");
        }

        /// <summary>
        /// it will delete all objects specified in given obj list from database
        /// </summary>
        /// <typeparam name="T">Name of Model Object</typeparam>
        /// <param name="obj"></param>
        /// <returns>Number of the objects deleted</returns>
        public static int Delete<T>(this List<T> obj)
        {
            return PerformActionList<T>(obj, "Delete");
        }

        /// <summary>
        /// it is an helper method for ObjList CURD(Insert, Update, Delete) methods
        /// </summary>
        /// <typeparam name="T">Name of Model Object</typeparam>
        /// <param name="obj"></param>
        /// <param name="ControllerActionName">Name of Controller method to invoke</param>
        /// <returns></returns>
        private static int PerformActionList<T>(List<T> obj, string ControllerActionName)
        {
            try
            {
                string ModelName = obj[0].GetType().FullName.Split('.').Last();
                string FullControllerName = obj[0].GetType().FullName.Replace(".Entities.", ".DBContexts.") + "Context";
                string Namespace = FullControllerName.Substring(0, FullControllerName.IndexOf(".DBContexts."));

                if (obj.Count < 1)
                { throw new Exception("Cannot perform operation while no valid " + ModelName + " data provided. Please check provided " + ModelName + "List object and try again."); }

                ///----- Referencing Controller Class of same object
                System.Runtime.Remoting.ObjectHandle ControllerWrapper = Activator.CreateInstance(Namespace, FullControllerName);
                var Controller = ControllerWrapper.Unwrap();
                ControllerWrapper = null;

                ///----- Referencing Insert method from Controller Class
                MethodInfo InsertController = Controller.GetType().GetMethods().ToList().Find(x => x.Name == ControllerActionName && x.GetParameters().Count() == 1 && x.GetParameters()[0].Name == "obj" + ModelName + "List");
                if (InsertController == null) { throw new Exception("Unable to find " + ControllerActionName + " method in Controller class which accepts " + ModelName + " as parameter."); }

                ///----- Invoking Insert and returning the Primary Key of newly inserted object
                return (int)InsertController.Invoke(Controller, new Object[] { obj });
            }
            catch (Exception ex) { throw ex; }
        }
        #endregion
    }
    public class Entity
    {
        public static object SqlOperation { get; set; }
    }
}
