using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MedicalTreatment.Models;
using PagedList;

namespace MedicalTreatment.Controllers
{
    public class MedicalServicesController : Controller
    {
        private Entities db = new Entities();

        //
        // GET: /MedicalServices/

        public ActionResult Index(string search, int? x)
        
        {
            //return View(db.MEDICALSERVICES.ToList());
            var list = db.MEDICALSERVICES.Where(i => i.STATUS == "Active" && i.NAME.StartsWith(search) || search == null).ToList().ToPagedList(x ?? 1, 10);
            return View(list);

        }

        //
        // GET: /MedicalServices/Details/5

        public ActionResult Details(int id = 0)
        {
            MEDICALSERVICE medicalservice = db.MEDICALSERVICES.Find(id);
            if (medicalservice == null)
            {
                return HttpNotFound();
            }
            return View(medicalservice);
        }

        //
        // GET: /MedicalServices/Create

        public ActionResult Create()
        {
          //  return View();
            ViewBag.MEDICALSERVICE = new SelectList(db.MEDICALSERVICES, "ID", "NAME");
            return View();
        }

        //
        // POST: /MedicalServices/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MEDICALSERVICE medicalservice)
        {
            try
            {
                medicalservice.STATUS = "Active";
                medicalservice.LASTUPDATED = System.DateTime.Now;
                db.MEDICALSERVICES.Add(medicalservice);
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

            ViewBag.MEDICALSERVICE = new SelectList(db.MEDICALSERVICES, "ID", "NAME");

            return View(medicalservice);
        }

        //
        // GET: /MedicalServices/Edit/5

        public ActionResult Edit(int id = 0)
        {
            MEDICALSERVICE medicalservice = db.MEDICALSERVICES.Find(id);
            if (medicalservice == null)
            {
                return HttpNotFound();
            }
            ViewBag.MEDICALSERVICE = new SelectList(db.MEDICALSERVICES, "ID", "NAME",medicalservice.MEDICALDETAILSERVICES);
            //medicalservice.MEDICALDETAILSERVICES
            return View(medicalservice);
        }

        //
        // POST: /MedicalServices/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MEDICALSERVICE medicalservice)
        {

            try
            {
                db.Entry(medicalservice).State = EntityState.Modified;
                db.SaveChanges();
                TempData["AlertMessage"] = "edit";
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
            ViewBag.MEDICALSERVICE = new SelectList(db.MEDICALSERVICES, "ID", "NAME", medicalservice.MEDICALDETAILSERVICES);

            return View(medicalservice);
        }

            //
            // GET: /MedicalServices/Delete/5

            public ActionResult Delete(short id = 0)
        {
            MEDICALSERVICE medicalservice = db.MEDICALSERVICES.Find(id);
            if (medicalservice == null)
            {
                return HttpNotFound();
            }
            return View(medicalservice);
        }

        //
        // POST: /MedicalServices/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MEDICALSERVICE medicalservice = db.MEDICALSERVICES.Find(id);
            db.MEDICALSERVICES.Remove(medicalservice);
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