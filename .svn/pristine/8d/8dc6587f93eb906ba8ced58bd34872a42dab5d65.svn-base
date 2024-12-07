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
    public class UnitsController : Controller
    {
        private Entities db = new Entities();

        //
        // GET: /Units/

        public ActionResult Index(string search, int? x)
        {
            var list = db.UNITS.Where(i => i.STATUS == "Active" && i.NAME.StartsWith(search) || search == null).ToList().ToPagedList(x ?? 1, 10);
            return View(list);
        }

        //
        // GET: /Units/Details/5

        public ActionResult Details(int id = 0)
        {
            UNIT unit = db.UNITS.Find(id);
            if (unit == null)
            {
                return HttpNotFound();
            }
            return View(unit);
        }

        //
        // GET: /Units/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Units/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UNIT unit)
        {
            try
            {
                unit.STATUS = "Active";
                unit.LASTUPDATED = System.DateTime.Now;
                db.UNITS.Add(unit);
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


           
            return View(unit);
        }

        //
        // GET: /Units/Edit/5

        public ActionResult Edit(int id = 0)
        {
            UNIT unit = db.UNITS.Find(id);
            if (unit == null)
            {
                return HttpNotFound();
            }
            return View(unit);
        }

        //
        // POST: /Units/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UNIT unit)
        {
            try
            {
                db.Entry(unit).State = EntityState.Modified;
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
                return View(unit);
            }
        }

        //
        // GET: /Units/Delete/5

        public ActionResult Delete(int id = 0)
        {
            UNIT unit = db.UNITS.Find(id);
            if (unit == null)
            {
                return HttpNotFound();
            }
            return View(unit);
        }

        //
        // POST: /Units/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UNIT unit = db.UNITS.Find(id);
            db.UNITS.Remove(unit);
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