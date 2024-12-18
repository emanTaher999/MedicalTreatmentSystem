﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MedicalTreatment.Models;
using PagedList;

namespace MedicalTreatment.Controllers
{
    public class OrganizationProfileController : Controller
    {
        private Entities db = new Entities();

        //
        // GET: /OrganizationProfile/
        public ActionResult Index(string search, int? x)
        {
            var list = db.ORGANIZATIONPROFILEs.Where(i => i.STATUS == "Active" && i.ADDRESS.StartsWith(search) || search == null).ToList().ToPagedList(x ?? 1, 10);
            return View(list);
            //var organizationprofiles = db.ORGANIZATIONPROFILEs.Include(o => o.ORGANIZATIONSTRUCTURE);
            //return View(organizationprofiles.ToList());
        }

        //
        // GET: /OrganizationProfile/Details/5
        public ActionResult Details(short id = 0)
        {
            ORGANIZATIONPROFILE organizationprofile = db.ORGANIZATIONPROFILEs.Find(id);
            if (organizationprofile == null)
            {
                return HttpNotFound();
            }
            return View(organizationprofile);
        }

        //
        // GET: /OrganizationProfile/Create
        public ActionResult Create()
        {
            ViewBag.ORGANIZATIONID = new SelectList(db.ORGANIZATIONSTRUCTUREs, "ID", "NAME");
            return View();
        }

        //
        // POST: /OrganizationProfile/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ORGANIZATIONPROFILE organizationprofile
            , HttpPostedFileBase file, HttpPostedFileBase file1)
        {
            if (file!=null)
            {
                using (Stream fs = file.InputStream)
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        organizationprofile.LOGO = br.ReadBytes((Int32)fs.Length);

                    }
                }
            }
            if (file1!=null)
            {
                using (Stream fs = file1.InputStream)
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        organizationprofile.FORMLOGO = br.ReadBytes((Int32)fs.Length);
                    }
                }
            }
           
            if (ModelState.IsValid)
            {
                organizationprofile.STATUS = "Active";
                organizationprofile.LASTUPDATED = System.DateTime.Now;
                db.ORGANIZATIONPROFILEs.Add(organizationprofile);
                db.SaveChanges();
                TempData["AlertMessage"] = "success";
                return RedirectToAction("Index");
            }

            ViewBag.ORGANIZATIONID = new SelectList(db.ORGANIZATIONSTRUCTUREs
                , "ID", "NAME", organizationprofile.ORGANIZATIONID);
            return View(organizationprofile);
        }

        //
        // GET: /OrganizationProfile/Edit/5

        public ActionResult Edit(short id = 0)
        {
            ORGANIZATIONPROFILE organizationprofile = db.ORGANIZATIONPROFILEs.Find(id);
            if (organizationprofile == null)
            {
                return HttpNotFound();
            }
            ViewBag.ORGANIZATIONID = new SelectList(db.ORGANIZATIONSTRUCTUREs
                , "ID", "NAME", organizationprofile.ORGANIZATIONID);
            return View(organizationprofile);
        }

        //
        // POST: /OrganizationProfile/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ORGANIZATIONPROFILE organizationprofile
            , HttpPostedFileBase file, HttpPostedFileBase file1)
        {
            ORGANIZATIONPROFILE update = db.ORGANIZATIONPROFILEs
                .SingleOrDefault(i=>i.ID== organizationprofile.ID);

            if (file != null)
            {
                using (Stream fs = file.InputStream)
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        update.LOGO = br.ReadBytes((Int32)fs.Length);

                    }
                }
            }
            if (file1 != null)
            {
                using (Stream fs = file1.InputStream)
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        update.FORMLOGO = br.ReadBytes((Int32)fs.Length);

                    }
                }
            }
            if (ModelState.IsValid)
            {
                update.ORGANIZATIONID = organizationprofile.ORGANIZATIONID;
                update.ADDRESS = organizationprofile.ADDRESS;
                update.STATUS = "Active";
                update.LASTUPDATED = DateTime.Now;
                db.Entry(update).State = EntityState.Modified;
                db.SaveChanges();
                TempData["AlertMessage"] = "edit";
                return RedirectToAction("Index");
            }
            ViewBag.ORGANIZATIONID = new SelectList(db.ORGANIZATIONSTRUCTUREs
                , "ID", "NAME", organizationprofile.ORGANIZATIONID);
            return View(organizationprofile);
        }

        //
        // GET: /OrganizationProfile/Delete/5
        public ActionResult Delete(short id = 0)
        {
            ORGANIZATIONPROFILE organizationprofile = db.ORGANIZATIONPROFILEs.Find(id);
            if (organizationprofile == null)
            {
                return HttpNotFound();
            }
            return View(organizationprofile);
        }

        // GET: /OrganizationProfile/RetrieveImage/5
        public ActionResult RetrieveImage(int id)
        {
            byte[] cover = db.ORGANIZATIONPROFILEs.Find(id).LOGO;

            if (cover != null)
            {
                return File(cover, "image/jpg");
            }
            else
            {
                return null;
            }
        }

        // GET: /OrganizationProfile/RetrieveFromLogo/5
        public ActionResult RetrieveFromLogo(int id)
        {
            byte[] cover = db.ORGANIZATIONPROFILEs.Find(id).FORMLOGO;
       
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
        // POST: /OrganizationProfile/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            ORGANIZATIONPROFILE organizationprofile = db.ORGANIZATIONPROFILEs.Find(id);
            db.ORGANIZATIONPROFILEs.Remove(organizationprofile);
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