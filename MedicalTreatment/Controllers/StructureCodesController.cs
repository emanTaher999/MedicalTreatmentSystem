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
    public class StructureCodesController : Controller
    {
        private Entities db = new Entities();

        //
        // GET: /StructureCodes/

        public ActionResult Index(string search, int? x)
        {
            var list = db.STRUCTURECODES.Where(i => i.STATUS == "Active" && i.NAME.StartsWith(search) || search == null).ToList().ToPagedList(x ?? 1, 10);
            return View(list);
        }

        //
        // GET:/StructureCodes/Details/5
        public ActionResult Details(short id = 0)
        {
            STRUCTURECODE structurecode = db.STRUCTURECODES.Find(id);
            if (structurecode == null)
            {
                return HttpNotFound();
            }
            return View(structurecode);
        }

        //
        // GET: /StructureCodes/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /StructureCodes/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(STRUCTURECODE structurecode)
        {
            try
            {
                structurecode.STATUS = "Active";
                structurecode.LASTUPDATED = System.DateTime.Now;
                db.STRUCTURECODES.Add(structurecode);
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
                return View(structurecode);
            }
            

            
        }

        //
        // GET: /StructureCodes/Edit/5

        public ActionResult Edit(int id)
        {
            STRUCTURECODE structurecode = db.STRUCTURECODES.Find(id);
            if (structurecode == null)
            {
                return HttpNotFound();
            }
            return View(structurecode);
        }

        //
        // POST: /StructureCodes/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(STRUCTURECODE structurecode)
        {

            try
            {
                db.Entry(structurecode).State = EntityState.Modified;
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
                return View(structurecode);
            }
           
        }

        //
        // GET: /StructureCodes/Delete/5

        public ActionResult Delete(short id = 0)
        {
            STRUCTURECODE structurecode = db.STRUCTURECODES.Find(id);
            if (structurecode == null)
            {
                return HttpNotFound();
            }
            return View(structurecode);
        }

        //
        // POST: /StructureCodes/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            STRUCTURECODE structurecode = db.STRUCTURECODES.Find(id);
            db.STRUCTURECODES.Remove(structurecode);
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