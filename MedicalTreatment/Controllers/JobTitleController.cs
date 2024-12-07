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
    public class JobTitleController : Controller
    {
        private Entities db = new Entities();

        //
        // GET: /JobTitle/

        public ActionResult Index(string search, int? x)
        {
            var list = db.JOBTITLES.Where(i => i.STATUS == "Active" && i.NAME.Contains(search) || search == null).ToList().ToPagedList(x ?? 1, 10);
            return View(list);
        }

        //
        // GET: /JobTitle/Details/5

        public ActionResult Details(int id = 0)
        {
            JOBTITLE jobtitle = db.JOBTITLES.Find(id);
            if (jobtitle == null)
            {
                return HttpNotFound();
            }
            return View(jobtitle);
        }

        //
        // GET: /JobTitle/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /JobTitle/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(JOBTITLE jobtitle)
        {
        
            try
            {
               
                    jobtitle.STATUS = "Active";
                    jobtitle.LASTUPDATED = DateTime.Now;
                    db.JOBTITLES.Add(jobtitle);
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
                return View(jobtitle);
            }

           
        }

        //
        // GET: /JobTitle/Edit/5

        public ActionResult Edit(int id = 0)
        {
            JOBTITLE jobtitle = db.JOBTITLES.Find(id);
            if (jobtitle == null)
            {
                return HttpNotFound();
            }
            return View(jobtitle);
        }

        //
        // POST: /JobTitle/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(JOBTITLE jobtitle)
        {
            try 
            {
                jobtitle.STATUS = "Active";
                jobtitle.LASTUPDATED = DateTime.Now;
                db.Entry(jobtitle).State = EntityState.Modified;
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
                return View(jobtitle);
            }
           
        }

        //
        // GET: /JobTitle/Delete/5

        public ActionResult Delete(int id = 0)
        {
            JOBTITLE jobtitle = db.JOBTITLES.Find(id);
            if (jobtitle == null)
            {
                return HttpNotFound();
            }
            return View(jobtitle);
        }

        //
        // POST: /JobTitle/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            JOBTITLE jobtitle = db.JOBTITLES.Find(id);
            db.JOBTITLES.Remove(jobtitle);
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