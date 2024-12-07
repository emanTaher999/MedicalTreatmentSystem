using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MedicalTreatment.Models;
using PagedList;
using PagedList.Mvc;
using System.Data.Entity.Validation;

namespace MedicalTreatment.Controllers
{
    public class MedicalClaimFormDetailController : Controller
    {
        private Entities db = new Entities();

        //
        // GET: /MedicalClaimFormDetail/

        public ActionResult Index(string search, int? x)
        {
            var list = db.MEDICALCLAIMFORMDETAILS.Where(i=>i.MEDICALCLAIMFORM.PATIENTEMPLOYEE== search || search == null).ToList().ToPagedList(x ?? 1, 10);

           // var medicalclaimformdetails = db.MEDICALCLAIMFORMDETAILS.Include(m => m.MEDICALCLAIMFORM).Include(m => m.SERVICEFEESTYPE).Include(m => m.UNIT);
            return View(list);
        }

        //
        // GET: /MedicalClaimFormDetail/Details/5

        public ActionResult Details(decimal id = 0)
        {
            MEDICALCLAIMFORMDETAIL medicalclaimformdetail = db.MEDICALCLAIMFORMDETAILS.Find(id);
            if (medicalclaimformdetail == null)
            {
                return HttpNotFound();
            }
            return View(medicalclaimformdetail);
        }

        //
        // GET: /MedicalClaimFormDetail/Create

        public ActionResult Create()
        {
            ViewBag.MEDICALCLAIMFORMID = new SelectList(db.MEDICALCLAIMFORMs, "ID", "PATIENTEMPLOYEE");
            ViewBag.SERVICEFEESTYPEID = new SelectList(db.SERVICEFEESTYPEs, "ID", "NAME");
            ViewBag.UNITID = new SelectList(db.UNITS, "ID", "NAME");
            return View();
        }

        //
        // POST: /MedicalClaimFormDetail/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MEDICALCLAIMFORMDETAIL medicalclaimformdetail)
        {
           try{
               medicalclaimformdetail.LASTUPDATED = System.DateTime.Now;
                db.MEDICALCLAIMFORMDETAILS.Add(medicalclaimformdetail);
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

            ViewBag.MEDICALCLAIMFORMID = new SelectList(db.MEDICALCLAIMFORMs, "ID", "PATIENTEMPLOYEE", medicalclaimformdetail.MEDICALCLAIMFORMID);
            ViewBag.SERVICEFEESTYPEID = new SelectList(db.SERVICEFEESTYPEs, "ID", "NAME", medicalclaimformdetail.SERVICEFEESTYPEID);
            ViewBag.UNITID = new SelectList(db.UNITS, "ID", "NAME", medicalclaimformdetail.UNITID);
           

            return View(medicalclaimformdetail);
        }

        //
        // GET: /MedicalClaimFormDetail/Edit/5

        public ActionResult Edit(decimal id = 0)
        {
            MEDICALCLAIMFORMDETAIL medicalclaimformdetail = db.MEDICALCLAIMFORMDETAILS.Find(id);
            if (medicalclaimformdetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.MEDICALCLAIMFORMID = new SelectList(db.MEDICALCLAIMFORMs, "ID", "PATIENTEMPLOYEE", medicalclaimformdetail.MEDICALCLAIMFORMID);
            ViewBag.SERVICEFEESTYPEID = new SelectList(db.SERVICEFEESTYPEs, "ID", "NAME", medicalclaimformdetail.SERVICEFEESTYPEID);
            ViewBag.UNITID = new SelectList(db.UNITS, "ID", "NAME", medicalclaimformdetail.UNITID);
            return View(medicalclaimformdetail);
        }

        //
        // POST: /MedicalClaimFormDetail/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MEDICALCLAIMFORMDETAIL medicalclaimformdetail)
        {
            try{
                db.Entry(medicalclaimformdetail).State = EntityState.Modified;
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
         
            ViewBag.MEDICALCLAIMFORMID = new SelectList(db.MEDICALCLAIMFORMs, "ID", "PATIENTEMPLOYEE", medicalclaimformdetail.MEDICALCLAIMFORMID);
            ViewBag.SERVICEFEESTYPEID = new SelectList(db.SERVICEFEESTYPEs, "ID", "NAME", medicalclaimformdetail.SERVICEFEESTYPEID);
            ViewBag.UNITID = new SelectList(db.UNITS, "ID", "NAME", medicalclaimformdetail.UNITID);
            return View(medicalclaimformdetail);
        }

        //
        // GET: /MedicalClaimFormDetail/Delete/5

        public ActionResult Delete(decimal id = 0)
        {
            MEDICALCLAIMFORMDETAIL medicalclaimformdetail = db.MEDICALCLAIMFORMDETAILS.Find(id);
            if (medicalclaimformdetail == null)
            {
                return HttpNotFound();
            }
            return View(medicalclaimformdetail);
        }

        //
        // POST: /MedicalClaimFormDetail/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            MEDICALCLAIMFORMDETAIL medicalclaimformdetail = db.MEDICALCLAIMFORMDETAILS.Find(id);
            db.MEDICALCLAIMFORMDETAILS.Remove(medicalclaimformdetail);
            TempData["AlertMessage"] = "deleted";
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}