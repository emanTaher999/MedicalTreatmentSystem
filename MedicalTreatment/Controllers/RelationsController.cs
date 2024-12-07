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
    public class RelationsController : Controller
    {
        private Entities db = new Entities();

        //
        // GET: /Relations/

        // GET: /StructureCodes/

        public ActionResult Index(string search, int? x)
        {
            var list = db.RELATIONS.Where(i => i.STATUS == "Active" && i.NAME.StartsWith(search) || search == null).ToList().ToPagedList(x ?? 1, 10);
            return View(list);
        }

        //public ActionResult Index()
        //{
        //    return View(db.RELATIONS.ToList());
        //}

        //
        // GET: /Relations/Details/5

        public ActionResult Details(int id = 0)
        {
            RELATION relation = db.RELATIONS.Find(id);
            if (relation == null)
            {
                return HttpNotFound();
            }
            return View(relation);
        }

        //
        // GET: /Relations/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Relations/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RELATION relation)
        {
            if (relation.GENDER == "Male")
            {
                try
                {
                    relation.MATERNITYAPPLICABLE = "No";
                    db.Entry(relation).State = EntityState.Modified;
                    relation.STATUS = "Active";
                    relation.LASTUPDATED = System.DateTime.Now;
                    db.RELATIONS.Add(relation);
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
            }
            else if (relation.GENDER == "Male")
            {
                try
                {
                    relation.MATERNITYAPPLICABLE = "Yes";
                    db.Entry(relation).State = EntityState.Modified;
                    relation.STATUS = "Active";
                    relation.LASTUPDATED = System.DateTime.Now;
                    db.RELATIONS.Add(relation);
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
            }
            return View(relation);
         
            //try
            //{
            //    relation.STATUS = "Active";
            //    relation.LASTUPDATED = System.DateTime.Now;
            //    db.RELATIONS.Add(relation);
            //    db.SaveChanges();
            //    TempData["AlertMessage"] = "success";
            //    return RedirectToAction("Index");
            //}
            //catch (DbEntityValidationException e)
            //{
            //    foreach (var eve in e.EntityValidationErrors)
            //    {
            //        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
            //            eve.Entry.Entity.GetType().Name, eve.Entry.State);
            //        foreach (var ve in eve.ValidationErrors)
            //        {
            //            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
            //                ve.PropertyName, ve.ErrorMessage);
            //        }
            //    }
            //    return View(relation);
            //}
            
       
              
          

           
        }

        //
        // GET: /Relations/Edit/5

        public ActionResult Edit(int id = 0)
        {
            RELATION relation = db.RELATIONS.Find(id);
            if (relation == null)
            {
                return HttpNotFound();
            }
            return View(relation);
        }

        //
        // POST: /Relations/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RELATION relation)
        {
            if (relation.GENDER=="Male")
            {
                try
                {
                    relation.MATERNITYAPPLICABLE = "No";
                    relation.LASTUPDATED = System.DateTime.Now;
                    relation.STATUS = "Active";
                    db.Entry(relation).State = EntityState.Modified;
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
            }
            else if (relation.GENDER == "Male")
            {
                try
                {
                    relation.MATERNITYAPPLICABLE = "Yes";
                    relation.LASTUPDATED = System.DateTime.Now;
                    relation.STATUS = "Active";
                    db.Entry(relation).State = EntityState.Modified;
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
            }
            return View(relation);

        }

        //
        // GET: /Relations/Delete/5

        public ActionResult Delete(int id = 0)
        {
            RELATION relation = db.RELATIONS.Find(id);
            if (relation == null)
            {
                return HttpNotFound();
            }
            return View(relation);
        }

        //
        // POST: /Relations/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RELATION relation = db.RELATIONS.Find(id);
            db.RELATIONS.Remove(relation);
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