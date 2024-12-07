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
    public class OrganizationStructureController : Controller
    {
        private Entities db = new Entities();

        //
        // GET: /OrganizationStructure/

        public ActionResult Index(string search, int? x)
        {
            var list = db.ORGANIZATIONSTRUCTUREs.Where(i => i.STATUS == "Active" && i.NAME.StartsWith(search) || search == null).ToList().ToPagedList(x ?? 1, 10);
            return View(list);
            //var organizationstructures = db.ORGANIZATIONSTRUCTUREs.Include(o => o.ORGANIZATIONSTRUCTURE2);
            //return View(organizationstructures.ToList());
        }

        //
        // GET: /OrganizationStructure/Details/5

        public ActionResult Details(int id = 0)
        {
            ORGANIZATIONSTRUCTURE organizationstructure = db.ORGANIZATIONSTRUCTUREs.Find(id);
            if (organizationstructure == null)
            {
                return HttpNotFound();
            }
            return View(organizationstructure);
        }

        //
        // GET: /OrganizationStructure/Create

        public ActionResult Create()
        {
            ViewBag.PARENTID = new SelectList(db.ORGANIZATIONSTRUCTUREs, "ID", "NAME");
            ViewBag.StructureCodId = new SelectList(db.STRUCTURECODES, "ID", "NAME");
            return View();
        }

        //
        // POST: /OrganizationStructure/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ORGANIZATIONSTRUCTURE organizationstructure)
        {
            try
            {
                organizationstructure.STATUS = "Active";
                organizationstructure.LASTUPDATED = System.DateTime.Now;
                db.ORGANIZATIONSTRUCTUREs.Add(organizationstructure);
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

            ViewBag.PARENTID = new SelectList(db.ORGANIZATIONSTRUCTUREs, "ID", "NAME", organizationstructure.PARENTID);
            ViewBag.StructureCodId = new SelectList(db.STRUCTURECODES, "ID", "NAME" , organizationstructure.STRUCTURECODEID);

            return View(organizationstructure);
        }

        //
        // GET: /OrganizationStructure/Edit/5

        public ActionResult Edit(int id = 0)
        {
            ORGANIZATIONSTRUCTURE organizationstructure = db.ORGANIZATIONSTRUCTUREs.Find(id);
            if (organizationstructure == null)
            {
                return HttpNotFound();
            }
            ViewBag.PARENTID = new SelectList(db.ORGANIZATIONSTRUCTUREs, "ID", "NAME", organizationstructure.PARENTID);
            ViewBag.StructureCodId = new SelectList(db.STRUCTURECODES, "ID", "NAME", organizationstructure.STRUCTURECODEID);
            return View(organizationstructure);
        }

        //
        // POST: /OrganizationStructure/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ORGANIZATIONSTRUCTURE organizationstructure,int PARENTID)
        {
            organizationstructure.PARENTID = PARENTID;
            try
            {

                db.Entry(organizationstructure).State = EntityState.Modified;
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
         
            ViewBag.PARENTID = new SelectList(db.ORGANIZATIONSTRUCTUREs, "ID", "NAME", organizationstructure.PARENTID);
            ViewBag.StructureCodId = new SelectList(db.STRUCTURECODES, "ID", "NAME", organizationstructure.STRUCTURECODEID);
            return View(organizationstructure);
        }

        //
        // GET: /OrganizationStructure/Delete/5

        public ActionResult Delete(int id = 0)
        {
            ORGANIZATIONSTRUCTURE organizationstructure = db.ORGANIZATIONSTRUCTUREs.Find(id);
            if (organizationstructure == null)
            {
                return HttpNotFound();
            }
            ViewBag.PARENTID = new SelectList(db.ORGANIZATIONSTRUCTUREs, "ID", "NAME", organizationstructure.PARENTID);
            ViewBag.StructureCodId = new SelectList(db.STRUCTURECODES, "ID", "NAME", organizationstructure.STRUCTURECODEID);
            return View(organizationstructure);
        }

        //
        // POST: /OrganizationStructure/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ORGANIZATIONSTRUCTURE organizationstructure = db.ORGANIZATIONSTRUCTUREs.Find(id);
            db.ORGANIZATIONSTRUCTUREs.Remove(organizationstructure);
            db.SaveChanges();
             TempData["AlertMessage"] = "deleted";
            return RedirectToAction("Index");
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}