﻿using System;
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
    public class SuppliersController : Controller
    {
        private Entities db = new Entities();

        //
        // GET: /Suppliers/

        public ActionResult Index(string search, int? x)
        {
            var list = db.SUPPLIERS.Where(i => i.STATUS == "Active" && i.NAME.Contains(search)|| search == null).ToList().ToPagedList(x ?? 1, 10);
            return View(list);
        }

        //
        // GET: /Suppliers/Details/5

        public ActionResult Details(int id = 0)
        {
            SUPPLIER supplier = db.SUPPLIERS.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        //
        // GET: /Suppliers/Create

        public ActionResult Create()
        {
            ViewBag.SUPPLIERTYPEID = new SelectList(db.SUPPLIERTYPES, "ID", "NAME");
            return View();
        }

        //
        // POST: /Suppliers/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SUPPLIER supplier)
        {
            try
            {

                supplier.STATUS = "Active";
                supplier.LASTUPDATED = DateTime.Now;
                db.SUPPLIERS.Add(supplier);
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

            return View(supplier);
        }

        //
        // GET: /Suppliers/Edit/5

        public ActionResult Edit(int id = 0)
        {
          
            SUPPLIER supplier = db.SUPPLIERS.Find(id);
          
            if (supplier == null)
            {
                return HttpNotFound();
            }
            ViewBag.SUPPLIERTYPE = new SelectList(db.SUPPLIERTYPES, "ID", "NAME", supplier.SUPPLIERTYPEID);
            return View(supplier);
        }

        //
        // POST: /Suppliers/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SUPPLIER supplier, int? SUPPLIERTYPE)
        {
            try
            {
                supplier.SUPPLIERTYPEID = (int) SUPPLIERTYPE;
                supplier.STATUS = "Active";
                supplier.LASTUPDATED = DateTime.Now;
                db.Entry(supplier).State = EntityState.Modified;
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
            return View(supplier);
        }

        //
        // GET: /Suppliers/Delete/5

        public ActionResult Delete(int id = 0)
        {
            SUPPLIER supplier = db.SUPPLIERS.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        //
        // POST: /Suppliers/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SUPPLIER supplier = db.SUPPLIERS.Find(id);
            db.SUPPLIERS.Remove(supplier);
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