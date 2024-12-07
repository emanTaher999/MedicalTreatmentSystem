using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MedicalTreatment.Models;
using PagedList;
using System.Data.Entity.Validation;

//namespace MedicalTreatment.Controllers
//{
//    public class MedicalDetailServiceController : Controller
//    {
//        private Entities db = new Entities();

        //
        // GET: /MedicalDetailService/


namespace MedicalTreatment.Controllers
{
    public class MedicalDetailServiceController : Controller
    {
        private Entities db = new Entities();

        //
        // GET: /MedicalDetailService/

        public ActionResult Index(string search, int? x)
        {
            var list = db.MEDICALDETAILSERVICES.Include(m=>m.MEDICALSERVICE).Where(i => i.STATUS == "Active" && i.NAME.StartsWith(search) || search == null).ToList().ToPagedList(x ?? 1, 10);
            return View(list);

            //var medicaldetailservices = db.MEDICALDETAILSERVICES.Include(m => m.MEDICALSERVICE);
            //return View(medicaldetailservices.ToList());
            //var list = db.ORGANIZATIONPROFILEs.Where(i => i.STATUS == "Active" && i.ADDRESS.StartsWith(search) || search == null).ToList().ToPagedList(x ?? 1, 10);
            //return View(list);
        }

        //
        // GET: /MedicalDetailService/Details/5

        public ActionResult Details(int id = 0)
        {
            MEDICALDETAILSERVICE medicaldetailservice = db.MEDICALDETAILSERVICES.Find(id);
            if (medicaldetailservice == null)
            {
                return HttpNotFound();
            }
            return View(medicaldetailservice);
        }

        //
        // GET: /MedicalDetailService/Create

        public ActionResult Create()
        {
            ViewBag.MEDICALSERVICEID = new SelectList(db.MEDICALSERVICES, "ID", "NAME");
            return View();
        }

        //
        // POST: /MedicalDetailService/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MEDICALDETAILSERVICE medicaldetailservice)
        {
            //if (ModelState.IsValid)
            //{
            try
            {
                int id = 0;
                if (db.MEDICALCLAIMFORMs.Any())
                    id = db.MEDICALDETAILSERVICES.Max(i => i.ID);
                id += 1;
                medicaldetailservice.ID = id;
                medicaldetailservice.STATUS = "Active";
                medicaldetailservice.LASTUPDATED = System.DateTime.Now;
                db.MEDICALDETAILSERVICES.Add(medicaldetailservice);
                db.SaveChanges();
                TempData["AlertMessage"] = "success";
                return RedirectToAction("Index");
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;

            }

            ViewBag.MEDICALSERVICEID = new SelectList(db.MEDICALSERVICES, "ID", "NAME", medicaldetailservice.MEDICALSERVICEID);
            return View(medicaldetailservice);
        }

        //
        // GET: /MedicalDetailService/Edit/5

        public ActionResult Edit(int id = 0)
        {
            MEDICALDETAILSERVICE medicaldetailservice = db.MEDICALDETAILSERVICES.Find(id);
            if (medicaldetailservice == null)
            {
                return HttpNotFound();
            }
            ViewBag.MEDICALSERVICEID = new SelectList(db.MEDICALSERVICES, "ID", "NAME", medicaldetailservice.MEDICALSERVICEID);
            return View(medicaldetailservice);
        }

        //
        // POST: /MedicalDetailService/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MEDICALDETAILSERVICE medicaldetailservice)
        {
            if (ModelState.IsValid)
            {
                medicaldetailservice.STATUS = "Active";
                medicaldetailservice.LASTUPDATED = DateTime.Now;
                db.Entry(medicaldetailservice).State = EntityState.Modified;
                db.SaveChanges();
                TempData["AlertMessage"] = "edit";
                return RedirectToAction("Index");
            }
            ViewBag.MEDICALSERVICEID = new SelectList(db.MEDICALSERVICES, "ID", "NAME", medicaldetailservice.MEDICALSERVICEID);
            return View(medicaldetailservice);
        }

        //
        // GET: /MedicalDetailService/Delete/5

        public ActionResult Delete(int id = 0)
        {
            MEDICALDETAILSERVICE medicaldetailservice = db.MEDICALDETAILSERVICES.Find(id);
            if (medicaldetailservice == null)
            {
                return HttpNotFound();
            }
            ViewBag.MEDICALSERVICEID = new SelectList(db.MEDICALSERVICES, "ID", "NAME", medicaldetailservice.MEDICALSERVICEID);
            return View(medicaldetailservice);
        }

        //
        // POST: /MedicalDetailService/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MEDICALDETAILSERVICE medicaldetailservice = db.MEDICALDETAILSERVICES.Find(id);
            db.MEDICALDETAILSERVICES.Remove(medicaldetailservice);
            db.SaveChanges();
            TempData["AlertMessage"] = "deleted";
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}