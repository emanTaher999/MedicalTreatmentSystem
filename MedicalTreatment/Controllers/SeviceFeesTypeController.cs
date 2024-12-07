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
    public class SeviceFeesTypeController : Controller
    {
        private Entities db = new Entities();

       
        //
        // GET: /SeviceFeesType/

        public ActionResult Index(string search, int? x)
        {
            var list = db.SERVICEFEESTYPEs.Where(i => i.STATUS == "Active" && i.NAME.Contains(search) || search == null).ToList().ToPagedList(x ?? 1, 10);
            return View(list);
        }

        //
        // GET: /SeviceFeesType/Details/5

        public ActionResult Details(int id = 0)
        {
            SERVICEFEESTYPE sevicefeestype = db.SERVICEFEESTYPEs.Find(id);
            if (sevicefeestype == null)
            {
                return HttpNotFound();
            }
            return View(sevicefeestype);
        }

        //
        // GET: /SeviceFeesType/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /SeviceFeesType/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SERVICEFEESTYPE sevicefeestype)
        {
        
            try
            {
               
                    sevicefeestype.STATUS = "Active";
                    sevicefeestype.LASTUPDATED = DateTime.Now;
                    db.SERVICEFEESTYPEs.Add(sevicefeestype);
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
                return View(sevicefeestype);
            }

           
        }

        //
        // GET: /SeviceFeesType/Edit/5

        public ActionResult Edit(int id = 0)
        {
            SERVICEFEESTYPE sevicefeestype = db.SERVICEFEESTYPEs.Find(id);
            if (sevicefeestype == null)
            {
                return HttpNotFound();
            }
            return View(sevicefeestype);
        }

        //
        // POST: /SeviceFeesType/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SERVICEFEESTYPE sevicefeestype)
        {
            try 
            {
                sevicefeestype.STATUS = "Active";
                sevicefeestype.LASTUPDATED = DateTime.Now;
                db.Entry(sevicefeestype).State = EntityState.Modified;
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
                return View(sevicefeestype);
            }
           
        }

        //
        // GET: /SeviceFeesType/Delete/5

        public ActionResult Delete(int id = 0)
        {
            SERVICEFEESTYPE sevicefeestype = db.SERVICEFEESTYPEs.Find(id);
            if (sevicefeestype == null)
            {
                return HttpNotFound();
            }
            return View(sevicefeestype);
        }

        //
        // POST: /SeviceFeesType/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SERVICEFEESTYPE sevicefeestype = db.SERVICEFEESTYPEs.Find(id);
            db.SERVICEFEESTYPEs.Remove(sevicefeestype);
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