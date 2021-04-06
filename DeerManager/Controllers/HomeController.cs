using DeerManager.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using DeerManager.ViewModels;
using System.Globalization;
using DeerManager.Classes;

namespace DeerManager.Controllers
{
    public class HomeController : Controller
    {
        public bool toggle = false;
        //DB_AccessLayer.DB dblayer = new DB_AccessLayer.DB();
        public ActionResult AdvancedDetails(int id)
        {
            //here you should import the informations from tables:
            using (DBModel db = new DBModel())
            {
                var shpVM = new UserViewModel
                {
                    shpDiseases = db.Diseases.FirstOrDefault(x => x.Id == id),
                    maintblSheeps = db.maintable.FirstOrDefault(x => x.Id == id),
                    shpDetail = db.Details.FirstOrDefault(x => x.Id == id),
                    shpHasraa = db.Hasroot.Where(x=>x.Id==id).ToList<Hasroot>(),
                    shpVac= db.Vaccinations.Where(x=>x.Id==id).ToList<Vaccinations>()
                };
                return View(shpVM);
            }
        }

        [HttpGet]
        public ActionResult errorPage(string err)
        {
            return View((object)err);
        }

        public ActionResult ShowMyHome()
        {
            return View("ShowMyHome");
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            using (DBModel db = new DBModel())
            {
                maintable emp = db.maintable.Where(x => x.Id == id).FirstOrDefault<maintable>();
                if (emp == null) {
                    TempData["error"] = "כבשה לא קיימת";
                    return View("errorPage"); 
                }
                db.maintable.Remove(emp);
                db.SaveChanges();
                return Redirect(Request.UrlReferrer.ToString());
            }
        }

        [HttpGet]
        public ActionResult Details(int id=0)
        {
            if (id ==0)
                return View(new maintable());
            else
            {
                using (DBModel db = new DBModel())
                {
                    return View(db.maintable.Where(x => x.Id == id).FirstOrDefault<maintable>());
                }
            }
        }

        [HttpPost]
        public ActionResult Details(maintable emp)
        {
            using (DBModel db = new DBModel())
            {
                if (emp == null)
                {
                    TempData["error"] = "אתה מנסה למחוק משהו לא קיים";
                    return View("errorPage");
                }
                else if (emp.Birthday == null || emp.Birthday == "")
                {
                    emp.Birthday = "אין תאריך";
                }
                db.Entry(emp).State = EntityState.Modified;
                db.SaveChanges();
                return Redirect(Request.UrlReferrer.ToString());
            }
        }

        [HttpPost]
        public ActionResult AddNewSheep(maintable shp)
        {
            if (ModelState.IsValid)
            {
                using (DBModel db = new DBModel())
                {
                    var check = db.maintable.FirstOrDefault(x => x.Id == shp.Id);
                    if (check != null)
                    {
                        TempData["error"] = "כבשה כבר קיימת במערכת";
                        return View("errorPage");
                    }
                    if (shp.Birthday == null)
                    {
                        shp.Birthday = "אין תאריך";
                    }
                    db.maintable.Add(shp);
                    db.SaveChanges();
                    return View("ShowMyHome");
                }
            }
            return View("errorPage");
        }

        [HttpPost]
        public ActionResult AddNewSheepPage(maintable shp)
        {
            if (ModelState.IsValid)
            {
                using (DBModel db = new DBModel())
                {
                    var check = db.maintable.FirstOrDefault(x => x.Id == shp.Id);
                    if (check != null)
                    {
                        TempData["error"] = "כבשה כבר קיימת";
                        return View("errorPage");
                    }
                    if (shp.Birthday == null)
                    {
                        shp.Birthday = "אין תאריך";
                    }
                    db.maintable.Add(shp);
                    db.SaveChanges();
                    return View("ShowMyHome");
                }
            }
            return View("errorPage");
        }

        //function check weither an id already exists in db.
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult isExists(int id)
        {

            using (DBModel db = new DBModel())
            {
                var check = db.maintable.FirstOrDefault(x => x.Id == id);
                if (check == null)
                {
                    return Json(new { emailSent = "False" });
                }
                else
                {
                    return Json(new { emailSent = "True" });
                }
            }
        }

        [HttpGet]
        public ActionResult AddNewSheepPage()

        {
            return View(new maintable());
        }

        [HttpGet]
        public ActionResult AddNewSheep()
        
        {
            return View(new maintable());
        }

        [HttpGet]
        public ActionResult AddHasraa(int id)
        {
            if (ModelState.IsValid)
            {
                var newHasraa = new Hasroot();
                newHasraa.Id = id;
                return View(newHasraa);
            }
            else {
                TempData["error"] = "לא הצלחנו למצוא את רשומה עבור "+ id;
                return View("errorPage");
            }
        }

        //this function when you remove hasraa , we call this function to remove vacs that related to the same sheep(cuz already removed)
        public void RemoveAutoVac(int shpid, DateTime date)
        {
            using (DBModel db = new DBModel())
            {
                //first we check if there already vacs in same dates in db
                var checkAlready = db.Vaccinations.Where(x => x.Id == shpid).ToList();
                if (checkAlready.Count > 0)
                {
                    for (int i = 0; i < checkAlready.Count; i++)
                    {
                        var dateInDB = DateTime.Parse(checkAlready[i].NextVaccinationDate);
                        if ((date.AddDays(120)==dateInDB && checkAlready[i].Medicine.Replace(" ","").Contains("סימוםמעיים")))
                        {
                            db.Vaccinations.Remove(checkAlready[i]);
                            db.SaveChanges();
                        }
                        if ((date.AddDays(70) == dateInDB && checkAlready[i].Medicine.Replace(" ", "").Contains("אוקסי")))
                        {
                            db.Vaccinations.Remove(checkAlready[i]);
                            db.SaveChanges();
                        }
                        if ((date.AddDays(90) == dateInDB && checkAlready[i].Medicine.Replace(" ", "").Contains("אוקסי")))
                        {
                            db.Vaccinations.Remove(checkAlready[i]);
                            db.SaveChanges();
                        }
                        if ((date.AddDays(110) == dateInDB && checkAlready[i].Medicine.Replace(" ", "").Contains("אוקסי")))
                        {
                            db.Vaccinations.Remove(checkAlready[i]);
                            db.SaveChanges();
                        }
                    }
                }
            }
        }



        //buggged
        //this function get the latest vac of shpid and remove it from db when adding takser to sheep
        public void RemoveLatestVac(int shpid)
        {
            using (DBModel db = new DBModel())
            {
                //first we check if there already vacs in same dates in db
                var lastHamlataDate = db.Hasroot.Where(x => x.Id == shpid).ToList();
                var curdate = DateTime.Now;
                var obj="";
                double min = 99999;
                if (lastHamlataDate.Count > 0)
                {
                    for (int i = 0; i < lastHamlataDate.Count; i++)
                    {
                        var dbDate = DateTime.Parse(lastHamlataDate[i].DateOfHasraa);
                        if ((curdate - dbDate).TotalDays < min)
                        {
                            min = (curdate - dbDate).TotalDays;
                            obj = lastHamlataDate[i].DateOfHasraa;
                        }
                    }
                    RemoveAutoVac(shpid, DateTime.Parse(obj));
                }
            }
        }

        public bool AddAutoVac(int shpid, DateTime date , int days, string medicine)
        {
            using (DBModel db = new DBModel())
            {
                //first we check if there already vacs in same dates in db
                var checkAlready = db.Vaccinations.Where(x => x.Id == shpid).ToList();
                if (checkAlready.Count > 0)
                {
                    for(int i = 0; i < checkAlready.Count; i++)
                    {
                        var dateInDB = DateTime.Parse(checkAlready[i].NextVaccinationDate);
                        if(date==dateInDB)
                        {
                            return false;
                        }
                    }
                }
                /////////////////////////////////////
                var NextVac = new Vaccinations();
                var NextVacDateSimom = new DateTime(); //סימום מעיים
                NextVacDateSimom = date.AddDays(days);
                NextVac.NextVaccinationDate = NextVacDateSimom.ToString();
                NextVac.Medicine = medicine;
                NextVac.Id = shpid;
                NextVac.isEnabled = 0;
                db.Vaccinations.Add(NextVac);
                db.SaveChanges();
                return true;
                }
        }
        [HttpGet]
        //this function receives sheep id and date and update hamlata
        public ActionResult AddSpecificHamlata(int shpid, DateTime date, int amount)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (DBModel db = new DBModel())
                    {
                        var hamlata = new Hamlata();
                        /////////////////////////////////////
                        //we check if there are column in database with already date with same sheep id.
                        var checkAlready = db.Hamlata.Where(x => x.Id == shpid).ToList();
                        //checking if there are already same id and date of hamlata
                        if (checkAlready != null)
                        {
                            for(int i =0; i < checkAlready.Count; i++)
                            {
                                var dbDate = DateTime.Parse(checkAlready[i].dateOfHamlata);
                                if((dbDate - date).Days ==0)
                                {
                                    return Json(new { result = false }, JsonRequestBehavior.AllowGet);
                                }
                            }
                        }
                        hamlata.Id = shpid;
                        hamlata.dateOfHamlata = date.ToString("dd/MM/yyyy HH:mm:ss");
                        hamlata.amount = amount;
                        db.Hamlata.Add(hamlata);
                        db.SaveChanges();
                        return Json(new { result = true }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { result = false }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
        }
        [HttpGet]
        //this function receives sheep id and date and update hamlata
        public ActionResult AddSpecificHasraa(int shpid, DateTime date)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (DBModel db = new DBModel())
                    {
                        var hasraa = new Hasroot();
                        var res = true;
                        /////////////////////////////////////
                        /// check if there are same hasraa date
                        ///
                        /////////////////////////////////////
                        var checkAlready = db.Hasroot.Where(x => x.Id == shpid).ToList();
                        if (checkAlready.Count > 0)
                        {
                            for (int i = 0; i < checkAlready.Count; i++)
                            {
                                var dateInDB = DateTime.Parse(checkAlready[i].DateOfHasraa);
                                if (date == dateInDB)
                                {
                                    return Json(new { result = "Already" }, JsonRequestBehavior.AllowGet);
                                }
                            }
                        }

                        /////////////////////////////////////
                        res =AddAutoVac(shpid, date, 120, "סימום מעיים");
                        if (res == false) { return Json(new { result = false }, JsonRequestBehavior.AllowGet); }
                        res=AddAutoVac(shpid, date, 70, "אוקסי");
                        if (res == false) { return Json(new { result = false }, JsonRequestBehavior.AllowGet); }
                        res =AddAutoVac(shpid, date, 90, "אוקסי");
                        if (res == false) { return Json(new { result = false }, JsonRequestBehavior.AllowGet); }
                        res =AddAutoVac(shpid, date, 110, "אוקסי");
                        if (res == false) { return Json(new { result = false }, JsonRequestBehavior.AllowGet); }
                        /////////////////////////////////////

                        hasraa.Id = shpid;
                        hasraa.DateOfHasraa = date.ToString("dd/MM/yyyy HH:mm:ss");
                        db.Hasroot.Add(hasraa);
                        db.SaveChanges();
                        return Json(new { result = true }, JsonRequestBehavior.AllowGet);
                    }
                }
                else { return Json(new { result = false }, JsonRequestBehavior.AllowGet); }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
        }

        [HttpPost]
        //this function receives sheep id and date and update hamlata
        public bool AddSpecificTakser(int shpid, DateTime date)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (DBModel db = new DBModel())
                    {
                        var takser = new TakserTable();
                        //we check if there are column in database with already date with same sheep id.
                        var checkAlready = db.TakserTable.Where(x => x.Id == shpid).ToList();
                        if (checkAlready.Count > 0)
                        {
                            for (int i = 0; i < checkAlready.Count; i++)
                            {
                                var dateInDB = DateTime.Parse(checkAlready[i].DateOfTakser);
                                if (date == dateInDB)
                                {
                                    return false;
                                }
                            }
                        }
                        takser.Id = shpid;
                        takser.DateOfTakser = date.ToString();
                        RemoveLatestVac(shpid); //when you add takser to sheep you should remove the upcoming vacs of same sheep.
                        db.TakserTable.Add(takser);
                        db.SaveChanges();
                        return true;
                    }
                }
                else { return false; }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
        }


        //this function receives sheep id and date and update hamlata
        public bool AddSpecificVac(int shpid, int group, string med ,string nextdate)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (DBModel db = new DBModel())
                    {
                        var vac = new Vaccinations();
                        //we check if there are column in database with already date with same sheep id.
                        var checkAlready = db.Vaccinations.FirstOrDefault(x => x.Id == shpid && x.Medicine==med);
                        //we remove the old date if exists.
                        if (checkAlready != null)
                        {
                            db.Vaccinations.Remove(checkAlready);
                        }
                        vac.Id = shpid;
                        vac.Medicine = med; 
                        vac.NextVaccinationDate = nextdate;
                        db.Vaccinations.Add(vac);
                        db.SaveChanges();
                        return true;
                    }
                }
                else { return false; }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
        }


        [HttpPost]
        public ActionResult AddHasraa(Hasroot shp)
        {
            if (ModelState.IsValid)
            {
                if (shp == null) { return View("ShowMyHome"); }
                using (DBModel db = new DBModel())
                {
                    db.Hasroot.Add(shp);
                    db.SaveChanges();
                    return RedirectToAction("AdvancedDetails", new { id = shp.Id });
                }
            }
            else { return View("errorPage"); }
        }


        [HttpGet]
        public ActionResult AddVaccine(int id)
        {
            var newVac = new Vaccinations();
            newVac.Id = id;
            return View(newVac);
        }

        [HttpPost]
        public ActionResult AddVaccine(Vaccinations shp)
        {
            if (ModelState.IsValid)
            {
                if (shp == null) { return View("ShowMyHome"); }
                using (DBModel db = new DBModel())
                {
                    shp.isEnabled = 1; //enabling alerts
                    db.Vaccinations.Add(shp);
                    db.SaveChanges();
                    return RedirectToAction("AdvancedDetails", new { id = shp.Id });
                }
            }
            else { return View("errorPage"); }
        }


        [HttpGet]
        public ActionResult AddVaccineGroup(int id)
        {
            return View(id);
        }


        [HttpGet]
        public ActionResult AddMedicine()
        {
            return View(new Medicine());
        }

        [HttpPost]
        public ActionResult AddMedicine(Medicine med)
        {
            if (ModelState.IsValid)
            {
                if (med == null) {
                    TempData["error"] = "לא נקלטה שם תרופה לחיסון";
                    return View("errorPage"); 
                }
                using (DBModel db = new DBModel())
                {
                    if(db.Medicine.Any(md => md.MedName == med.MedName))
                    {
                        TempData["error"] = "תרופה כבר קיימת במערכת";
                        return View("errorPage");
                    }
                    db.Medicine.Add(med);
                    db.SaveChanges();
                    return RedirectToAction("ShowMyHome");
                }
            }
            else { return View("errorPage"); }
        }

        [HttpGet]
        public ActionResult RemoveMedicine()
        {
            using (DBModel db = new DBModel())
            {

                var meds = db.Medicine.ToList();
                return View(meds);
            }
        }

        [HttpPost]
        public ActionResult RemoveMedicine(Medicine med)
        {
            if (ModelState.IsValid)
            {
                if (med == null) {
                    TempData["error"] = "לא נבחרה תרופה נכונה למחיקה";
                    return View("errorPage"); 
                }
                using (DBModel db = new DBModel())
                {
                    if (!db.Medicine.Any(md => md.MedName == med.MedName))
                    {
                        TempData["error"] = "מנסה למחוק תרופה שלא מתאימה";
                        return View("errorPage");
                    }
                    var SpecificMed = db.Medicine.Where(x => x.MedName == med.MedName).FirstOrDefault();
                    db.Medicine.Remove(SpecificMed);
                    db.SaveChanges();
                    return RedirectToAction("ShowMyHome");
                }
            }
            else { return View("errorPage"); }
        }

        [HttpGet]
        public ActionResult RemoveVaccine(int id)
        {
            using (DBModel db = new DBModel())
            {
                var RemoveVac = new List<Vaccinations>();
                RemoveVac = db.Vaccinations.Where(x => x.Id == id).ToList();
                if (RemoveVac == null)
                {
                    return null;
                }
                return View(RemoveVac);
            }
        }

        [HttpPost]
        public ActionResult RemoveVaccine(Vaccinations shp)
        {
            using (DBModel db = new DBModel())
            {
                var SpecificVac = db.Vaccinations.Where(x => x.Id == shp.Id).Where(x=>x.Medicine.Contains(shp.Medicine)).FirstOrDefault();
                if (SpecificVac != null)
                {
                    db.Vaccinations.Remove(SpecificVac);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("AdvancedDetails", new { id = shp.Id });
        }

        [HttpPost]
        public ActionResult RemoveVac(Vaccinations shp)
        {
            using (DBModel db = new DBModel())
            {
                if (shp.Medicine != null)
                {
                    var med = shp.Medicine.Replace(" ", "");
                    var SpecificVac = db.Vaccinations.Where(x => x.Id == shp.Id).Where(x => x.Medicine.Replace(" ", "") == med).FirstOrDefault();
                    if (SpecificVac != null)
                    {
                        db.Vaccinations.Remove(SpecificVac);
                        db.SaveChanges();
                        return Json(new { result = "OK" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(new { result = "ERROR" , id = shp.Id }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult RemoveVacInHistory(int Id, String Medicine)
        {
            using (DBModel db = new DBModel())
            {
                var med2 = db.Vaccinations.Where(x => x.Medicine.Replace(" ", "") == Medicine.Replace(" ",""));
                if (med2 == null)
                {
                    return null;
                }
                var SpecificVac = db.Vaccinations.Where(x => x.Id == Id).FirstOrDefault();
                if (SpecificVac != null)
                {
                    db.Vaccinations.Remove(SpecificVac);
                    db.SaveChanges();
                    return Json(new { result = "OK", id= Id }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { result = "ERROR", id = Id }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult RemoveHistoryOfVacsById(int Id)
        {
            using (DBModel db = new DBModel())
            {
                var vacsList = db.Vaccinations.Where(x => x.Id == Id).ToList();
                if (vacsList.Count < 1) { Json(new { result = "ERROR", id = Id }, JsonRequestBehavior.AllowGet); }
                for(int i=0;i< vacsList.Count; i++)
                {
                    db.Vaccinations.Remove(vacsList[i]);
                    db.SaveChanges();
                }
                return Json(new { result = "SUCCESS", id = Id }, JsonRequestBehavior.AllowGet);
            }
        }
        
        [HttpGet]
        public ActionResult HistoryOfVacsById(int id)
        {
            return View(id);
        }

        [HttpGet]
        public ActionResult GetHistoryOfVacs(int id)
        {
            using (DBModel db = new DBModel())
            {
                var vacs = db.Vaccinations.Where(x => x.Id == id).ToList();
                if (vacs == null)
                {
                    return View();
                }
                return Json(new { data = vacs }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult soonHazraa()
        {
            var curdate = DateTime.Now;
            using (DBModel db = new DBModel())
            {
                var dates = db.Hamlata.ToList();
                var group = new Hamlata();
                var maximum = 0;
                var flag = false;
                var groupOfSheeps = 0;
                if (dates.Count < 1)
                {
                    return Json(new { result = "Null" }, JsonRequestBehavior.AllowGet);
                }
                for (int i = 0; i < dates.Count; i++)
                {
                    var shpdate = DateTime.Parse(dates[i].dateOfHamlata);
                    if ((curdate - shpdate).Days > maximum && (curdate - shpdate).Days >= 40 && (curdate - shpdate).Days <= 60)
                    {
                        maximum = (curdate - shpdate).Days;
                        group.dateOfHamlata = dates[i].dateOfHamlata;
                        groupOfSheeps = GetGroupById(dates[i].Id);
                        flag = true;
                    }
                }
                if (flag == false)
                {
                    return Json(new { flag = false }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { days = maximum, _date = group.dateOfHamlata.ToString(), shpsgroup = groupOfSheeps }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult LaterHazraa()
        {
            var curdate = DateTime.Now;
            using (DBModel db = new DBModel())
            {
                var dates = db.Hamlata.ToList();
                var group = new Hamlata();
                group.dateOfHamlata = "";
                var maximum = 0;
                var groupOfSheeps = 0;
                var flag = false;
                if (dates.Count < 1)
                {
                    return Json(new { result = "Null" }, JsonRequestBehavior.AllowGet);
                }
                for (int i = 0; i < dates.Count; i++)
                {
                    var shpdate = DateTime.Parse(dates[i].dateOfHamlata);
                    if ((curdate - shpdate).Days > maximum && (curdate - shpdate).Days >10 && (curdate - shpdate).Days < 40)
                    {
                        maximum = (curdate - shpdate).Days;
                        group.dateOfHamlata = dates[i].dateOfHamlata;
                        groupOfSheeps = GetGroupById(dates[i].Id);
                        flag = true;
                    }
                }
                if (flag == false)
                {
                    return Json(new { flag = false }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { days = maximum, _date = group.dateOfHamlata.ToString(), shpsgroup = groupOfSheeps }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public ActionResult soonHamlata()
        {
            var curdate = DateTime.Now;
            using (DBModel db = new DBModel())
            {
                var dates = db.Hasroot.ToList();
                var group = new Hasroot();
                var maximum = 0;
                var flag = false;
                var groupOfSheeps = 0;
                if (dates.Count<1)
                {
                    return Json(new { result = "Null" }, JsonRequestBehavior.AllowGet);
                }
                for(int i=0; i < dates.Count; i++)
                {
                    var shpdate= DateTime.Parse(dates[i].DateOfHasraa);
                    if((curdate - shpdate).Days > maximum && (curdate - shpdate).Days >=135 && (curdate-shpdate).Days <=150)
                    {
                        maximum = (curdate - shpdate).Days;
                        group.DateOfHasraa = dates[i].DateOfHasraa;
                        groupOfSheeps = GetGroupById(dates[i].Id);
                        flag = true;
                    }
                }
                if (flag == false)
                {
                  return Json(new { flag=false }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { max = maximum, _date = group.DateOfHasraa.ToString(), shpsgroup= groupOfSheeps , still = 150-maximum }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult LaterHamlata()
        {
            var curdate = DateTime.Now;
            using (DBModel db = new DBModel())
            {
                var dates = db.Hasroot.ToList();
                var group = new Hasroot();
                group.DateOfHasraa = "";
                var maximum = 0;
                var groupOfSheeps = 0;
                var flag = false;
                if (dates.Count<1)
                {
                    return Json(new { result = "Null" }, JsonRequestBehavior.AllowGet);
                }
                for (int i = 0; i < dates.Count; i++)
                {
                    var shpdate = DateTime.Parse(dates[i].DateOfHasraa);
                    if ((curdate - shpdate).Days > maximum && (curdate - shpdate).Days > 90 && (curdate - shpdate).Days < 135)
                    {
                        maximum = (curdate - shpdate).Days;
                        group.DateOfHasraa = dates[i].DateOfHasraa;
                        groupOfSheeps = GetGroupById(dates[i].Id);
                        flag = true;
                    }
                }
                if (flag == false)
                {
                    return Json(new { flag = false }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { max = maximum, _date = group.DateOfHasraa.ToString(), shpsgroup = groupOfSheeps }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult soonVac()
        {
            var curdate = DateTime.Now;
            using (DBModel db = new DBModel())
            {
                var dates = db.Vaccinations.ToList();

                var group = new Vaccinations();
                var flag = false;
                var groupOfSheeps = 0;
                var medicine = "";
                var tilldays = 0;
                var soondays = 0;
                if (dates.Count < 1 || dates == null)
                {
                    return Json(new { result = "Null" }, JsonRequestBehavior.AllowGet);
                }
                for (int i = 0; i < dates.Count; i++)
                {
                    var shpdate = DateTime.Parse(dates[i].NextVaccinationDate);
                    tilldays = (shpdate - curdate).Days;
                    if(dates[i].Medicine.Replace(" ","")=="אוקסי")
                    {
                        if (tilldays <= 10 && tilldays>=0)
                        {
                            group.NextVaccinationDate = dates[i].NextVaccinationDate;
                            groupOfSheeps = GetGroupById(dates[i].Id);
                            flag = true;
                            medicine = dates[i].Medicine;
                            soondays = tilldays;
                        }
                    }
                    else if (dates[i].Medicine.Replace(" ","").Contains("סימוםמעיים"))
                    {
                        if (tilldays <= 10 && tilldays >= 0)
                        {
                            group.NextVaccinationDate = dates[i].NextVaccinationDate;
                            groupOfSheeps = GetGroupById(dates[i].Id);
                            flag = true;
                            medicine = dates[i].Medicine;
                            soondays = tilldays;
                        }
                    }
                    else
                    {
                        if (tilldays <= 10 && tilldays >= 0)
                        {
                            group.NextVaccinationDate = dates[i].NextVaccinationDate;
                            groupOfSheeps = GetGroupById(dates[i].Id);
                            flag = true;
                            medicine = dates[i].Medicine;
                            soondays = tilldays;
                        }
                    }
                }
                if (flag == false)
                {
                    return Json(new { flag = false }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { days= soondays, _date = group.NextVaccinationDate.ToString(), shpsgroup = groupOfSheeps , med =medicine}, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult LaterVac()
        {
            var curdate = DateTime.Now;
            using (DBModel db = new DBModel())
            {
                var dates = db.Vaccinations.ToList();

                var group = new Vaccinations();
                var flag = false;
                var groupOfSheeps = 0;
                var medicine = "";
                var tilldays = 0;
                var latedays = 0;
                if (dates.Count < 1 || dates == null)
                {
                    return Json(new { result = "Null" }, JsonRequestBehavior.AllowGet);
                }
                for (int i = 0; i < dates.Count; i++)
                {
                    var shpdate = DateTime.Parse(dates[i].NextVaccinationDate);
                    tilldays = (shpdate - curdate).Days;
                    if (dates[i].Medicine.Replace(" ", "") == "אוקסי")
                    {
                        if (tilldays <= 60 && tilldays >= 10)
                        {
                            group.NextVaccinationDate = dates[i].NextVaccinationDate;
                            groupOfSheeps = GetGroupById(dates[i].Id);
                            flag = true;
                            medicine = dates[i].Medicine;
                            latedays = tilldays;
                        }
                    }
                    else if (dates[i].Medicine.Replace(" ", "").Contains("סימוםמעיים"))
                    {
                        if (tilldays <= 60 && tilldays >= 10)
                        {
                            group.NextVaccinationDate = dates[i].NextVaccinationDate;
                            groupOfSheeps = GetGroupById(dates[i].Id);
                            flag = true;
                            medicine = dates[i].Medicine;
                            latedays = tilldays;
                        }
                    }
                    else
                    {
                        if (tilldays <= 60 && tilldays >= 10)
                        {
                            group.NextVaccinationDate = dates[i].NextVaccinationDate;
                            groupOfSheeps = GetGroupById(dates[i].Id);
                            flag = true;
                            medicine = dates[i].Medicine;
                            latedays = tilldays;
                        }
                    }
                }
                if (flag == false)
                {
                    return Json(new { _flag = flag }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { days = latedays, _date = group.NextVaccinationDate.ToString(), shpsgroup = groupOfSheeps, med = medicine ,_flag =flag }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult RemoveHasraaInTable(int id, string date)
        {
            using (DBModel db = new DBModel())
            {
                /////////////////////////////////////
                //we check if there are column in database with already date with same sheep id.
                var hasraashp = db.Hasroot.Where(x => x.Id == id).ToList();
                var webdate = DateTime.Parse(date);
                //checking if there are already same id and date of hasraa
                if (hasraashp != null)
                {
                    for (int i = 0; i < hasraashp.Count; i++)
                    {
                        var dbDate = DateTime.Parse(hasraashp[i].DateOfHasraa);
                        if ((dbDate - webdate).Days == 0)
                        {
                            RemoveAutoVac(id, DateTime.Parse(hasraashp[i].DateOfHasraa)); //removing vacs related to this sheep
                            db.Hasroot.Remove(hasraashp[i]);
                            db.SaveChanges();
                            return Json(new { result = true }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            }
        }



        [HttpGet]
        public ActionResult RemoveHasraa(int id)
        {
            using (DBModel db = new DBModel())
            {
                var RemoveHasraa = new Hasroot();
                RemoveHasraa = db.Hasroot.Where(x => x.Id == id).FirstOrDefault();
                if (RemoveHasraa == null)
                {
                    return null;
                }
                return View(RemoveHasraa);
            }
        }

        [HttpPost]
        public ActionResult RemoveHasraa(Hasroot shp)
        {
            using (DBModel db = new DBModel())
            {
                var SpecificHamlata = db.Hasroot.Where(x => x.Id == shp.Id).FirstOrDefault();
                db.Hasroot.Remove(SpecificHamlata);
                db.SaveChanges();
            }
            return RedirectToAction("AdvancedDetails", new { id = shp.Id });
        }

        [HttpPost]
        public ActionResult RemoveHamlata(int id, string date)
        {
            using (DBModel db = new DBModel())
            {
                var SpecificHamlata = db.Hamlata.Where(x => x.Id == id && x.dateOfHamlata == date).FirstOrDefault();
                if(SpecificHamlata == null)
                {
                    return Json(new { emailSent = "ERROR" });
                }
                db.Hamlata.Remove(SpecificHamlata);
                db.SaveChanges();
            }
            return RedirectToAction("AdvancedDetails", new { id = id });
        }

        [HttpGet]
        public ActionResult RemoveTakser(int id, string date)
        {
            using (DBModel db = new DBModel())
            {
                /////////////////////////////////////
                //we check if there are column in database with already date with same sheep id.
                var takserShp = db.TakserTable.Where(x => x.Id == id).ToList();
                var webdate = DateTime.Parse(date);
                //checking if there are already same id and date of hamlata
                if (takserShp != null)
                {
                    for (int i = 0; i < takserShp.Count; i++)
                    {
                        var dbDate = DateTime.Parse(takserShp[i].DateOfTakser);
                        if ((dbDate - webdate).Days == 0)
                        {
                            db.TakserTable.Remove(takserShp[i]);
                            db.SaveChanges();
                            return Json(new { result = true }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult MoveGroupPage()
        {
            using (DBModel db = new DBModel())
            {
                var sheepDetails = db.maintable.ToList();
                return View(sheepDetails);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult isVaced(int id)
        {

            using (DBModel db = new DBModel())
            {
                var vacalert = db.Vaccinations.FirstOrDefault(x => x.Id == id);
                if (vacalert == null)
                {
                    return Json(new { emailSent = "False" });
                }
                else
                {
                    return Json(new { emailSent = "True" });
                }
            }
        }



        [HttpPost]
        [ValidateInput(false)]
        public ActionResult getNextVacDate(int id)
        {

            using (DBModel db = new DBModel())
            {
                var vacalert = db.Vaccinations.FirstOrDefault(x => x.Id == id);
                if (vacalert != null)
                {
                    return Json(new { emailSent = vacalert.NextVaccinationDate });
                }
                else
                {
                    return Json(new { emailSent = "Null" });
                }
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult getMedById(int id)
        {

            using (DBModel db = new DBModel())
            {
                var vacalert = db.Vaccinations.FirstOrDefault(x => x.Id == id);
                if (vacalert == null)
                {
                    return Json(new { emailSent = "Null" });
                }
                else
                {
                    return Json(new { emailSent = vacalert.Medicine });
                }
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult getTillDaysVac(int id)
        {

            using (DBModel db = new DBModel())
            {
                var vacalert = db.Vaccinations.FirstOrDefault(x => x.Id == id);
                if (vacalert == null)
                {
                    return Json(new { emailSent = "Null" });
                }
                else
                {
                    var curdate = DateTime.Now;
                    var shpvacdate = DateTime.Parse(vacalert.NextVaccinationDate);
                    return Json(new { emailSent = (shpvacdate - curdate).Days.ToString() });
                }
            }
        }


        [HttpGet]
        public ActionResult getTakserDetails(int id)
        {
            using (DBModel db = new DBModel())
            {
                var last = 99;
                var lastDate = "";
                var takser = db.TakserTable.Where(x => x.Id == id).ToList();
                if (takser.Count >0)
                {
                    var dateAndTime = DateTime.Now;
                    var curDate = dateAndTime.Date; //current day
                    for (int i = 0; i < takser.Count; i++)
                    {
                        var dbDate = DateTime.Parse(takser[i].DateOfTakser);
                        if ((curDate - dbDate).Days < last)
                        {
                            lastDate = dbDate.ToString();
                            last = (curDate - dbDate).Days;
                        }
                    }
                    return Json(new { emailSent = lastDate }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { emailSent = "null" }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        [HttpGet]
        [ValidateInput(false)]
        public ActionResult getHamlataDetails(int id)
        {

            using (DBModel db = new DBModel())
            {
                var vacalert = db.Hasroot.FirstOrDefault(x => x.Id == id);
                if (vacalert == null)
                {
                    return Json(new { emailSent = "null" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var date = vacalert.DateOfHasraa;
                    var dateAndTime = DateTime.Now;
                    var TodayDate = dateAndTime.Date; //current day
                    var HamlataDate = DateTime.Parse(date);
                    return Json(new { emailSent = date.ToString(), Days = (TodayDate - HamlataDate).Days }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpGet]
        [ValidateInput(false)]
        public ActionResult HamlataDetails(int id)
        {

            using (DBModel db = new DBModel())
            {
                float _sumamount = 0;
                float shpavghamlata = 0;
                var hamlataSum = db.Hamlata.Where(x => x.Id == id).ToList();
                for(int i=0; i < hamlataSum.Count; i++)
                {
                    _sumamount = _sumamount + Convert.ToInt32(hamlataSum[i].amount);
                }
                if (hamlataSum.Count > 0)
                {
                    shpavghamlata = (_sumamount / hamlataSum.Count);
                }
                var vacalert = db.Hamlata.FirstOrDefault(x => x.Id == id);
                if (vacalert == null)
                {
                    return Json(new { emailSent = "null" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var date = vacalert.dateOfHamlata;
                    var dateAndTime = DateTime.Now;
                    var TodayDate = dateAndTime.Date; //current day
                    var HamlataDate = DateTime.Parse(date);
                    return Json(new { emailSent = date.ToString(), Days = (TodayDate - HamlataDate).Days ,amount= vacalert.amount, avg = shpavghamlata }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public bool getAlertBoolean()
        {
            using (DBModel db = new DBModel())
            {
                var vacalert = db.Vaccinations.FirstOrDefault();
                if (vacalert == null) { return false; }
                if (vacalert.isEnabled == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        [HttpPost]
        public ActionResult ChangeToggle()
        {
            using (DBModel db = new DBModel())
            {
                var vacalert = db.Vaccinations.FirstOrDefault();
                if (vacalert == null) { return View("NULL"); }
                if (vacalert.isEnabled == 1)
                {
                    vacalert.isEnabled = 0;
                    db.Entry(vacalert).State = EntityState.Modified;
                    db.SaveChanges();
                    return View();
                }
                else
                {
                    vacalert.isEnabled = 1;
                    db.Entry(vacalert).State = EntityState.Modified;
                    db.SaveChanges();
                    return View();
                }
            }
        }

        [HttpGet]
        public ActionResult VacAlertByGroup(int id)
        {
            using (DBModel db = new DBModel())
            {
                var vacs = db.Vaccinations.ToList();
                if (vacs == null) { return null; } //nothing to check cuz there are no vacs YET!
                var dateAndTime = DateTime.Now;
                var Enddate = dateAndTime.Date; //current day
                /*let's get from DB all the information 
                            in order to check weither there are upcoming vacs 
                */
                var vac = new HashSet<VacsAlert>();
                DateTime d;
                int _id = 0;
                int group = 0;
                string med;
                for (int i = 0; i < vacs.Count; i++)
                {
                    //if you want to add more vacs , just use if for another type of vacs.
                        if (vacs[i].NextVaccinationDate != null)
                        {
                            d=  DateTime.Parse(vacs[i].NextVaccinationDate);
                            _id = vacs[i].Id;
                            group = GetGroupById(vacs[i].Id);
                            med = vacs[i].Medicine;
                            var check = vac.FirstOrDefault(x => x.Group == group);
                            if (check == null)
                            {
                                if (id == group)
                                {
                                    vac.Add(new VacsAlert { NextDate = d, Id = _id, Group = group, medicine = med });
                                }
                            }
                        }
                }
                //now let's do manupilation to check if there are upcoming vacs
                //we should use the current date variable in order to check that.
                foreach (var element in vac)
                {
                    if ((Enddate - element.NextDate).TotalDays <= 90)
                    {
                        element.flag = true;
                        element.days = Math.Abs((Enddate - element.NextDate).Days);
                    }
                }
                return View(vac);
            }
        }

        [HttpGet]
        public ActionResult VacAlert()
        {
            using (DBModel db = new DBModel())
            {
                var vacs = db.Vaccinations.ToList();
                if (vacs == null) { return null; } //nothing to check cuz there are no vacs YET!
                var dateAndTime = DateTime.Now;
                var Enddate = dateAndTime.Date; //current day
                /*let's get from DB all the information 
                            in order to check weither there are upcoming vacs 
                */
                var vac = new HashSet<VacsAlert>();
                DateTime d;
                int id = 0;
                int group = 0;
                string med;
                for (int i = 0; i < vacs.Count; i++)
                {
                        //if you want to add more vacs , just use if for another type of vacs.
                        if (vacs[i].NextVaccinationDate != null)
                        {
                            d=DateTime.ParseExact(vacs[i].NextVaccinationDate,
                            "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                            id = vacs[i].Id;
                            group = GetGroupById(vacs[i].Id);
                            med = vacs[i].Medicine;
                            var check = vac.FirstOrDefault(x => x.Group == group);
                            if (check == null)
                            {
                                vac.Add(new VacsAlert { NextDate = d, Id = id, Group = group, medicine = med });
                            }
                        }
                }
                //now let's do manupilation to check if there are upcoming vacs
                //we should use the current date variable in order to check that.
                foreach (var element in vac)
                {
                    if ((Enddate - element.NextDate).TotalDays <= 75)
                    {
                        element.flag = true;
                        element.days = Math.Abs((Enddate - element.NextDate).Days);
                    }
                }
                
               return View(vac);
            }
        }
        public int GetGroupById(int id)
        {
            using (DBModel db = new DBModel())
            {
                var shpId = db.maintable.FirstOrDefault(e => e.Id == id);
                if (shpId == null)
                {
                    return -1;
                }
                else { return shpId.Group; }
            }
        }
        //this function receives sheep id and group and updates the group
        public ActionResult ToGroup(int sid, int group ,int fromgroup)
        {
            using (DBModel db = new DBModel())
            {
                var shp = db.maintable.FirstOrDefault(e => e.Id == sid);
                shp.Group = group;
                db.Entry(shp).State = EntityState.Modified;
                db.SaveChanges();
                return null;
            }
        }

        public void updateDetails (UserViewModel shp)
        {
            using (DBModel db = new DBModel())
            {
                var sheepDetails = db.Details.FirstOrDefault(e => e.Id == shp.maintblSheeps.Id);
                //if there are already information but we need to modify it 
                if (sheepDetails != null)
                {
                    sheepDetails.Information = shp.shpDetail.Information;
                    db.Entry(sheepDetails).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else  //there are new information so we create new row in the db
                {
                    var newSheep = new Details();
                    newSheep.Information = shp.shpDetail.Information;
                    newSheep.Id = shp.maintblSheeps.Id;
                    db.Details.Add(newSheep);
                    db.SaveChanges();
                }
            }
        }
        public void updateBasicInfo(UserViewModel shp)
        {
            using (DBModel db = new DBModel())
            {
                var sheepDetails = db.maintable.FirstOrDefault(e => e.Id == shp.maintblSheeps.Id);
                sheepDetails.SheepNum = shp.maintblSheeps.SheepNum;
                sheepDetails.Group = shp.maintblSheeps.Group;
                sheepDetails.Birthday = shp.maintblSheeps.Birthday;
                sheepDetails.Blood = shp.maintblSheeps.Blood;
                db.Entry(sheepDetails).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        //this function just update the current diseases (not adding new)
        public void updateDiseases(UserViewModel shp)
        {
            using (DBModel db = new DBModel())
            {
                //first we update the current fields
                var sheepDiseases = db.Diseases.FirstOrDefault(e => e.Id == shp.maintblSheeps.Id);
                //already diseases but just modified.
                if (sheepDiseases != null)
                {
                    sheepDiseases.ShpDisease = shp.shpDiseases.ShpDisease;
                    db.Entry(sheepDiseases).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else //new row in database
                {
                    var newDisase = new Diseases();
                    newDisase.ShpDisease = shp.shpDiseases.ShpDisease;
                    newDisase.Id = shp.maintblSheeps.Id;
                    db.Diseases.Add(newDisase);
                    db.SaveChanges();
                }
            }
        }


        [HttpPost]
        public ActionResult AdvancedDetailsUpdate(UserViewModel shp)
        {
            try {
                updateBasicInfo(shp);
                updateDetails(shp);
                updateDiseases(shp);
                //updateHamlatot(shp);
                //updateVac(shp);
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
            return RedirectToAction("ShowMyHome");
        }

        public ActionResult ContactUs()
        {
            return View();
        }

        public ActionResult GetList()
        {
            using (DBModel db = new DBModel())
            {
                var shpList = db.maintable.ToList<maintable>();
                return Json(new { data = shpList }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public ActionResult GetMedsJson()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            using (DBModel db = new DBModel())
            {
                var medsJson = db.Medicine.ToList<Medicine>();
                for(int i = 0; i < medsJson.Count; i++)
                {
                    items.Add(new SelectListItem() { Text = medsJson[i].MedName,Value = medsJson[i].Info, Selected = false });
                }
                //items.Insert(0, new SelectListItem { Text = "Please select", Value = "0" });
                return Json(items, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Vacs()
        {
            using (DBModel db = new DBModel())
            {
                var vacs = db.Vaccinations.ToList<Vaccinations>();
                return View(vacs);
            }
        }

        [HttpGet]
        public ActionResult Hamlatot()
        {
            using (DBModel db = new DBModel())
            {
                var Hamlatot = db.Hamlata.ToList<Hamlata>();
                return View(Hamlatot);
            }
        }
        [HttpGet]
        public ActionResult Hasroot()
        {
            using (DBModel db = new DBModel())
            {
                var Hasroot = db.Hasroot.ToList<Hasroot>();
                return View(Hasroot);
            }
        }
        [HttpGet]
        public ActionResult GetListByGroup(int id)
        {
            using (DBModel db = new DBModel())
            {
                var shpList = db.maintable.Where(i => i.Group == id).ToList<maintable>();
                return Json(new { data = shpList }, JsonRequestBehavior.AllowGet);
            }
        }


    }
}