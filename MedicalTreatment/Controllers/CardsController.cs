﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using PagedList;
using System.Web.Mvc;
using MedicalTreatment.Models;
using System.IO;

namespace MedicalTreatment.Controllers
{
    public class CardsController : Controller
    {
        private Entities db = new Entities();

        //
        // GET: /Cards/

        public ActionResult Index(int? search, int? x)
        {
            var list = db.CARDS.Where(i => i.STATUS == "Active" && i.CARDNUMBER == search || search == null).ToList().ToPagedList(x ?? 1, 10);
            return View(list);
        }

        //
        // GET: /Cards/Details/5

        public ActionResult Details(int id = 0)
        {
            CARD card = db.CARDS.Find(id);
            if (card == null)
            {
                return HttpNotFound();
            }
            return View(card);
        }

        //
        // GET: /Cards/Create

        public ActionResult Create()
        {
            ViewBag.INDIVIDUALID = new SelectList(db.INDIVIDUALS, "ID", "NAME");
            return View();
        }

        //
        // POST: /Cards/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CARD card, HttpPostedFileBase file)
        {
            using (Stream fs = file.InputStream)
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    card.PHOTO = br.ReadBytes((Int32)fs.Length);
                }
            }
            if (ModelState.IsValid)
            {
                card.STATUS = "Active";
                card.LASTUPDATED = DateTime.Now;
                db.CARDS.Add(card);
                db.SaveChanges();
                TempData["AlertMessage"] = "success";
                return RedirectToAction("Index");
            }

            ViewBag.INDIVIDUALID = new SelectList(db.INDIVIDUALS, "ID", "NAME", card.INDIVIDUALID);
            return View(card);
        }

        // GET: /Cards/RetrieveImage/5
        public ActionResult RetrieveImage(int id)
        {
            byte[] cover = db.CARDS.Find(id).PHOTO;
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
        // GET: /Cards/Edit/5

        public ActionResult Edit(int id = 0)
        {
            CARD card = db.CARDS.Find(id);
            if (card == null)
            {
                return HttpNotFound();
            }
            ViewBag.INDIVIDUAL = new SelectList(db.INDIVIDUALS, "ID", "NAME", card.INDIVIDUALID);
            return View(card);
        }

        //
        // GET: /Cards/Print/5

        public ActionResult Print(int id = 0)
        {
            CARD card = db.CARDS.Find(id);
            if (card == null)
            {
                return HttpNotFound();
            }
            //ViewBag.INDIVIDUAL = new SelectList(db.INDIVIDUALS, "ID", "NAME", card.INDIVIDUALID);
            return View(card);
        }

        //
        // POST: /Cards/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CARD card, HttpPostedFileBase file,int? INDIVIDUAL)
        {
            using (Stream fs = file.InputStream)
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    card.PHOTO = br.ReadBytes((Int32)fs.Length);
                }
            }
          
                card.INDIVIDUALID =  (int)INDIVIDUAL;
                card.STATUS = "Active";
                card.LASTUPDATED = DateTime.Now;
                db.Entry(card).State = EntityState.Modified;
                db.SaveChanges();
                TempData["AlertMessage"] = "edit";
                return RedirectToAction("Index");
            
            ViewBag.INDIVIDUAL = new SelectList(db.INDIVIDUALS, "ID", "NAME", card.INDIVIDUALID);
            return View(card);
        }

        //
        // GET: /Cards/Delete/5

        public ActionResult Delete(int id = 0)
        {
            CARD card = db.CARDS.Find(id);
            if (card == null)
            {
                return HttpNotFound();
            }
            return View(card);
        }

        //
        // POST: /Cards/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CARD card = db.CARDS.Find(id);
            db.CARDS.Remove(card);
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