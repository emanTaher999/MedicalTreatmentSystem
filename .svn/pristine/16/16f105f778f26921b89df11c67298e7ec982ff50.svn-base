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
    public class SupplierTypesController : Controller
    {
        private Entities db = new Entities();

        //
        // GET: /SupplierTypes/

        public ActionResult Index(string search, int? x)
        {
            var list = db.SUPPLIERTYPES.Where(i => i.STATUS == "Active" && i.NAME.StartsWith(search) || search == null).ToList().ToPagedList(x ?? 1, 10);
            return View(list);
           // return View(db.SUPPLIERTYPES.ToList());
        }

        //
        // GET: /SupplierTypes/Details/5

        public ActionResult Details(int id = 0)
        {
            SUPPLIERTYPE suppliertype = db.SUPPLIERTYPES.Find(id);
            if (suppliertype == null)
            {
                return HttpNotFound();
            }
            return View(suppliertype);
        }

        //
        // GET: /SupplierTypes/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /SupplierTypes/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SUPPLIERTYPE suppliertype)
        {

            try
            {
                
                suppliertype.STATUS = "Active";
                suppliertype.LASTUPDATED = System.DateTime.Now;
                db.Entry(suppliertype).State = EntityState.Modified;
                db.SUPPLIERTYPES.Add(suppliertype);
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

            }


            return View(suppliertype);
        }

        //
        // GET: /SupplierTypes/Edit/5

        public ActionResult Edit(int id = 0)
        {
            SUPPLIERTYPE suppliertype = db.SUPPLIERTYPES.Find(id);
            if (suppliertype == null)
            {
                return HttpNotFound();
            }
            return View(suppliertype);
        }

        //
        // POST: /SupplierTypes/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SUPPLIERTYPE suppliertype)
        {
            try
            {
                db.Entry(suppliertype).State = EntityState.Modified;
                suppliertype.STATUS = "Active";
                suppliertype.LASTUPDATED = System.DateTime.Now;
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

            }

            return View(suppliertype);
        }

        //
        // GET: /SupplierTypes/Delete/5

        public ActionResult Delete(int id = 0)
        {
            SUPPLIERTYPE suppliertype = db.SUPPLIERTYPES.Find(id);
            if (suppliertype == null)
            {
                return HttpNotFound();
            }
            return View(suppliertype);
        }

        //
        // POST: /SupplierTypes/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SUPPLIERTYPE suppliertype = db.SUPPLIERTYPES.Find(id);
            db.SUPPLIERTYPES.Remove(suppliertype);
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