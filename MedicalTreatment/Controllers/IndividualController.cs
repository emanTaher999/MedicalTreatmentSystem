﻿using System;
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
using System.IO;


namespace MedicalTreatment.Controllers
{
    public class IndividualController : Controller
    {
        private Entities db = new Entities();

        //
        // GET: /Individual/

        public ActionResult Index(string search, int? x)
        {
            var individuals = db.INDIVIDUALS.Where(a=>a.MAININDIVIDUALID==null).Include(i => i.OCCUPATION).Include(i => i.INDIVIDUAL1).Include(i => i.ORGANIZATIONSTRUCTURE).Include(i => i.ORGANIZATIONSTRUCTURE1).Include(i => i.ORGANIZATIONSTRUCTURE2).Include(i => i.ORGANIZATIONSTRUCTURE3).Include(i => i.RELATION).Include(i => i.JOBTITLE);
            return View(individuals.Where(i => i.STATUS == "Active" && i.NAME.Contains(search) || search == null).ToList().ToPagedList(x ?? 1, 10));
        }

        //
        // GET: /Individual/Details/5

        public ActionResult Details(int id = 0)
        {
            VMIndividual model = new VMIndividual();
            var individual = db.INDIVIDUALS.Where(i => i.ID == id).ToList();
            List<INDIVIDUAL> individual1 = db.INDIVIDUALS.Where(o => o.MAININDIVIDUALID == id).ToList();
            model.FamilyData = individual1;
            model.Individual = individual;
            if (individual == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        //
        // GET: /Individual/Create

        public ActionResult Create()
        {
            ViewBag.OCCUPATIONID = new SelectList(db.OCCUPATIONs, "ID", "NAME");
            ViewBag.MAININDIVIDUALID = new SelectList(db.INDIVIDUALS, "ID", "NAME");
            ViewBag.DEPARTMENT = new SelectList(db.ORGANIZATIONSTRUCTUREs, "ID", "NAME");
            ViewBag.MAINORGANISATIONID = new SelectList(db.ORGANIZATIONSTRUCTUREs.Where(i => i.ID == 1), "ID", "NAME");
            ViewBag.ORGANISATION = new SelectList(db.ORGANIZATIONSTRUCTUREs, "ID", "NAME");
            ViewBag.SECTIONID = new SelectList(db.ORGANIZATIONSTRUCTUREs, "ID", "NAME");
            ViewBag.RELATIONID = new SelectList(db.RELATIONS, "ID", "NAME");
            ViewBag.JOBTITLEID = new SelectList(db.JOBTITLES, "ID", "NAME");
            return View();
        }
        public JsonResult getpartialDiv(int? Number)
        {
            //---------------Individual Data------------------------------------------------
            ViewBag.OCCUPATIONID = new SelectList(db.OCCUPATIONs, "ID", "NAME");
            ViewBag.MAININDIVIDUALID = new SelectList(db.INDIVIDUALS, "ID", "NAME");
            ViewBag.DEPARTMENT = new SelectList(db.ORGANIZATIONSTRUCTUREs, "ID", "NAME");
            ViewBag.MAINORGANISATIONID = new SelectList(db.ORGANIZATIONSTRUCTUREs.Where(i => i.ID == 1), "ID", "NAME");
            ViewBag.ORGANISATION = new SelectList(db.ORGANIZATIONSTRUCTUREs, "ID", "NAME");
            ViewBag.SECTIONID = new SelectList(db.ORGANIZATIONSTRUCTUREs, "ID", "NAME");
            ViewBag.RELATIONID = new SelectList(db.RELATIONS, "ID", "NAME");
            ViewBag.JOBTITLEID = new SelectList(db.JOBTITLES, "ID", "NAME");
            //---------------Individual Data------------------------------------------------
            INDIVIDUAL model = new INDIVIDUAL();
            List<INDIVIDUAL> repeat = new List<INDIVIDUAL>();
            for (int i = 0; i < Number; i++)
            {
                repeat.Add(model);
            }

            var result = CommonUtils.RenderPartialViewToString(this, "IndividualData", repeat);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getORGANISATION(int? ORGANISATION)
        {

            var Lst = db.ORGANIZATIONSTRUCTUREs.Where(i => i.PARENTID == ORGANISATION && i.STATUS == "Active").ToList();


            var ORGANISATION1 = new SelectList(Lst, "ID", "NAME");

            return Json(ORGANISATION1, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getDEPARTMENT(int? DEPARTMENT)
        {

            var Lst = db.ORGANIZATIONSTRUCTUREs.Where(i => i.PARENTID == DEPARTMENT && i.STATUS == "Active").ToList();


            var DEPARTMENT1 = new SelectList(Lst, "ID", "NAME");

            return Json(DEPARTMENT1, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getSECTIONID(int? SECTIONID)
        {

            var Lst = db.ORGANIZATIONSTRUCTUREs.Where(i => i.PARENTID == SECTIONID && i.STATUS == "Active").ToList();


            var SECTIONID1 = new SelectList(Lst, "ID", "NAME");

            return Json(SECTIONID1, JsonRequestBehavior.AllowGet);
        }
        //
        // POST: /Individual/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(INDIVIDUAL individual, List<INDIVIDUAL> FamilyData, HttpPostedFileBase file, List<HttpPostedFileBase> file1, int? MAINORGANISATIONID, int? ORGANISATION, int? DEPARTMENT, int? SECTIONID, int? OCCUPATIONID, int? JOBTITLEID)
        {
             using (Stream fs = file.InputStream)
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        individual.PHOTO = br.ReadBytes((Int32)fs.Length);
                    }
                }
            try
            {
                RELATION relation = db.RELATIONS.SingleOrDefault(i=>i.NAME=="الكافل");
                RELATION relation1 = db.RELATIONS.SingleOrDefault(i => i.NAME == "الكافلة");
                if (individual.GENDER=="Male")
                {
                    individual.RELATIONID = relation.ID; 
                }
                else if (individual.GENDER=="Female")
                {
                   individual.RELATIONID = relation1.ID; 
                }
                individual.MAINORGANISATIONID = Convert.ToInt32(MAINORGANISATIONID);
                individual.ORGANISATIONID = Convert.ToInt32(ORGANISATION);
                individual.DEPARTMENTID = DEPARTMENT;
                individual.SECTIONID = SECTIONID;
                individual.OCCUPATIONID = OCCUPATIONID;
                individual.JOBTITLEID = JOBTITLEID;
                individual.STARTDATE = System.DateTime.Now;
                individual.STATUS = "Active";
                individual.LASTUPDATED = System.DateTime.Now;
                    db.INDIVIDUALS.Add(individual);
                    db.SaveChanges();
                    db.Entry(individual).State = EntityState.Detached;
                
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

            try
            {
                INDIVIDUAL savefamily = new INDIVIDUAL();
                var mainindividualid = db.INDIVIDUALS.SingleOrDefault(o=>o.NAME==individual.NAME&&o.NAMEEN==individual.NAMEEN&&o.JOBNUMBER==individual.JOBNUMBER&&o.JOBTITLEID==individual.JOBTITLEID);
                int i = -1;
                        foreach (var item in FamilyData)
                            {
                                using (Stream fs = file1[++i].InputStream)
                                    {
                                        using (BinaryReader br = new BinaryReader(fs))
                                        {
                                            savefamily.PHOTO = br.ReadBytes((Int32)fs.Length);
                                        }
                                    }
                        savefamily.MAININDIVIDUALID = mainindividualid.ID;
                        savefamily.MAINORGANISATIONID = mainindividualid.MAINORGANISATIONID;
                        savefamily.ORGANISATIONID = mainindividualid.ORGANISATIONID;
                        savefamily.STARTDATE = System.DateTime.Now;
                        savefamily.NATIONALITY = mainindividualid.NATIONALITY;
                        savefamily.STATUS = "Active";
                        savefamily.LASTUPDATED = System.DateTime.Now;
                        savefamily.NAME = item.NAME;
                        savefamily.NAMEEN = item.NAMEEN;
                        savefamily.BIRTHDATE = item.BIRTHDATE;
                        savefamily.GENDER = item.GENDER;
                        savefamily.MARITALSTATUS = item.MARITALSTATUS;
                        savefamily.RELATIONID = item.RELATIONID;
                        db.INDIVIDUALS.Add(savefamily);
                        db.SaveChanges();
                        //db.Entry(savefamily).State = EntityState.Detached;

                        }



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
           

            ViewBag.OCCUPATIONID = new SelectList(db.OCCUPATIONs, "ID", "NAME", individual.OCCUPATIONID);
            ViewBag.MAININDIVIDUALID = new SelectList(db.INDIVIDUALS, "ID", "NAME", individual.MAININDIVIDUALID);
            ViewBag.DEPARTMENTID = new SelectList(db.ORGANIZATIONSTRUCTUREs, "ID", "NAME", individual.DEPARTMENTID);
            ViewBag.MAINORGANISATIONID = new SelectList(db.ORGANIZATIONSTRUCTUREs, "ID", "NAME", individual.MAINORGANISATIONID);
            ViewBag.ORGANISATIONID = new SelectList(db.ORGANIZATIONSTRUCTUREs, "ID", "NAME", individual.ORGANISATIONID);
            ViewBag.SECTIONID = new SelectList(db.ORGANIZATIONSTRUCTUREs, "ID", "NAME", individual.SECTIONID);
            ViewBag.RELATIONID = new SelectList(db.RELATIONS, "ID", "NAME", individual.RELATIONID);
            ViewBag.JOBTITLEID = new SelectList(db.JOBTITLES, "ID", "NAME", individual.JOBTITLEID);
            return View(individual);
        }

        //
        // GET: /Individual/Edit/5

        public ActionResult Edit(int id = 0)
        {
            VMIndividual model = new VMIndividual();
            INDIVIDUAL individual = db.INDIVIDUALS.Find(id);
            List<INDIVIDUAL> individual1 = db.INDIVIDUALS.Where(o => o.MAININDIVIDUALID == id).ToList();
            var ind = db.INDIVIDUALS.Where(i => i.ID == id).ToList();
            var ind1 = db.INDIVIDUALS.Where(x=>x.MAININDIVIDUALID==id).ToList();
            model.Individual = ind;
            model.FamilyData = ind1;
            if (individual == null)
            {
                return HttpNotFound();
            }
            ViewBag.OCCUPATIONID = new SelectList(db.OCCUPATIONs, "ID", "NAME", individual.OCCUPATIONID);
            ViewBag.MAININDIVIDUALID = new SelectList(db.INDIVIDUALS, "ID", "NAME", individual.MAININDIVIDUALID);
            ViewBag.DEPARTMENT = new SelectList(db.ORGANIZATIONSTRUCTUREs, "ID", "NAME", individual.DEPARTMENTID);
            ViewBag.MAINORGANISATIONID = new SelectList(db.ORGANIZATIONSTRUCTUREs.Where(i => i.ID == 1), "ID", "NAME", individual.MAINORGANISATIONID);
            ViewBag.ORGANISATION = new SelectList(db.ORGANIZATIONSTRUCTUREs, "ID", "NAME", individual.ORGANISATIONID);
            ViewBag.SECTIONID = new SelectList(db.ORGANIZATIONSTRUCTUREs, "ID", "NAME", individual.SECTIONID);
            ViewBag.RELATIONID = new SelectList(db.RELATIONS, "ID", "NAME", individual.RELATIONID);
            ViewBag.JOBTITLEID = new SelectList(db.JOBTITLES, "ID", "NAME", individual.JOBTITLEID);

            //------------------------------------------------------
            foreach (var item in individual1)
            {
                 ViewBag.RELATIONID1 = new SelectList(db.RELATIONS, "ID", "NAME", item.RELATIONID);
            }
            ViewBag.GENDER = new List<SelectListItem>{ new SelectListItem{
                Text="ذكر",
                Value = "Male"
            },
 new SelectListItem{
                Text="انثى",
                Value = "Female"
            }};
            ViewBag.MARITALSTATUS = new List<SelectListItem>{ new SelectListItem{
                Text="متزوج/ة",
                Value = "Married"
            },
 new SelectListItem{
                Text="اعزب",
                Value = "Single"
            },
           new SelectListItem{
                Text="مطلق/ة",
                Value = "Divorced"
            },
            new SelectListItem{
                Text="ارمل/ة",
                Value = "Widow"
            }};
            ViewBag.NATIONALITY = new List<SelectListItem>{ new SelectListItem{
                Text="مواطن",
                Value = "Citizen"
            },
 new SelectListItem{
                Text="اجنبي",
                Value = "Foreigner"
            }};
            return View(model);
        }

        //
        // POST: /Individual/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VMIndividual model, List<INDIVIDUAL> FamilyData, HttpPostedFileBase file, List<HttpPostedFileBase> file2, int? MAINORGANISATIONID, int? ORGANISATION, int? DEPARTMENT, int? SECTIONID, int? OCCUPATIONID, int? JOBTITLEID,int id) 
        {
            INDIVIDUAL individual = db.INDIVIDUALS.SingleOrDefault(i => i.ID == id);
           
            try
            {
    if (file!=null)
      {
      foreach (var item in model.Individual)
	{
        using (Stream fs = file.InputStream)
        {
            using (BinaryReader br = new BinaryReader(fs))
            {
                individual.PHOTO = br.ReadBytes((Int32)fs.Length);
            }
        }
		            individual.STARTDATE = System.DateTime.Now;
                    individual.STATUS = "Active";
                    individual.LASTUPDATED = System.DateTime.Now;
                    individual.NAME = item.NAME;
                    individual.NAMEEN = item.NAMEEN;
                    individual.BIRTHDATE = item.BIRTHDATE;
                    individual.GENDER = item.GENDER;
                    individual.ORGANISATIONID = Convert.ToInt32(ORGANISATION);
                    individual.DEPARTMENTID = DEPARTMENT;
                    individual.SECTIONID = SECTIONID;
                    individual.MARITALSTATUS = item.MARITALSTATUS;
                    individual.RELATIONID = item.RELATIONID;
                    individual.MAINORGANISATIONID = Convert.ToInt32(MAINORGANISATIONID);
                    individual.NATIONALITY = item.NATIONALITY;
                    individual.OCCUPATIONID = OCCUPATIONID;
                    individual.JOBNUMBER = item.JOBNUMBER;
                    individual.JOBTITLEID = JOBTITLEID;
                    db.Entry(individual).State = EntityState.Modified;
                    db.SaveChanges();
                    db.Entry(individual).State = EntityState.Detached;
	}
                }
                else
                {
                    foreach (var item in model.Individual)
                    {
                        individual.STARTDATE = System.DateTime.Now;
                        individual.STATUS = "Active";
                        individual.LASTUPDATED = System.DateTime.Now;
                        individual.NAME = item.NAME;
                        individual.NAMEEN = item.NAMEEN;
                        individual.BIRTHDATE = item.BIRTHDATE;
                        individual.GENDER = item.GENDER;
                        individual.ORGANISATIONID = Convert.ToInt32(ORGANISATION);
                        individual.DEPARTMENTID = DEPARTMENT;
                        individual.SECTIONID = SECTIONID;
                        individual.MARITALSTATUS = item.MARITALSTATUS;
                        individual.RELATIONID = item.RELATIONID;
                        individual.MAINORGANISATIONID = Convert.ToInt32(MAINORGANISATIONID);
                        individual.NATIONALITY = item.NATIONALITY;
                        individual.OCCUPATIONID = OCCUPATIONID;
                        individual.JOBNUMBER = item.JOBNUMBER;
                        individual.JOBTITLEID = JOBTITLEID;
                        db.Entry(individual).State = EntityState.Modified;
                        db.SaveChanges();
                        db.Entry(individual).State = EntityState.Detached;
                    }    
                }
                    //return RedirectToAction("Index");
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

            if ( model.FamilyData!=null)
            {
                  try
            {
                if (file2 != null)
                {
                    int o = -1; 
                    foreach (var item in model.FamilyData)
                    {
                        INDIVIDUAL individual1 = db.INDIVIDUALS.SingleOrDefault(i => i.ID == item.ID);
                        if (file2[++o]!=null)
                        {
                            using (Stream fs = file2[++o].InputStream)
                            {
                                using (BinaryReader br = new BinaryReader(fs))
                                {
                                    item.PHOTO = br.ReadBytes((Int32)fs.Length);
                                }
                            } 
                        }
                       
                        individual1.STARTDATE = System.DateTime.Now;
                        individual1.STATUS = "Active";
                        individual1.LASTUPDATED = System.DateTime.Now;
                        individual1.NAME = item.NAME;
                        individual1.NAMEEN = item.NAMEEN;
                        individual1.BIRTHDATE = item.BIRTHDATE;
                        individual1.GENDER = item.GENDER;
                        individual1.ORGANISATIONID = Convert.ToInt32(ORGANISATION);
                        individual1.DEPARTMENTID = DEPARTMENT;
                        individual1.SECTIONID = SECTIONID;
                        individual1.MARITALSTATUS = item.MARITALSTATUS;
                        individual1.RELATIONID =item.RELATIONID;
                        individual1.MAINORGANISATIONID = Convert.ToInt32(MAINORGANISATIONID);
                        individual1.NATIONALITY = item.NATIONALITY;
                        individual1.OCCUPATIONID = OCCUPATIONID;
                        individual1.JOBNUMBER = item.JOBNUMBER;
                        individual1.JOBTITLEID = JOBTITLEID;
                        db.Entry(individual1).State = EntityState.Modified;
                        db.SaveChanges();
                        db.Entry(individual1).State = EntityState.Detached;
                    }
                }
                else
                {
                    foreach (var item in model.FamilyData)
                    {
                        INDIVIDUAL individual1 = db.INDIVIDUALS.SingleOrDefault(i => i.ID == item.ID);
                        individual1.STARTDATE = System.DateTime.Now;
                        individual1.STATUS = "Active";
                        individual1.LASTUPDATED = System.DateTime.Now;
                        individual1.NAME = item.NAME;
                        individual1.NAMEEN = item.NAMEEN;
                        individual1.BIRTHDATE = item.BIRTHDATE;
                        individual1.GENDER = item.GENDER;
                        individual1.ORGANISATIONID = Convert.ToInt32(ORGANISATION);
                        individual1.DEPARTMENTID = DEPARTMENT;
                        individual1.SECTIONID = SECTIONID;
                        individual1.MARITALSTATUS = item.MARITALSTATUS;
                        individual1.RELATIONID = item.RELATIONID;
                        individual1.MAINORGANISATIONID = Convert.ToInt32(MAINORGANISATIONID);
                        individual1.NATIONALITY = item.NATIONALITY;
                        individual1.OCCUPATIONID = OCCUPATIONID;
                        individual1.JOBNUMBER = item.JOBNUMBER;
                        individual1.JOBTITLEID = JOBTITLEID;
                        db.Entry(individual1).State = EntityState.Modified;
                        db.SaveChanges();
                        db.Entry(individual1).State = EntityState.Detached;
                    }
                }
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

            }
            else
            {
                TempData["AlertMessage"] = "edit";
                return RedirectToAction("Index");
            }
          
            ViewBag.OCCUPATIONID = new SelectList(db.OCCUPATIONs, "ID", "NAME", individual.OCCUPATIONID);
            ViewBag.MAININDIVIDUALID = new SelectList(db.INDIVIDUALS, "ID", "NAME", individual.MAININDIVIDUALID);
            ViewBag.DEPARTMENTID = new SelectList(db.ORGANIZATIONSTRUCTUREs, "ID", "NAME", individual.DEPARTMENTID);
            ViewBag.MAINORGANISATIONID = new SelectList(db.ORGANIZATIONSTRUCTUREs, "ID", "NAME", individual.MAINORGANISATIONID);
            ViewBag.ORGANISATIONID = new SelectList(db.ORGANIZATIONSTRUCTUREs, "ID", "NAME", individual.ORGANISATIONID);
            ViewBag.SECTIONID = new SelectList(db.ORGANIZATIONSTRUCTUREs, "ID", "NAME", individual.SECTIONID);
            ViewBag.RELATIONID = new SelectList(db.RELATIONS, "ID", "NAME", individual.RELATIONID);
            ViewBag.JOBTITLEID = new SelectList(db.JOBTITLES, "ID", "NAME", individual.JOBTITLEID);
            return View(individual);
        }
        public ActionResult RetrieveImage(int id)
        {
            byte[] cover = db.INDIVIDUALS.Find(id).PHOTO;
            if (cover != null)
            {
                return File(cover, "image/jpg");
            }
            else
            {
                return null;
            }
        }
        //
        // GET: /Individual/Delete/5

        public ActionResult Delete(int id = 0)
        {
            VMIndividual model = new VMIndividual();
            var individual = db.INDIVIDUALS.Where(i => i.ID == id).ToList();
            List<INDIVIDUAL> individual1 = db.INDIVIDUALS.Where(o => o.MAININDIVIDUALID == id).ToList();
            model.FamilyData = individual1;
            model.Individual = individual;
            if (individual == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        //
        // POST: /Individual/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                List<INDIVIDUAL> DeleteFamily = db.INDIVIDUALS.Where(i => i.MAININDIVIDUALID == id).ToList();
                INDIVIDUAL individual = db.INDIVIDUALS.Find(id);
                db.INDIVIDUALS.Remove(individual);
                db.SaveChanges();
                db.Entry(individual).State = EntityState.Detached;
               
                foreach (var item in DeleteFamily)
                {
                    db.INDIVIDUALS.Remove(item);
                    db.SaveChanges();
                }
                TempData["AlertMessage"] = "deleted";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                
                throw;
            }
            return View();
           
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}