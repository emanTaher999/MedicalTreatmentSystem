﻿using Accounting.Data;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MedicalTreatment.Controllers
{
    public class rptMedicalExpensesAnalysisController : Controller
    {
        //
        // GET: /rptMedicalExpensesAnalysis/

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetReport(DateTime? InFromDate, DateTime? InToDate)
        {
            DataTable dt = CommonUtils.Call_SpMedicalExpensesAnalysis(InFromDate, InToDate);
            if (dt != null && dt.Rows != null && dt.Rows.Count != 0)
            {
                string strReportName = "rptMedicalExpensesAnalysis";
                using (ReportDocument rd = new ReportDocument())
                {
                    Session["ReportData"] = dt;
                    Session["ReportOption"] = "Reports";
                    Session["reportname"] = strReportName;

                    Tools tool = new Tools();
                    strReportName = Session["reportname"].ToString();
                    string strRptPath = tool.getProgectPath() + strReportName + ".rpt";

                    if (Session["ReportData"] != null)
                    {
                        var ReportData = Session["ReportData"];

                        rd.Load(strRptPath);
                        rd.SetDataSource(ReportData);

                        List<KeyValuePair<string, string>> reportParameters = (List<KeyValuePair<string, string>>)Session["ReportParameter"];
                        if (reportParameters != null)
                        {

                            foreach (var reportParameter in reportParameters)
                            {
                                rd.SetParameterValue("@" + reportParameter.Key, reportParameter.Value);
                            }
                        }
                        rd.SetDatabaseLogon("medicaltreatment", "medicaltreatment");
                        var isWord = Session["ReportType"];
                        if (isWord != null)
                            isWord = isWord.ToString();
                        if (isWord == "word")
                            rd.ExportToHttpResponse(ExportFormatType.WordForWindows, System.Web.HttpContext.Current.Response, false, "Report");
                        else if (isWord == "excel")
                            rd.ExportToHttpResponse(ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, "Report");
                        else
                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Report");
                    }

                    else
                    {
                        Response.Write("<H2>Nothing Found, No Data To Show</H2>");
                    }
                }


                List<KeyValuePair<string, string>> param = new List<KeyValuePair<string, string>>();


                return Json(true, JsonRequestBehavior.AllowGet);

            }

            else
            {
                var mag = "";
                mag = "لا يوجد بيانات لعرضها";
                return Json(mag, JsonRequestBehavior.AllowGet);
            }
        }
    }


}
