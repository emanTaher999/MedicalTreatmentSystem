using System.Configuration;
using System.Data.Odbc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web.SessionState;
using Accounting.Data;
using System.ComponentModel;
using System.Data.OracleClient;
using MedicalTreatment.Resources;


namespace MedicalTreatment.Controllers
{
    //--------------------------
    public static class CommonUtils
    {
       
        public static string RenderPartialViewToString(Controller controller, string viewName, object model)
        {
            controller.ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);

                var viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
                viewResult.View.Render(viewContext, sw);
                return sw.GetStringBuilder().ToString();
            }
        }


        internal static DataTable Call_GetImage()
        {
            throw new NotImplementedException();
        }

      
        public static DataTable Call_SpEmployeeMedicalExpenses(DateTime? InFromDate, DateTime? InToDate, int? InIndividual)
        {
            OracleConnection con = new OracleConnection("data source=hp-pc:1521/orcl;password=medicaltreatment;user id=MEDICALTREATMENT");
            //using (OracleConnection objConn = new OracleConnection(ConfigurationManager.ConnectionStrings["Entities"].ConnectionString))
            //{
                con.Open();

                OracleTransaction transaction = con.BeginTransaction();

                OracleCommand objCmd = new OracleCommand();
                objCmd.Connection = con;
                objCmd.Transaction = transaction;
                objCmd.CommandText = "SpEmployeeMedicalExpenses";
                objCmd.CommandType = CommandType.StoredProcedure;
                if (InIndividual == null)
                    objCmd.Parameters.Add("InIndividual", OracleType.Number).Value = DBNull.Value;
                else
                    objCmd.Parameters.Add("InIndividual", OracleType.Number).Value = InIndividual;

                if (InFromDate == null)
                    objCmd.Parameters.Add("InFromDate", OracleType.DateTime).Value = DBNull.Value;
                else
                    objCmd.Parameters.Add("InFromDate", OracleType.DateTime).Value = InFromDate;

                if (InToDate == null)
                    objCmd.Parameters.Add("InToDate", OracleType.DateTime).Value = DBNull.Value;
                else
                    objCmd.Parameters.Add("InToDate", OracleType.DateTime).Value = InToDate;

                objCmd.Parameters.Add("ErrorCode", OracleType.Number, 1000).Direction = ParameterDirection.Output;
                objCmd.Parameters.Add("ErrorDescription", OracleType.VarChar, 1000).Direction = ParameterDirection.Output;


                try
                {
                    objCmd.ExecuteNonQuery();

                    var errorDescription = objCmd.Parameters["ErrorDescription"].Value.ToString();
                    var ErrorCode = objCmd.Parameters["ErrorCode"].Value.ToString();
                    string sql =
                string.Format(
                            "Select * from GENERICREPORT");
                    objCmd.CommandType = CommandType.Text;
                    objCmd.CommandText = sql;
                    objCmd.Parameters.Clear();
                    DataTable dt = new DataTable();
                    using (OracleDataReader rdr = objCmd.ExecuteReader())
                    {
                        dt.Load(rdr);

                    }

                    transaction.Commit();
                    return dt;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return null;
                }

            //}
        }


        public static DataTable Call_SpCompanyDepartmentExpenses(DateTime? InFromDate, DateTime? InToDate)
        {
            OracleConnection con = new OracleConnection("data source=hp-pc:1521/orcl;password=medicaltreatment;user id=MEDICALTREATMENT");
            //using (OracleConnection objConn = new OracleConnection(ConfigurationManager.ConnectionStrings["Entities"].ConnectionString))
            //{
            con.Open();

            OracleTransaction transaction = con.BeginTransaction();

                OracleCommand objCmd = new OracleCommand();
                objCmd.Connection = con;
                objCmd.Transaction = transaction;
                objCmd.CommandText = "SpCompanyDepartmentExpenses";
                objCmd.CommandType = CommandType.StoredProcedure;

                if (InFromDate == null)
                    objCmd.Parameters.Add("InFromDate", OracleType.DateTime).Value = DBNull.Value;
                else
                    objCmd.Parameters.Add("InFromDate", OracleType.DateTime).Value = InFromDate;

                if (InToDate == null)
                    objCmd.Parameters.Add("InToDate", OracleType.DateTime).Value = DBNull.Value;
                else
                    objCmd.Parameters.Add("InToDate", OracleType.DateTime).Value = InToDate;

                objCmd.Parameters.Add("ErrorCode", OracleType.Number, 1000).Direction = ParameterDirection.Output;
                objCmd.Parameters.Add("ErrorDescription", OracleType.VarChar, 1000).Direction = ParameterDirection.Output;


                try
                {
                    objCmd.ExecuteNonQuery();

                    var errorDescription = objCmd.Parameters["ErrorDescription"].Value.ToString();
                    var ErrorCode = objCmd.Parameters["ErrorCode"].Value.ToString();
                    string sql =
                string.Format(
                            "Select * from GENERICREPORT");
                    objCmd.CommandType = CommandType.Text;
                    objCmd.CommandText = sql;
                    objCmd.Parameters.Clear();
                    DataTable dt = new DataTable();
                    using (OracleDataReader rdr = objCmd.ExecuteReader())
                    {
                        dt.Load(rdr);

                    }

                    transaction.Commit();
                    return dt;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return null;
                }

            //}
        }
        public static DataTable Call_SpCompareEmpExpenseCeiling(DateTime? InFromDate, DateTime? InToDate)
        {
            OracleConnection con = new OracleConnection("data source=hp-pc:1521/orcl;password=medicaltreatment;user id=MEDICALTREATMENT");
            //using (OracleConnection objConn = new OracleConnection(ConfigurationManager.ConnectionStrings["Entities"].ConnectionString))
            //{
            con.Open();

            OracleTransaction transaction = con.BeginTransaction();

                OracleCommand objCmd = new OracleCommand();
                objCmd.Connection = con;
                objCmd.Transaction = transaction;
                objCmd.CommandText = "SpCompareEmpExpenseCeiling";
                objCmd.CommandType = CommandType.StoredProcedure;

                if (InFromDate == null)
                    objCmd.Parameters.Add("InFromDate", OracleType.DateTime).Value = DBNull.Value;
                else
                    objCmd.Parameters.Add("InFromDate", OracleType.DateTime).Value = InFromDate;

                if (InToDate == null)
                    objCmd.Parameters.Add("InToDate", OracleType.DateTime).Value = DBNull.Value;
                else
                    objCmd.Parameters.Add("InToDate", OracleType.DateTime).Value = InToDate;

                objCmd.Parameters.Add("ErrorCode", OracleType.Number, 1000).Direction = ParameterDirection.Output;
                objCmd.Parameters.Add("ErrorDescription", OracleType.VarChar, 1000).Direction = ParameterDirection.Output;


                try
                {
                    objCmd.ExecuteNonQuery();

                    var errorDescription = objCmd.Parameters["ErrorDescription"].Value.ToString();
                    var ErrorCode = objCmd.Parameters["ErrorCode"].Value.ToString();
                    string sql =
                string.Format(
                            "Select * from GENERICREPORT");
                    objCmd.CommandType = CommandType.Text;
                    objCmd.CommandText = sql;
                    objCmd.Parameters.Clear();
                    DataTable dt = new DataTable();
                    using (OracleDataReader rdr = objCmd.ExecuteReader())
                    {
                        dt.Load(rdr);

                    }

                    transaction.Commit();
                    return dt;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return null;
                }

            //}
        }

        public static DataTable Call_SpMedicalExpensesAnalysis(DateTime? InFromDate, DateTime? InToDate)
        {
            OracleConnection con = new OracleConnection("data source=hp-pc:1521/orcl;password=medicaltreatment;user id=MEDICALTREATMENT");
            //using (OracleConnection objConn = new OracleConnection(ConfigurationManager.ConnectionStrings["Entities"].ConnectionString))
            //{
            con.Open();

            OracleTransaction transaction = con.BeginTransaction();

            OracleCommand objCmd = new OracleCommand();
            objCmd.Connection = con;
            objCmd.Transaction = transaction;
            objCmd.CommandText = "SpMedicalExpensesAnalysis";
            objCmd.CommandType = CommandType.StoredProcedure;

            if (InFromDate == null)
                objCmd.Parameters.Add("InFromDate", OracleType.DateTime).Value = DBNull.Value;
            else
                objCmd.Parameters.Add("InFromDate", OracleType.DateTime).Value = InFromDate;

            if (InToDate == null)
                objCmd.Parameters.Add("InToDate", OracleType.DateTime).Value = DBNull.Value;
            else
                objCmd.Parameters.Add("InToDate", OracleType.DateTime).Value = InToDate;

            objCmd.Parameters.Add("ErrorCode", OracleType.Number, 1000).Direction = ParameterDirection.Output;
            objCmd.Parameters.Add("ErrorDescription", OracleType.VarChar, 1000).Direction = ParameterDirection.Output;


            try
            {
                objCmd.ExecuteNonQuery();

                var errorDescription = objCmd.Parameters["ErrorDescription"].Value.ToString();
                var ErrorCode = objCmd.Parameters["ErrorCode"].Value.ToString();
                string sql =
            string.Format(
                        "Select * from GENERICREPORT");
                objCmd.CommandType = CommandType.Text;
                objCmd.CommandText = sql;
                objCmd.Parameters.Clear();
                DataTable dt = new DataTable();
                using (OracleDataReader rdr = objCmd.ExecuteReader())
                {
                    dt.Load(rdr);

                }

                transaction.Commit();
                return dt;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return null;
            }

            //}
        }
        public static void SetFeedback(string message, string type)
        {
            //Stopwatch stopwatch = new Stopwatch();
            //stopwatch.Start();

            #region Asynchronous

            


            #endregion

            HttpContext.Current.Session["Message"] = message;
            HttpContext.Current.Session["MessageStyle"] = type;
            HttpContext.Current.Session["Type"] = Feedback.ResourceManager.GetString("Feedback_" + type);

            //stopwatch.Stop();
            //Debug.WriteLine($"Elapsed time = {stopwatch.Elapsed}");


        }
    }

}