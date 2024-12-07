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
    public class CompanyPolicyController : Controller
    {
        private Entities db = new Entities();

        //
        // GET: /CompanyPolicy/

        public ActionResult Index(string search, int? x)
        {
            //var companypolicies = db.COMPANYPOLICies.Include(c => c.ORGANIZATIONSTRUCTURE);
            var list = db.COMPANYPOLICies.Where(i => i.STATUS == "Active" && i.EMPLOYEESAMECEILING.StartsWith(search) || search == null).ToList().ToPagedList(x ?? 1, 10);
            return View(list);
        }

        //
        // GET: /CompanyPolicy/Details/5

        public ActionResult Details(short id = 0)
        {
            COMPANYPOLICY companypolicy = db.COMPANYPOLICies.Find(id);
            if (companypolicy == null)
            {
                return HttpNotFound();
            }
            return View(companypolicy);
        }

        //
        // GET: /CompanyPolicy/Create

        public ActionResult Create()
        {
            ViewBag.ORGANIZATIONID = new SelectList(db.ORGANIZATIONSTRUCTUREs, "ID", "NAME");
            return View();
        }

        //
        // POST: /CompanyPolicy/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(COMPANYPOLICY companypolicy)
        {
            try
            {
                companypolicy.STATUS = "Active";
                companypolicy.LASTUPDATED = System.DateTime.Now;
                db.COMPANYPOLICies.Add(companypolicy);
                db.SaveChanges();
                return RedirectToAction("Index");
                 ViewBag.ORGANIZATIONID = new SelectList(db.ORGANIZATIONSTRUCTUREs, "ID", "NAME", companypolicy.ORGANIZATIONID);
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

            return View(companypolicy);
        }

        //
        // GET: /CompanyPolicy/Edit/5

        public ActionResult Edit(short id = 0)
        {
            COMPANYPOLICY companypolicy = db.COMPANYPOLICies.Find(id);
            if (companypolicy == null)
            {
                return HttpNotFound();
            }
            ViewBag.ORGANIZATIONID = new SelectList(db.ORGANIZATIONSTRUCTUREs, "ID", "NAME", companypolicy.ORGANIZATIONID);
            return View(companypolicy);
        }

        //
        // POST: /CompanyPolicy/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(COMPANYPOLICY companypolicy)
        {
            try
            {
                db.Entry(companypolicy).State = EntityState.Modified;
                db.SaveChanges();
                TempData["AlertMessage"] = "edit";
                return RedirectToAction("Index");

                ViewBag.ORGANIZATIONID = new SelectList(db.ORGANIZATIONSTRUCTUREs, "ID", "NAME", companypolicy.ORGANIZATIONID);
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
            return View(companypolicy);
        }

        //
        // GET: /CompanyPolicy/Delete/5

        public ActionResult Delete(short id = 0)
        {
            COMPANYPOLICY companypolicy = db.COMPANYPOLICies.Find(id);
            if (companypolicy == null)
            {
                return HttpNotFound();
            }
            return View(companypolicy);
        }

        //
        // POST: /CompanyPolicy/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            COMPANYPOLICY companypolicy = db.COMPANYPOLICies.Find(id);
            db.COMPANYPOLICies.Remove(companypolicy);
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