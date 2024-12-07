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
    //public class MedicalClaimFormVM
    //{
    //    public List<MEDICALCLAIMFORM> medForm;
    //    public List<MEDICALCLAIMFORMDETAIL> medFormDet;
     

    //}
    public class MedicalClaimFormDetailVM
    {

        public decimal ID { get; set; }
        public int SERVICEFEESTYPEID { get; set; }
        public decimal AMOUNT { get; set; }
        public int UNITID { get; set; }
        public decimal NUMBEROFUNITS { get; set; }
        public decimal TOTALAMOUNT { get; set; }
        public decimal ENTITLEMENTPERCENT { get; set; }
        public decimal ENTITLEMENTAMOUNT { get; set; }
        public System.DateTime LASTUPDATED { get; set; }


    }

    public class MedicalClaimFormController : Controller
    {
        private Entities db = new Entities();

     
        //
        // GET: /MedicalClaimForm/

        public ActionResult Index(string search, int? x)
        {
            var list = db.MEDICALCLAIMFORMs.Where(i => i.STATUS == "Entered" && i.INDIVIDUAL.NAME == search || search == null).ToList().ToPagedList(x ?? 1, 10);

           // var medicalclaimforms = db.MEDICALCLAIMFORMs.Include(m => m.INDIVIDUAL).Include(m => m.ORGANIZATIONSTRUCTURE).Include(m => m.ORGANIZATIONSTRUCTURE1).Include(m => m.ORGANIZATIONSTRUCTURE2).Include(m => m.ORGANIZATIONSTRUCTURE3).Include(m => m.MEDICALDETAILSERVICE).Include(m => m.MEDICALSERVICE);
            return View(list);
        }

        public ActionResult ApproveIndex(string search, int? x)
        {
            var list = db.MEDICALCLAIMFORMs.Where(i => i.STATUS == "Entered" ).ToList().ToPagedList(x ?? 1, 10);

            // var medicalclaimforms = db.MEDICALCLAIMFORMs.Include(m => m.INDIVIDUAL).Include(m => m.ORGANIZATIONSTRUCTURE).Include(m => m.ORGANIZATIONSTRUCTURE1).Include(m => m.ORGANIZATIONSTRUCTURE2).Include(m => m.ORGANIZATIONSTRUCTURE3).Include(m => m.MEDICALDETAILSERVICE).Include(m => m.MEDICALSERVICE);
            return View(list);
        }
        public JsonResult getpartialDiv(int? Number)
        {
            //---------------MEDICALCLAIMFORMDetails Data------------------------------------------------
            ViewBag.MEDICALCLAIMFORMID = new SelectList(db.MEDICALCLAIMFORMs, "ID", "PATIENTEMPLOYEE");
            ViewBag.SERVICEFEESTYPEID = new SelectList(db.SERVICEFEESTYPEs, "ID", "NAME");
            ViewBag.UNITID = new SelectList(db.UNITS, "ID", "NAME");
            ViewData["ENTITLEMENTPERCENT"] = db.COMPANYPOLICies.SingleOrDefault().COVERAGEPERCENT;
           
            //---------------Individual Data------------------------------------------------
            MEDICALCLAIMFORMDETAIL model = new MEDICALCLAIMFORMDETAIL();
            List<MEDICALCLAIMFORMDETAIL> repeat = new List<MEDICALCLAIMFORMDETAIL>();
            for (int i = 0; i < Number; i++)
            {
                repeat.Add(model);
                
            }

            var result = CommonUtils.RenderPartialViewToString(this, "MedicalClaimFormDetail", repeat);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //
        // GET: /MedicalClaimForm/Details/5

        public ActionResult Details(decimal id = 0)
        {
            MEDICALCLAIMFORM medicalclaimform = db.MEDICALCLAIMFORMs.Find(id);
            if (medicalclaimform == null)
            {
                return HttpNotFound();
            }
            return View(medicalclaimform);
        }
        public static List<SelectListItem> getPatientEmployee()
        {
            var selectList = new List<SelectListItem>
                {
                new SelectListItem {Text = "نعم", Value = "Yes"},
                new SelectListItem {Text ="لا", Value = "No"}
            };
            return selectList;
        }

        public static List<SelectListItem> GetStatus()
        {
            var selectList = new List<SelectListItem>
                {
                new SelectListItem {Text = "موافقة", Value = "Approved"},
                new SelectListItem {Text ="رفض", Value = "Rejected"},
                 new SelectListItem {Text ="مدخل", Value = "Entered"},
                

            };
            return selectList;
        }


        public JsonResult GetTOTAL(string AMOUNT, string NUMBEROFUNITS)
        {
            decimal amount = Convert.ToDecimal(AMOUNT);
            int NoOfUnits =int.Parse( NUMBEROFUNITS);

            var TotalAmount = (amount * NoOfUnits);
            if (TotalAmount != null)
            {
                var data = TotalAmount;
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(false, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetTotalPer(string AMOUNT, string NUMBEROFUNITS)
        {
            decimal amount = Convert.ToDecimal(AMOUNT);
            int NoOfUnits = int.Parse(NUMBEROFUNITS);

            var TotalAmountPer = ((amount * NoOfUnits) * 80 / 100);
            if (TotalAmountPer != null)
            {
                var data = TotalAmountPer;
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(false, JsonRequestBehavior.AllowGet);

        }

        //newgetConsumer
        public JsonResult getConsumer(int INDIVIDUALID, short Years)
        {
           // short Year2 = 2022;
            if (INDIVIDUALID != null)
            {
                decimal All = db.GENERALCEILINGs.Where(o => o.YEAR == Years).SingleOrDefault().CEILING;
                var SumAmount = db.MEDICALCLAIMFORMs.Where(x => x.INDIVIDUALID == INDIVIDUALID).Sum(x => x.APPROVEDAMOUNT);

                var result = new Total_data
                {
                    ConsumerCeiling = db.MEDICALCLAIMFORMs.Where(x => x.INDIVIDUALID == INDIVIDUALID).Sum(x => x.APPROVEDAMOUNT),
                    ResidualCeiling = All - SumAmount,

                };

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        //short Year,
        public JsonResult GetConsumerCeiling(int INDIVIDUALID,short Years)
        {
            //short Year2 = 2022;
            //short Year2 = Convert.ToInt16(Years);
            if (INDIVIDUALID != null)
            {
                decimal All = db.GENERALCEILINGs.SingleOrDefault(o => o.YEAR == Years).CEILING;
                var SumAmount = db.MEDICALCLAIMFORMs.Where(x => x.INDIVIDUALID == INDIVIDUALID).Sum(x => x.APPROVEDAMOUNT);
                  //var  ResidualCeiling = All - SumAmount;
                  var ConsumerCeiling = SumAmount;
                  var data = ConsumerCeiling;
               

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
            
        }

        public JsonResult GetResidualCeiling(int INDIVIDUALID, short Years)
        {
            //short Year2 = Convert.ToInt16(Years);
            if (INDIVIDUALID != null)
            {
                decimal All = db.GENERALCEILINGs.Where(o => o.YEAR == Years).SingleOrDefault().CEILING;
                var SumAmount = db.MEDICALCLAIMFORMs.Where(x => x.INDIVIDUALID == INDIVIDUALID).Sum(x => x.APPROVEDAMOUNT);
                var ResidualCeiling = All - SumAmount;
              //  var ConsumerCeiling = SumAmount;
                var data = ResidualCeiling;


                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);

        }
        // GET: /MedicalClaimForm/Create

        public ActionResult Create()
        {
            ViewBag.INDIVIDUALID = new SelectList(db.INDIVIDUALS.Where(i => i.MAININDIVIDUALID == null && i.STATUS == "Active" || i.STATUS == "Terminated"), "ID", "NAME");
            ViewBag.DEPARTMENTID = new SelectList(db.ORGANIZATIONSTRUCTUREs.Where(i=>i.STRUCTURECODEID==3), "ID", "NAME");
            ViewBag.ORGANISATIONID = new SelectList(db.ORGANIZATIONSTRUCTUREs.Where(i => i.STRUCTURECODEID == 2), "ID", "NAME");
            ViewBag.SECTIONID = new SelectList(db.ORGANIZATIONSTRUCTUREs.Where(i=>i.STRUCTURECODEID==4), "ID", "NAME");
            ViewBag.MAINORGANISATIONID = new SelectList(db.ORGANIZATIONSTRUCTUREs.Where(i => i.STRUCTURECODEID == 1), "ID", "NAME");
            ViewBag.MEDICALDETAILSERVICEID = new SelectList(db.MEDICALDETAILSERVICES, "ID", "NAME");
            ViewBag.MEDICALSERVICEID = new SelectList(db.MEDICALSERVICES, "ID", "NAME");
            ViewBag.PATIENTEMPLOYEE = new SelectList(getPatientEmployee(), "Value", "Text");
            ViewBag.PATIENTID = new SelectList(db.INDIVIDUALS, "ID", "NAME");
            ViewBag.FORMYEAR = new SelectList(db.GENERALCEILINGs.Where(o => o.STATUS == "Active"), "YEAR", "YEAR");
            ViewData["ENTITLEMENTPERCENT"] = db.COMPANYPOLICies.SingleOrDefault().COVERAGEPERCENT;
            
           
           // ViewData["CILING"] =db.GENERALCEILINGs.SingleOrDefault().CEILING;
           //ViewData["ENTITLEMENTPERCENT"] =
            

            return View();
        }
        //public decimal? ResidualCeiling { get; set; }
        //public decimal? ConsumerCeiling { get; set; }
        //
        // POST: /MedicalClaimForm/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MEDICALCLAIMFORM medicalclaimform, List<MEDICALCLAIMFORMDETAIL> medicalclaimformdetails)
        {
            //ViewData["ENTITLEMENTPERCENT"] = db.COMPANYPOLICies.SingleOrDefault().COVERAGEPERCENT;
            var x = db.COMPANYPOLICies.SingleOrDefault().COVERAGEPERCENT;
            
           try{
                 var sumTOTALAMOUNT =medicalclaimformdetails.Sum(a=>a.TOTALAMOUNT);
             var sumAPPROVEDAMOUNT =medicalclaimformdetails.Sum(a=>a.APPROVEDAMOUNTDD);
            var claimTOTALAMOUNT=medicalclaimform.TOTALCLAIMAMOUNT;
            var claimapprove=medicalclaimform.APPROVEDAMOUNT;
           
               if(sumTOTALAMOUNT> claimTOTALAMOUNT|| sumAPPROVEDAMOUNT >claimapprove)
               {
                   // TempData["message"] = "Grat";
                   TempData["AlertMessage2"] = "er";
                   //return View();
               }
               else{

              
                    decimal id = 0;
                    if (db.MEDICALCLAIMFORMs.Any())
                        id = db.MEDICALCLAIMFORMs.Max(i => i.ID);
                id += 1;
                medicalclaimform.ID = id;
               medicalclaimform.STATUS = "Entered";
               medicalclaimform.LASTUPDATED = System.DateTime.Now;
               medicalclaimform.ENTERDATE = System.DateTime.Now;
               db.MEDICALCLAIMFORMs.Add(medicalclaimform);
               db.SaveChanges();
               var ClaimId = medicalclaimform.ID;
MEDICALCLAIMFORM md=new MEDICALCLAIMFORM();
md.ID = medicalclaimform.ID;
var x2 = md.ID;

                   foreach (var Med in medicalclaimformdetails)
                   {
                      
                       MEDICALCLAIMFORMDETAIL med = new MEDICALCLAIMFORMDETAIL();
                       
                       med.LASTUPDATED = System.DateTime.Now;
                       med.MEDICALCLAIMFORMID = ClaimId;
                       med.NUMBEROFUNITS = Med.NUMBEROFUNITS;
                       med.TOTALAMOUNT = Med.TOTALAMOUNT;
                       med.UNITID = Med.UNITID;
                       med.AMOUNT = Med.AMOUNT;
                       med.APPROVEDAMOUNTDD = Med.APPROVEDAMOUNTDD;
                       med.ENTITLEMENTAMOUNT = Med.ENTITLEMENTAMOUNT;
                       med.SERVICEFEESTYPEID = Med.SERVICEFEESTYPEID;
                       med.ENTITLEMENTPERCENT =x;
                       db.MEDICALCLAIMFORMDETAILS.Add(med);
                      db.SaveChanges();
                       
                   }
                   TempData["AlertMessage"] = "success";
               
               }

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
           ViewBag.INDIVIDUALID = new SelectList(db.INDIVIDUALS.Where(i => i.MAININDIVIDUALID == null && i.STATUS == "Active" || i.STATUS == "Terminated"), "ID", "NAME", medicalclaimform.INDIVIDUALID);
            ViewBag.DEPARTMENTID = new SelectList(db.ORGANIZATIONSTRUCTUREs, "ID", "NAME", medicalclaimform.DEPARTMENTID);
            ViewBag.ORGANISATIONID = new SelectList(db.ORGANIZATIONSTRUCTUREs, "ID", "NAME", medicalclaimform.ORGANISATIONID);
            ViewBag.SECTIONID = new SelectList(db.ORGANIZATIONSTRUCTUREs, "ID", "NAME", medicalclaimform.SECTIONID);
            ViewBag.MAINORGANISATIONID = new SelectList(db.ORGANIZATIONSTRUCTUREs, "ID", "NAME", medicalclaimform.MAINORGANISATIONID);
            ViewBag.MEDICALDETAILSERVICEID = new SelectList(db.MEDICALDETAILSERVICES, "ID", "NAME", medicalclaimform.MEDICALDETAILSERVICEID);
            ViewBag.MEDICALSERVICEID = new SelectList(db.MEDICALSERVICES, "ID", "NAME", medicalclaimform.MEDICALSERVICEID);
            ViewBag.PATIENTEMPLOYEE = new SelectList(getPatientEmployee(), "Value", "Text", medicalclaimform.PATIENTEMPLOYEE);
            ViewBag.PATIENTID = new SelectList(db.INDIVIDUALS, "ID", "NAME", medicalclaimform.PATIENTID);
            ViewBag.FORMYEAR = new SelectList(db.GENERALCEILINGs.Where(o => o.STATUS == "Active"), "YEAR", "YEAR", medicalclaimform.FORMYEAR);

            ViewData["ENTITLEMENTPERCENT"] = db.COMPANYPOLICies.SingleOrDefault().COVERAGEPERCENT;
            ViewBag.MEDICALCLAIMFORMID = new SelectList(db.MEDICALCLAIMFORMs, "ID", "PATIENTEMPLOYEE");
            ViewBag.SERVICEFEESTYPEID = new SelectList(db.SERVICEFEESTYPEs, "ID", "NAME");
            ViewBag.UNITID = new SelectList(db.UNITS, "ID", "NAME");
          

            

            
            return View(medicalclaimform);
        }
      public class struc_data{
       public int? MAINORGANISATIONID { get; set; }
       public int? ORGANISATIONID { get; set; }
       public int? DEPARTMENTID { get; set; }
       public int? SECTIONID { get; set; }
         
        }
      public class Total_data
      {
          public decimal? ResidualCeiling { get; set; }
          public decimal? ConsumerCeiling { get; set; }
      }

      public JsonResult CheckMedicalService(int MEDICALSERVICEID, string PATIENTEMPLOYEE,int INDIVIDUALID)
      {
          try
          {
              string data = "";

              var med = db.MEDICALSERVICES.Where(m => m.ID == MEDICALSERVICEID && m.SERVICEINCLUDE == "EmployeeOnly").ToList();
                if( PATIENTEMPLOYEE =="No" && med.Count>=1)
                  {
                    var end=db.INDIVIDUALS.Where(o=>o.ID==INDIVIDUALID).SingleOrDefault();
                   var app = db.MEDICALSERVICES.Where(m => m.ID == MEDICALSERVICEID ).SingleOrDefault();
                   var x = app.APPLICABLEFOR != "Both";

                   if (app.APPLICABLEFOR != "Both")
                    {
                       
                        if (end.GENDER != app.APPLICABLEFOR)
                        {
                            data = "Exist2";
                            return Json(data, JsonRequestBehavior.AllowGet);
                        }   
                    }
                       data = "Exist";
                      return Json(data , JsonRequestBehavior.AllowGet);
                  }
                  else if (med.Count<1)
                  {
                       data = "notExist";
                      return Json(data, JsonRequestBehavior.AllowGet);
                  }
                return Json("", JsonRequestBehavior.AllowGet);

              }         
          catch (Exception ex)
          {
              return Json("", JsonRequestBehavior.AllowGet);
          }

      }

        public JsonResult getStruc(int INDIVIDUALID)
        {
            if (INDIVIDUALID != null)
            {
                var emp = db.INDIVIDUALS.Find(INDIVIDUALID);
                var result =new struc_data
                {
                    MAINORGANISATIONID = db.ORGANIZATIONSTRUCTUREs.SingleOrDefault(a => a.ID == emp.MAINORGANISATIONID).ID,
                    ORGANISATIONID = db.ORGANIZATIONSTRUCTUREs.SingleOrDefault(a => a.ID == emp.ORGANISATIONID).ID,
                    DEPARTMENTID = db.ORGANIZATIONSTRUCTUREs.SingleOrDefault(a => a.ID == emp.DEPARTMENTID).ID,
                    SECTIONID = db.ORGANIZATIONSTRUCTUREs.SingleOrDefault(a => a.ID == emp.SECTIONID).ID,
          
            };
            
             return Json(result, JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }
        //CheckTotalApprove
        public JsonResult CheckTotalApprove(int ResidualCeiling,int APPROVEDAMOUNT)
        {
            if (ResidualCeiling == 0 || ResidualCeiling==null)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            if (APPROVEDAMOUNT > ResidualCeiling)
            {
                var meg = "";
                meg = "المبلغ المصدق أكبر من المتبقى لجملة سقف الموظف";
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            else
                return Json(false, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckApproveTotalDET(int TOTALAMOUNTD, int APPROVEDAMOUNTDD)
        {
            
            if (APPROVEDAMOUNTDD > TOTALAMOUNTD)
            {
                var meg = "";
                meg = "المبلغ المصدق أكبر من المتبقى لجملة سقف الموظف";
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            else
                return Json(false, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getMEDICALDETAILSERVICEID(int MEDICALSERVICEID)
        {

            var name = db.MEDICALDETAILSERVICES.Where(o => o.MEDICALSERVICEID == MEDICALSERVICEID).ToList();


            if (name != null)
            {

                var data = new SelectList(name, "ID", "NAME");
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(false, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getSponsors(int INDIVIDUALID)
        {

            var name = db.INDIVIDUALS.Where(o => o.STATUS == "Active" && o.MAININDIVIDUALID == INDIVIDUALID).ToList();


            if (name != null)
            {

                var data = new SelectList(name, "ID", "NAME");
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(false, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getEmpName(int INDIVIDUALID)
        {
           
            var name = db.INDIVIDUALS.Where(o => o.STATUS == "Active" && o.ID == INDIVIDUALID && o.MAININDIVIDUALID == null).ToList();


            if (name != null)
            {
               
                var data = new SelectList(name, "ID", "NAME");
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(false, JsonRequestBehavior.AllowGet);
        }
       
        public ActionResult Edit(decimal id = 0)
        {
            VMMedicalClaimForm model = new VMMedicalClaimForm();
            MEDICALCLAIMFORM medicalclaimform = db.MEDICALCLAIMFORMs.Find(id);

            List<MEDICALCLAIMFORMDETAIL> MedCal = db.MEDICALCLAIMFORMDETAILS.Where(o => o.MEDICALCLAIMFORMID == id).ToList();
            var med = db.MEDICALCLAIMFORMs.Find(id);
            var med1 = db.MEDICALCLAIMFORMDETAILS.Where(x => x.MEDICALCLAIMFORMID == id).ToList();
            model.medForm = medicalclaimform;
            model.medFormDet = MedCal;
            //model.ResidualCeiling=
            //decimal All = db.GENERALCEILINGs.Where(o => o.YEAR == Years).SingleOrDefault().CEILING;
            //var SumAmount = db.MEDICALCLAIMFORMs.Where(x => x.INDIVIDUALID == INDIVIDUALID).Sum(x => x.APPROVEDAMOUNT);
            //var ResidualCeiling = All - SumAmount;

            if (medicalclaimform == null)
            {
                return HttpNotFound();
            }

            ViewBag.INDIVIDUALID = new SelectList(db.INDIVIDUALS, "ID", "NAME", medicalclaimform.INDIVIDUALID);
            ViewBag.DEPARTMENTID = new SelectList(db.ORGANIZATIONSTRUCTUREs, "ID", "NAME", medicalclaimform.DEPARTMENTID);
            ViewBag.ORGANISATIONID = new SelectList(db.ORGANIZATIONSTRUCTUREs, "ID", "NAME", medicalclaimform.ORGANISATIONID);
            ViewBag.SECTIONID = new SelectList(db.ORGANIZATIONSTRUCTUREs, "ID", "NAME", medicalclaimform.SECTIONID);
                ViewBag.MAINORGANISATIONID = new SelectList(db.ORGANIZATIONSTRUCTUREs, "ID", "NAME", medicalclaimform.MAINORGANISATIONID);
                ViewBag.MEDICALDETAILSERVICEID = new SelectList(db.MEDICALDETAILSERVICES, "ID", "NAME", medicalclaimform.MEDICALDETAILSERVICEID);
                ViewBag.MEDICALSERVICEID = new SelectList(db.MEDICALSERVICES, "ID", "NAME", medicalclaimform.MEDICALSERVICEID);
                ViewBag.PATIENTEMPLOYEE = new SelectList(getPatientEmployee(), "Value", "Text", medicalclaimform.PATIENTEMPLOYEE);
                ViewBag.PATIENTID = new SelectList(db.INDIVIDUALS, "ID", "NAME", medicalclaimform.PATIENTID);
                ViewBag.FORMYEAR = new SelectList(db.GENERALCEILINGs.Where(o => o.STATUS == "Active"), "YEAR", "YEAR", medicalclaimform.FORMYEAR);
                //ViewData["ResidualCeiling"] = ResidualCeiling;
                //ViewData["ConsumerCeiling"] = ConsumerCeiling;
           
            foreach (var item in MedCal)
            {
                ViewBag.MEDICALCLAIMFORMID = new SelectList(db.MEDICALCLAIMFORMs, "ID", "PATIENTEMPLOYEE",item.MEDICALCLAIMFORMID);
                ViewBag.SERVICEFEESTYPEID = new SelectList(db.SERVICEFEESTYPEs, "ID", "NAME",item.SERVICEFEESTYPEID);
                ViewBag.UNITID = new SelectList(db.UNITS, "ID", "NAME",item.UNITID);
                ViewData["ENTITLEMENTPERCENT"] = db.COMPANYPOLICies.SingleOrDefault().COVERAGEPERCENT;
                ViewData["medlenth"] = model.medFormDet.ToArray().Length;
                
            }

            return View(model);
        }
     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VMMedicalClaimForm model, short FORMYEAR, decimal id, int INDIVIDUALID, int? DEPARTMENTID, int? MAINORGANISATIONID, int? ORGANISATIONID, int? SECTIONID, int? PATIENTID, string PATIENTEMPLOYEE, int SERVICEFEESTYPEID)
        {
      
            MEDICALCLAIMFORM med = db.MEDICALCLAIMFORMs.SingleOrDefault(i => i.ID == id);             
                    try
                    {

                        med.INDIVIDUALID = INDIVIDUALID;
                            med.LASTUPDATED = System.DateTime.Now;
                            
                            med.APPROVEDAMOUNT = model.medForm.APPROVEDAMOUNT;
                            med.APPROVEDDATE = model.medForm.APPROVEDDATE;
                            med.CLAIMDATE = model.medForm.CLAIMDATE;
                            med.CLAIMNOTE = model.medForm.CLAIMNOTE;
                            med.DEPARTMENTID = model.medForm.DEPARTMENTID;
                            med.EMPLOYEESERVICEUNITDATE = model.medForm.EMPLOYEESERVICEUNITDATE;
                            med.EMPLOYEESERVICEUNITNOTE = model.medForm.EMPLOYEESERVICEUNITNOTE;
                            med.ENTERDATE = model.medForm.ENTERDATE;
                            med.ENTITLEMENTAMOUNT = model.medForm.ENTITLEMENTAMOUNT;
                            med.HUMANRESOURCESNOTE = model.medForm.HUMANRESOURCESNOTE;
                            med.PATIENTID = Convert.ToInt32(PATIENTID);
                            med.PATIENTEMPLOYEE = PATIENTEMPLOYEE;
                            med.MAINORGANISATIONID = Convert.ToInt32(MAINORGANISATIONID);
                            med.SECTIONID = SECTIONID;
                            med.DEPARTMENTID = DEPARTMENTID;
                            med.ORGANISATIONID = Convert.ToInt32(ORGANISATIONID);
                            med.FORMYEAR = FORMYEAR;

                            //ViewData["ResidualCeiling"] = ResidualCeiling;
                            //ViewData["ConsumerCeiling"] = ConsumerCeiling;
                            db.Entry(med).State = EntityState.Modified;
                            db.SaveChanges();
                      

                        //foreach (var item in model.medFormDet)
                        ViewData["medlenth"]= model.medFormDet.ToArray().Length;
                            for (int i = 0; i < model.medFormDet.ToArray().Length; i++)     
                        {
                            var ss = model.medFormDet[i].ID;
                            MEDICALCLAIMFORMDETAIL med1 = db.MEDICALCLAIMFORMDETAILS.SingleOrDefault(x => x.ID == ss && x.MEDICALCLAIMFORMID == id);

                            med1.LASTUPDATED = System.DateTime.Now;
                            med1.NUMBEROFUNITS = model.medFormDet[i].NUMBEROFUNITS;
                            med1.SERVICEFEESTYPEID = model.medFormDet[i].SERVICEFEESTYPEID;
                            med1.TOTALAMOUNT = model.medFormDet[i].TOTALAMOUNT;
                            med1.AMOUNT = model.medFormDet[i].AMOUNT;
                            med1.APPROVEDAMOUNTDD = model.medFormDet[i].APPROVEDAMOUNTDD;
                            med1.ENTITLEMENTAMOUNT = model.medFormDet[i].ENTITLEMENTAMOUNT;
                            med1.ENTITLEMENTPERCENT = model.medFormDet[i].ENTITLEMENTPERCENT;

                            db.Entry(med1).State = EntityState.Modified;
                            db.SaveChanges();
                            TempData["AlertMessage"] = "edit";

                            ViewBag.MEDICALCLAIMFORMID = new SelectList(db.MEDICALCLAIMFORMs, "ID", "PATIENTEMPLOYEE", med1.MEDICALCLAIMFORMID);
                            ViewBag.SERVICEFEESTYPEID = new SelectList(db.SERVICEFEESTYPEs, "ID", "NAME",med1.SERVICEFEESTYPEID);
                            ViewBag.UNITID = new SelectList(db.UNITS, "ID", "NAME",med1.UNITID);
                            //ViewData["ENTITLEMENTPERCENT"] = db.COMPANYPOLICies.SingleOrDefault().COVERAGEPERCENT;

             
            }

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
                    return RedirectToAction("Index");
            //if (model.medFormDet != null)
            //{
            //    try
            //    {
            //        foreach (var item in model.medFormDet)
            //        {
            //        MEDICALCLAIMFORMDETAIL med1 = db.MEDICALCLAIMFORMDETAILS.SingleOrDefault(i => i.ID == item.ID && i.MEDICALCLAIMFORMID == med.ID);

            //            med1.LASTUPDATED = System.DateTime.Now;
            //            med1.NUMBEROFUNITS = item.NUMBEROFUNITS;
            //            med1.SERVICEFEESTYPEID = item.SERVICEFEESTYPEID;
            //            med1.TOTALAMOUNT = item.TOTALAMOUNT;
            //            med1.AMOUNT = item.AMOUNT;
            //            med1.APPROVEDAMOUNT = item.APPROVEDAMOUNT;
            //            med1.ENTITLEMENTAMOUNT = item.ENTITLEMENTAMOUNT;
            //            med1.ENTITLEMENTPERCENT = item.ENTITLEMENTPERCENT;

            //            db.Entry(med1).State = EntityState.Modified;
            //            db.SaveChanges();

            //            ViewBag.SERVICEFEESTYPEID = new SelectList(db.SERVICEFEESTYPEs, "ID", "NAME", med1.SERVICEFEESTYPEID);
            //            ViewBag.UNITID = new SelectList(db.UNITS, "ID", "NAME",med1.UNITID);

            //        }
            //        TempData["AlertMessage"] = "edit";
            //        return RedirectToAction("Index");
                   
            //    }
            //    catch (DbEntityValidationException e)
            //    {
            //        foreach (var eve in e.EntityValidationErrors)
            //        {
            //            Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
            //                eve.Entry.Entity.GetType().Name, eve.Entry.State);
            //            foreach (var ve in eve.ValidationErrors)
            //            {
            //                Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
            //                    ve.PropertyName, ve.ErrorMessage);
            //            }
            //        }
            //        throw;

            //    }

            //}


                ViewBag.INDIVIDUALID = new SelectList(db.INDIVIDUALS, "ID", "NAME", med.INDIVIDUALID);
                ViewBag.DEPARTMENTID = new SelectList(db.ORGANIZATIONSTRUCTUREs, "ID", "NAME", med.DEPARTMENTID);
                ViewBag.ORGANISATIONID = new SelectList(db.ORGANIZATIONSTRUCTUREs, "ID", "NAME", med.ORGANISATIONID);
                ViewBag.SECTIONID = new SelectList(db.ORGANIZATIONSTRUCTUREs, "ID", "NAME", med.SECTIONID);
                ViewBag.MAINORGANISATIONID = new SelectList(db.ORGANIZATIONSTRUCTUREs, "ID", "NAME", med.MAINORGANISATIONID);
                ViewBag.MEDICALDETAILSERVICEID = new SelectList(db.MEDICALDETAILSERVICES, "ID", "NAME", med.MEDICALDETAILSERVICEID);
                ViewBag.MEDICALSERVICEID = new SelectList(db.MEDICALSERVICES, "ID", "NAME", med.MEDICALSERVICEID);
                ViewBag.FORMYEAR = new SelectList(db.GENERALCEILINGs.Where(o => o.STATUS == "Active"), "YEAR", "YEAR", med.FORMYEAR);
                ViewBag.PATIENTEMPLOYEE = new SelectList(db.MEDICALCLAIMFORMs, "ID", "PATIENTEMPLOYEE", med.PATIENTEMPLOYEE);
                ViewBag.PATIENTID = new SelectList(db.INDIVIDUALS, "ID", "NAME", med.PATIENTID);
                return View(med);
            }

        public ActionResult Approve(decimal id = 0)
        {
            VMMedicalClaimForm model = new VMMedicalClaimForm();
            MEDICALCLAIMFORM medicalclaimform = db.MEDICALCLAIMFORMs.Find(id);

            List<MEDICALCLAIMFORMDETAIL> MedCal = db.MEDICALCLAIMFORMDETAILS.Where(o => o.MEDICALCLAIMFORMID == id).ToList();
            var med = db.MEDICALCLAIMFORMs.Find(id);
            var med1 = db.MEDICALCLAIMFORMDETAILS.Where(x => x.MEDICALCLAIMFORMID == id).ToList();
            model.medForm = medicalclaimform;
            model.medFormDet = MedCal;

            if (medicalclaimform == null)
            {
                return HttpNotFound();
            }

            ViewBag.INDIVIDUALID = new SelectList(db.INDIVIDUALS, "ID", "NAME", medicalclaimform.INDIVIDUALID);
            ViewBag.DEPARTMENTID = new SelectList(db.ORGANIZATIONSTRUCTUREs, "ID", "NAME", medicalclaimform.DEPARTMENTID);
            ViewBag.ORGANISATIONID = new SelectList(db.ORGANIZATIONSTRUCTUREs, "ID", "NAME", medicalclaimform.ORGANISATIONID);
            ViewBag.SECTIONID = new SelectList(db.ORGANIZATIONSTRUCTUREs, "ID", "NAME", medicalclaimform.SECTIONID);
            ViewBag.MAINORGANISATIONID = new SelectList(db.ORGANIZATIONSTRUCTUREs, "ID", "NAME", medicalclaimform.MAINORGANISATIONID);
            ViewBag.MEDICALDETAILSERVICEID = new SelectList(db.MEDICALDETAILSERVICES, "ID", "NAME", medicalclaimform.MEDICALDETAILSERVICEID);
            ViewBag.MEDICALSERVICEID = new SelectList(db.MEDICALSERVICES, "ID", "NAME", medicalclaimform.MEDICALSERVICEID);
            ViewBag.PATIENTEMPLOYEE = new SelectList(getPatientEmployee(), "Value", "Text", medicalclaimform.PATIENTEMPLOYEE);
            ViewBag.PATIENTID = new SelectList(db.INDIVIDUALS, "ID", "NAME", medicalclaimform.PATIENTID);
            ViewBag.FORMYEAR = new SelectList(db.GENERALCEILINGs.Where(o => o.STATUS == "Active"), "YEAR", "YEAR", medicalclaimform.FORMYEAR);

            ViewBag.Status = new SelectList(GetStatus(), "Value", "Text", medicalclaimform.STATUS);
            //ViewData["ResidualCeiling"] = ResidualCeiling;
            //ViewData["ConsumerCeiling"] = ConsumerCeiling;

            foreach (var item in MedCal)
            {
                ViewBag.MEDICALCLAIMFORMID = new SelectList(db.MEDICALCLAIMFORMs, "ID", "PATIENTEMPLOYEE", item.MEDICALCLAIMFORMID);
                ViewBag.SERVICEFEESTYPEID = new SelectList(db.SERVICEFEESTYPEs, "ID", "NAME", item.SERVICEFEESTYPEID);
                ViewBag.UNITID = new SelectList(db.UNITS, "ID", "NAME", item.UNITID);
                ViewData["ENTITLEMENTPERCENT"] = db.COMPANYPOLICies.SingleOrDefault().COVERAGEPERCENT;
                ViewData["medlenth"] = model.medFormDet.ToArray().Length;

            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Approve(VMMedicalClaimForm model, decimal id, int INDIVIDUALID, int? DEPARTMENTID, int? MAINORGANISATIONID, int? ORGANISATIONID, int? SECTIONID, int? PATIENTID, string PATIENTEMPLOYEE, int SERVICEFEESTYPEID,string STATUS)
        {

            MEDICALCLAIMFORM med = db.MEDICALCLAIMFORMs.SingleOrDefault(i => i.ID == id);
            try
            {

                med.INDIVIDUALID = INDIVIDUALID;
                med.LASTUPDATED = System.DateTime.Now;
                med.STATUS = STATUS;
                med.APPROVEDAMOUNT = model.medForm.APPROVEDAMOUNT;
                med.APPROVEDDATE = model.medForm.APPROVEDDATE;
                med.CLAIMDATE = model.medForm.CLAIMDATE;
                med.CLAIMNOTE = model.medForm.CLAIMNOTE;
                med.DEPARTMENTID = model.medForm.DEPARTMENTID;
                med.EMPLOYEESERVICEUNITDATE = model.medForm.EMPLOYEESERVICEUNITDATE;
                med.EMPLOYEESERVICEUNITNOTE = model.medForm.EMPLOYEESERVICEUNITNOTE;
                med.ENTERDATE = model.medForm.ENTERDATE;
                med.ENTITLEMENTAMOUNT = model.medForm.ENTITLEMENTAMOUNT;
                med.HUMANRESOURCESNOTE = model.medForm.HUMANRESOURCESNOTE;
                med.PATIENTID = Convert.ToInt32(model.medForm.PATIENTID);
                med.PATIENTEMPLOYEE = PATIENTEMPLOYEE;
                med.MAINORGANISATIONID = Convert.ToInt32(MAINORGANISATIONID);
                med.SECTIONID = SECTIONID;
                med.DEPARTMENTID = DEPARTMENTID;
                med.ORGANISATIONID = Convert.ToInt32(ORGANISATIONID);

                //ViewData["ResidualCeiling"] = ResidualCeiling;
                //ViewData["ConsumerCeiling"] = ConsumerCeiling;
                db.Entry(med).State = EntityState.Modified;
                db.SaveChanges();


                //foreach (var item in model.medFormDet)
                ViewData["medlenth"] = model.medFormDet.ToArray().Length;
                for (int i = 0; i < model.medFormDet.ToArray().Length; i++)
                {
                    var ss = model.medFormDet[i].ID;
                    MEDICALCLAIMFORMDETAIL med1 = db.MEDICALCLAIMFORMDETAILS.SingleOrDefault(x => x.ID == ss && x.MEDICALCLAIMFORMID == id);

                    med1.LASTUPDATED = System.DateTime.Now;
                    med1.NUMBEROFUNITS = model.medFormDet[i].NUMBEROFUNITS;
                    med1.SERVICEFEESTYPEID = model.medFormDet[i].SERVICEFEESTYPEID;
                    med1.TOTALAMOUNT = model.medFormDet[0].TOTALAMOUNT;
                    med1.AMOUNT = model.medFormDet[i].AMOUNT;
                    med1.APPROVEDAMOUNTDD = model.medFormDet[i].APPROVEDAMOUNTDD;
                    med1.ENTITLEMENTAMOUNT = model.medFormDet[i].ENTITLEMENTAMOUNT;
                    med1.ENTITLEMENTPERCENT = model.medFormDet[i].ENTITLEMENTPERCENT;

                    db.Entry(med1).State = EntityState.Modified;
                    db.SaveChanges();
                    if (med.STATUS == "Approved")
                    {
                        TempData["AlertMessage"] = "Approved";
                    }
                    else if (med.STATUS == "Rejected")
                    {
                        TempData["AlertMessage"] = "Rejected";
                    }
                  

                    ViewBag.MEDICALCLAIMFORMID = new SelectList(db.MEDICALCLAIMFORMs, "ID", "PATIENTEMPLOYEE", med1.MEDICALCLAIMFORMID);
                    ViewBag.SERVICEFEESTYPEID = new SelectList(db.SERVICEFEESTYPEs, "ID", "NAME", med1.SERVICEFEESTYPEID);
                    ViewBag.UNITID = new SelectList(db.UNITS, "ID", "NAME", med1.UNITID);
                    ViewData["ENTITLEMENTPERCENT"] = db.COMPANYPOLICies.SingleOrDefault().COVERAGEPERCENT;


                }

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
            return RedirectToAction("ApproveIndex");
            //if (model.medFormDet != null)
            //{
            //    try
            //    {
            //        foreach (var item in model.medFormDet)
            //        {
            //        MEDICALCLAIMFORMDETAIL med1 = db.MEDICALCLAIMFORMDETAILS.SingleOrDefault(i => i.ID == item.ID && i.MEDICALCLAIMFORMID == med.ID);

            //            med1.LASTUPDATED = System.DateTime.Now;
            //            med1.NUMBEROFUNITS = item.NUMBEROFUNITS;
            //            med1.SERVICEFEESTYPEID = item.SERVICEFEESTYPEID;
            //            med1.TOTALAMOUNT = item.TOTALAMOUNT;
            //            med1.AMOUNT = item.AMOUNT;
            //            med1.APPROVEDAMOUNT = item.APPROVEDAMOUNT;
            //            med1.ENTITLEMENTAMOUNT = item.ENTITLEMENTAMOUNT;
            //            med1.ENTITLEMENTPERCENT = item.ENTITLEMENTPERCENT;

            //            db.Entry(med1).State = EntityState.Modified;
            //            db.SaveChanges();

            //            ViewBag.SERVICEFEESTYPEID = new SelectList(db.SERVICEFEESTYPEs, "ID", "NAME", med1.SERVICEFEESTYPEID);
            //            ViewBag.UNITID = new SelectList(db.UNITS, "ID", "NAME",med1.UNITID);

            //        }
            //        TempData["AlertMessage"] = "edit";
            //        return RedirectToAction("Index");

            //    }
            //    catch (DbEntityValidationException e)
            //    {
            //        foreach (var eve in e.EntityValidationErrors)
            //        {
            //            Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
            //                eve.Entry.Entity.GetType().Name, eve.Entry.State);
            //            foreach (var ve in eve.ValidationErrors)
            //            {
            //                Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
            //                    ve.PropertyName, ve.ErrorMessage);
            //            }
            //        }
            //        throw;

            //    }

            //}


            ViewBag.INDIVIDUALID = new SelectList(db.INDIVIDUALS, "ID", "NAME", med.INDIVIDUALID);
            ViewBag.DEPARTMENTID = new SelectList(db.ORGANIZATIONSTRUCTUREs, "ID", "NAME", med.DEPARTMENTID);
            ViewBag.ORGANISATIONID = new SelectList(db.ORGANIZATIONSTRUCTUREs, "ID", "NAME", med.ORGANISATIONID);
            ViewBag.SECTIONID = new SelectList(db.ORGANIZATIONSTRUCTUREs, "ID", "NAME", med.SECTIONID);
            ViewBag.MAINORGANISATIONID = new SelectList(db.ORGANIZATIONSTRUCTUREs, "ID", "NAME", med.MAINORGANISATIONID);
            ViewBag.MEDICALDETAILSERVICEID = new SelectList(db.MEDICALDETAILSERVICES, "ID", "NAME", med.MEDICALDETAILSERVICEID);
            ViewBag.MEDICALSERVICEID = new SelectList(db.MEDICALSERVICES, "ID", "NAME", med.MEDICALSERVICEID);
            ViewBag.Years = new SelectList(db.GENERALCEILINGs.Where(o => o.STATUS == "Active"), "YEAR", "YEAR",med.FORMYEAR);
            ViewBag.PATIENTEMPLOYEE = new SelectList(db.MEDICALCLAIMFORMs, "ID", "PATIENTEMPLOYEE", med.PATIENTEMPLOYEE);
            ViewBag.PATIENTID = new SelectList(db.INDIVIDUALS, "ID", "NAME", med.PATIENTID);
            ViewBag.STATUS = new SelectList(GetStatus(), "Value", "Text", med.STATUS);
            return View(med);
        }

        // GET: /MedicalClaimForm/Delete/5
        public ActionResult Delete(decimal id = 0)
        {
            MEDICALCLAIMFORM medicalclaimform = db.MEDICALCLAIMFORMs.Find(id);
            if (medicalclaimform == null)
            {
                return HttpNotFound();
            }
            return View(medicalclaimform);
        }

        //
        // POST: /MedicalClaimForm/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            MEDICALCLAIMFORM medicalclaimform = db.MEDICALCLAIMFORMs.Find(id);
            db.MEDICALCLAIMFORMs.Remove(medicalclaimform);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}