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
    public class MedicalServiceCeilingController : Controller
    {
        private Entities db = new Entities();

        //
        // GET: /MedicalServiceCeiling/

        public ActionResult Index(string search, int? x)
        {
            var medicalserviceceilings = db.MEDICALSERVICECEILINGs.Include(m => m.MEDICALDETAILSERVICE).Include(m => m.MEDICALSERVICE);
            return View(medicalserviceceilings.Where(i => i.STATUS == "Active" && i.MEDICALSERVICE.NAME.Contains(search) || search == null).ToList().ToPagedList(x ?? 1, 10));
        }

        //
        // GET: /MedicalServiceCeiling/Details/5

        public ActionResult Details(int id = 0)
        {
            MEDICALSERVICECEILING medicalserviceceiling = db.MEDICALSERVICECEILINGs.Find(id);
            if (medicalserviceceiling == null)
            {
                return HttpNotFound();
            }
            return View(medicalserviceceiling);
        }

        //
        // GET: /MedicalServiceCeiling/Create

        public ActionResult Create()
        {
            ViewBag.MEDICALDETAILSERVICEID = new SelectList(db.MEDICALDETAILSERVICES.Where(i=>i.STATUS=="Active"), "ID", "NAME");
            ViewBag.MEDICALSERVICEID = new SelectList(db.MEDICALSERVICES.Where(i => i.STATUS == "Active"), "ID", "NAME");
            return View();
        }

        //
        // POST: /MedicalServiceCeiling/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MEDICALSERVICECEILING medicalserviceceiling)
        {
            try
            {
                medicalserviceceiling.STATUS = "Active";
                medicalserviceceiling.LASTUPDATED = System.DateTime.Now;
                db.MEDICALSERVICECEILINGs.Add(medicalserviceceiling);
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
            ViewBag.MEDICALDETAILSERVICEID = new SelectList(db.MEDICALDETAILSERVICES, "ID", "NAME", medicalserviceceiling.MEDICALDETAILSERVICEID);
            ViewBag.MEDICALSERVICEID = new SelectList(db.MEDICALSERVICES, "ID", "NAME", medicalserviceceiling.MEDICALSERVICEID);
            return View(medicalserviceceiling);
        }

        //
        // GET: /MedicalServiceCeiling/Edit/5

        public ActionResult Edit(int id = 0)
        {
            MEDICALSERVICECEILING medicalserviceceiling = db.MEDICALSERVICECEILINGs.Find(id);
            if (medicalserviceceiling == null)
            {
                return HttpNotFound();
            }
            ViewBag.MEDICALDETAILSERVICEID = new SelectList(db.MEDICALDETAILSERVICES.Where(i => i.STATUS == "Active"), "ID", "NAME", medicalserviceceiling.MEDICALDETAILSERVICEID);
            ViewBag.MEDICALSERVICEID = new SelectList(db.MEDICALSERVICES.Where(i => i.STATUS == "Active"), "ID", "NAME", medicalserviceceiling.MEDICALSERVICEID);
            return View(medicalserviceceiling);
        }

        //
        // POST: /MedicalServiceCeiling/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MEDICALSERVICECEILING medicalserviceceiling)
        {
            try
            {
                medicalserviceceiling.STATUS = "Active";
                medicalserviceceiling.LASTUPDATED = System.DateTime.Now;
                db.Entry(medicalserviceceiling).State = EntityState.Modified;
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
            ViewBag.MEDICALDETAILSERVICEID = new SelectList(db.MEDICALDETAILSERVICES, "ID", "NAME", medicalserviceceiling.MEDICALDETAILSERVICEID);
            ViewBag.MEDICALSERVICEID = new SelectList(db.MEDICALSERVICES, "ID", "NAME", medicalserviceceiling.MEDICALSERVICEID);
            return View(medicalserviceceiling);
        }

        //
        // GET: /MedicalServiceCeiling/Delete/5

        public ActionResult Delete(int id = 0)
        {
            MEDICALSERVICECEILING medicalserviceceiling = db.MEDICALSERVICECEILINGs.Find(id);
            if (medicalserviceceiling == null)
            {
                return HttpNotFound();
            }
            ViewBag.MEDICALDETAILSERVICEID = new SelectList(db.MEDICALDETAILSERVICES.Where(i => i.STATUS == "Active"), "ID", "NAME", medicalserviceceiling.MEDICALDETAILSERVICEID);
            ViewBag.MEDICALSERVICEID = new SelectList(db.MEDICALSERVICES.Where(i => i.STATUS == "Active"), "ID", "NAME", medicalserviceceiling.MEDICALSERVICEID);
            return View(medicalserviceceiling);
        }

        //
        // POST: /MedicalServiceCeiling/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MEDICALSERVICECEILING medicalserviceceiling = db.MEDICALSERVICECEILINGs.Find(id);
            db.MEDICALSERVICECEILINGs.Remove(medicalserviceceiling);
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