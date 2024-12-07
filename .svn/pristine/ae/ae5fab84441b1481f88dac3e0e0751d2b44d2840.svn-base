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
    public class OccupationController : Controller
    {
        private Entities db = new Entities();

        //
        // GET: /Occupation/

        public ActionResult Index(string search, int? x)
        {
            var list = db.OCCUPATIONs.Where(i => i.STATUS == "Active" && i.NAME.StartsWith(search) || search == null).ToList().ToPagedList(x ?? 1, 10);
            return View(list);

        }

        //
        // GET: /Occupation/Details/5

        public ActionResult Details(int id = 0)
        {
            OCCUPATION occupation = db.OCCUPATIONs.Find(id);
            if (occupation == null)
            {
                return HttpNotFound();
            }
            return View(occupation);
        }

        //
        // GET: /Occupation/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Occupation/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OCCUPATION occupation)
        {
            try
            {
                occupation.STATUS = "Active";
                occupation.LASTUPDATED = System.DateTime.Now;
                db.OCCUPATIONs.Add(occupation);
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
                return View(occupation);
            }
           
        }

        //
        // GET: /Occupation/Edit/5

        public ActionResult Edit(short id = 0)
        {
            OCCUPATION occupation = db.OCCUPATIONs.Find(id);
            if (occupation == null)
            {
                return HttpNotFound();
            }
            return View(occupation);
        }

        //
        // POST: /Occupation/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(OCCUPATION occupation)
        {
            try
            {
                occupation.STATUS = "Active";
                occupation.LASTUPDATED = System.DateTime.Now;
                db.Entry(occupation).State = EntityState.Modified;
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
                return View(occupation);
            }


           
        }

        //
        // GET: /Occupation/Delete/5

        public ActionResult Delete(short id = 0)
        {
            OCCUPATION occupation = db.OCCUPATIONs.Find(id);
            if (occupation == null)
            {
                return HttpNotFound();
            }
            return View(occupation);
        }

        //
        // POST: /Occupation/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OCCUPATION occupation = db.OCCUPATIONs.Find(id);
            db.OCCUPATIONs.Remove(occupation);
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