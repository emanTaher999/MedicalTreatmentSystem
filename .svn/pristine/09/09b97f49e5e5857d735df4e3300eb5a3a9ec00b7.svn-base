using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using PagedList;
using System.Web.Mvc;
using MedicalTreatment.Models;
using System.Data.Entity.Validation;

namespace MedicalTreatment.Controllers
{
    public class rptEmployeeMedicalExpenses : Controller
    {
        private Entities db = new Entities();

        //
        // GET: /Suppliers/

        public ActionResult Index(string search, int? x)
        {
            ViewBag.INDIVIDUALS = new SelectList(db.INDIVIDUALS.Where(a=>a.STATUS=="Active" || a.STATUS=="Terminated"), "ID", "NAME");
            return View();
        }

        //
        // GET: /Suppliers/Details/5

        public ActionResult Details(int id = 0)
        {
            SUPPLIER supplier = db.SUPPLIERS.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        public JsonResult GetData(string Export, DateTime InFromDate, DateTime InToDate, int INDIVIDUALS)
        {
            Session["ReportType"] = Export;
            var data =CommonUtils.Call_SpEmployeeMedicalExpenses(InFromDate, InToDate, INDIVIDUALS);
            if (data.Rows.Count > 0)
            {
              
                var paremeters = new List<KeyValuePair<string, string>>();
              

                Session["ReportParameter"] = paremeters;
                Session["ReportData"] = data;
                Session["ReportType"] = Export;
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }


        }


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}