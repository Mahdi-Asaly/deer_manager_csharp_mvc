using DeerManager.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeerManager.DB_AccessLayer;
using Newtonsoft.Json;
using System.Data.Entity;
using DeerManager.ViewModels;
using System.Data.Entity.Validation;
using System.Globalization;
using DeerManager.Classes;

namespace DeerManager.Controllers
{
    public class HomeController : Controller
    {
        public bool toggle = false;
        DB_AccessLayer.DB dblayer = new DB_AccessLayer.DB();
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
                    shpHamlata = db.Hamlatot.Where(x=>x.Id==id).ToList<Hamlatot>(),
                    shpVac= db.Vaccinations.Where(x=>x.Id==id).ToList<Vaccinations>()
                };
                return View(shpVM);
            }
        }

        public ActionResult errorPage(string err)
        {
            return View(err);
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
                if (emp == null) { return View("errorPage","Home",new { error = "something" }); }
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
        public ActionResult AddHamlata(int id)
        {
            if (ModelState.IsValid)
            {
                var newHamlata = new Hamlatot();
                newHamlata.Id = id;
                return View(newHamlata);
            }
            else { return View("errorPage"); }
        }

        [HttpPost]
        //this function receives sheep id and date and update hamlata
        public bool AddSpecificHamlata(int shpid, DateTime date)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (DBModel db = new DBModel())
                    {
                        var hamlata = new Hamlatot();
                        //we check if there are column in database with already date with same sheep id.
                        var checkAlready = db.Hamlatot.FirstOrDefault(x => x.Id == shpid);
                        //we remove the old date if exists.
                        if (checkAlready != null)
                        {
                            db.Hamlatot.Remove(checkAlready);
                        }
                        hamlata.DateOfTakser = null;
                        hamlata.Id = shpid;
                        hamlata.DateOfHamlata = date.ToString("dd/MM/yyyy");
                        db.Hamlatot.Add(hamlata);
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
        public bool AddSpecificVac(int shpid, int group, string med, string curdate ,string nextdate)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (DBModel db = new DBModel())
                    {
                        var vac = new Vaccinations();
                        //we check if there are column in database with already date with same sheep id.
                        var checkAlready = db.Vaccinations.FirstOrDefault(x => x.Id == shpid && x.DateOfVaccination == curdate && x.Medicine==med);
                        //we remove the old date if exists.
                        if (checkAlready != null)
                        {
                            db.Vaccinations.Remove(checkAlready);
                        }
                        vac.Id = shpid;
                        vac.Medicine = med; 
                        vac.DateOfVaccination = curdate;
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
        public ActionResult AddHamlata(Hamlatot shp)
        {
            if (ModelState.IsValid)
            {
                if (shp == null) { return View("ShowMyHome"); }
                using (DBModel db = new DBModel())
                {
                    db.Hamlatot.Add(shp);
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


        //still bugs
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
                if (med == null) { return View("errorPage"); }
                using (DBModel db = new DBModel())
                {
                    if(db.Medicine.Any(md => md.MedName == med.MedName))
                    {
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
                if (med == null) { return View("errorPage"); }
                using (DBModel db = new DBModel())
                {
                    if (!db.Medicine.Any(md => md.MedName == med.MedName))
                    {
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
                var SpecificVac = db.Vaccinations.Where(x => x.Id == shp.Id).Where(x => x.Medicine.Contains(shp.Medicine)).FirstOrDefault();
                if (SpecificVac != null)
                {
                    db.Vaccinations.Remove(SpecificVac);
                    db.SaveChanges();
                    return Json(new { result = "OK" }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { result = "ERROR" , id = shp.Id }, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public ActionResult soonHamlata()
        {
            var curdate = DateTime.Now;
            using (DBModel db = new DBModel())
            {
                var dates = db.Hamlatot.ToList();
                var group = new Hamlatot();
                var maximum = 0;
                var flag = false;
                var groupOfSheeps = 0;
                if (dates.Count<1)
                {
                    return Json(new { result = "Null" }, JsonRequestBehavior.AllowGet);
                }
                for(int i=0; i < dates.Count; i++)
                {
                    var shpdate= DateTime.Parse(dates[i].DateOfHamlata);
                    if((curdate - shpdate).Days > maximum && (curdate - shpdate).Days <150 && (curdate-shpdate).Days >130)
                    {
                        maximum = (curdate - shpdate).Days;
                        group.DateOfHamlata = dates[i].DateOfHamlata;
                        groupOfSheeps = GetGroupById(dates[i].Id);
                        flag = true;
                    }
                }
                if (flag == false)
                {
                  return Json(new { flag=false }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { max = maximum, _date = group.DateOfHamlata.ToString(), shpsgroup= groupOfSheeps , still = 150-maximum }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult LaterHamlata()
        {
            var curdate = DateTime.Now;
            using (DBModel db = new DBModel())
            {
                var dates = db.Hamlatot.ToList();
                var group = new Hamlatot();
                var maximum = 0;
                var groupOfSheeps = 0;
                if (dates.Count<1)
                {
                    return Json(new { result = "Null" }, JsonRequestBehavior.AllowGet);
                }
                for (int i = 0; i < dates.Count; i++)
                {
                    var shpdate = DateTime.Parse(dates[i].DateOfHamlata);
                    if ((curdate - shpdate).Days > maximum && (curdate - shpdate).Days < 130)
                    {
                        maximum = (curdate - shpdate).Days;
                        group.DateOfHamlata = dates[i].DateOfHamlata;
                        groupOfSheeps = GetGroupById(dates[i].Id);
                    }
                }
                return Json(new { max = maximum, _date = group.DateOfHamlata.ToString(), shpsgroup = groupOfSheeps }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public ActionResult RemoveHamlata(int id)
        {
            using (DBModel db = new DBModel())
            {
                var RemoveHamlata = new Hamlatot();
                RemoveHamlata = db.Hamlatot.Where(x => x.Id == id).FirstOrDefault();
                if (RemoveHamlata == null)
                {
                    return null;
                }
                return View(RemoveHamlata);
            }
        }

        [HttpPost]
        public ActionResult RemoveHamlata(Hamlatot shp)
        {
            using (DBModel db = new DBModel())
            {
                var SpecificHamlata = db.Hamlatot.Where(x => x.Id == shp.Id).FirstOrDefault();
                db.Hamlatot.Remove(SpecificHamlata);
                db.SaveChanges();
            }
            return RedirectToAction("AdvancedDetails", new { id = shp.Id });
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
        public ActionResult getVacDate(int id)
        {

            using (DBModel db = new DBModel())
            {
                var vacalert = db.Vaccinations.FirstOrDefault(x => x.Id == id);
                if (vacalert != null)
                {
                    return Json(new { emailSent = vacalert.DateOfVaccination });
                }
                else
                {
                    return Json(new { emailSent = "Null" });
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
        [ValidateInput(false)]
        public ActionResult getHamlataDetails(int id)
        {

            using (DBModel db = new DBModel())
            {
                var vacalert = db.Hamlatot.FirstOrDefault(x => x.Id == id);
                if (vacalert == null)
                {
                    return Json(new { emailSent = "null" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var date = vacalert.DateOfHamlata;
                    var dateAndTime = DateTime.Now;
                    var TodayDate = dateAndTime.Date; //current day
                    var HamlataDate = DateTime.Parse(date);
                    return Json(new { emailSent = date.ToString(), Days= (TodayDate-HamlataDate).Days },JsonRequestBehavior.AllowGet);
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
                            d = DateTime.ParseExact(vacs[i].NextVaccinationDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
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
                            d = DateTime.ParseExact(vacs[i].NextVaccinationDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
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
                    if ((Enddate - element.NextDate).TotalDays <= 90)
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
        public void updateHamlatot(UserViewModel shp)
        {

        }
        public void updateVac(UserViewModel shp)
        {
            
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
                var Hamlatot = db.Hamlatot.ToList<Hamlatot>();
                return View(Hamlatot);
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