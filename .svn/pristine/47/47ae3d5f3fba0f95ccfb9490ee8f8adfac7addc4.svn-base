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

namespace MedicalTreatment.Controllers
{
    public class GeneralCeilingController : Controller
    {
        private Entities db = new Entities();

        //
        // GET: /GeneralCeiling/

        public ActionResult Index(string search, int? x)
             
        {
            var list = db.GENERALCEILINGs.Where(i => i.STATUS == "Active" && search == null).ToList().ToPagedList(x ?? 1, 10);
            return View(list);
            //return View(db.GENERALCEILINGs.ToList());
        }

        //
        // GET: /GeneralCeiling/Details/5

        public ActionResult Details(long id = 0)
        {
            GENERALCEILING generalceiling = db.GENERALCEILINGs.Find(id);
            if (generalceiling == null)
            {
                return HttpNotFound();
            }
            return View(generalceiling);
        }

        //
        // GET: /GeneralCeiling/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /GeneralCeiling/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GENERALCEILING generalceiling)
        {
            try
            {
                generalceiling.STATUS = "Active";
                generalceiling.LASTUPDATED = DateTime.Now;
                db.GENERALCEILINGs.Add(generalceiling);
                db.SaveChanges();
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
            return View(generalceiling);
        }

        //
        // GET: /GeneralCeiling/Edit/5

        public ActionResult Edit(long id = 0)
        {
            GENERALCEILING generalceiling = db.GENERALCEILINGs.Find(id);
            if (generalceiling == null)
            {
                return HttpNotFound();
            }
            return View(generalceiling);
        }

        //
        // POST: /GeneralCeiling/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(GENERALCEILING generalceiling)
        {
           try
            {
                db.Entry(generalceiling).State = EntityState.Modified;
                db.SaveChanges();
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
            return View(generalceiling);
        }

        //
        // GET: /GeneralCeiling/Delete/5

        public ActionResult Delete(long id = 0)
        {
            GENERALCEILING generalceiling = db.GENERALCEILINGs.Find(id);
            if (generalceiling == null)
            {
                return HttpNotFound();
            }
            return View(generalceiling);
        }

        //
        // POST: /GeneralCeiling/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            GENERALCEILING generalceiling = db.GENERALCEILINGs.Find(id);
            db.GENERALCEILINGs.Remove(generalceiling);
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