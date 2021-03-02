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

namespace DeerManager.Controllers
{
    public class HomeController : Controller
    {
        DB_AccessLayer.DB dblayer = new DB_AccessLayer.DB();

        public ActionResult AdvancedDetails(int id)
        {
            //here you should import the informations from tables:
            using (DBModel db = new DBModel())
            {
                var shpVM = new UserViewModel
                {
                    shpDiseases = db.Diseases.Where(x => x.Id == id).ToList(),
                    maintblSheeps = db.maintable.FirstOrDefault(x => x.Id == id),
                    shpDetail = db.Details.FirstOrDefault(x => x.Id == id),
                    shpHamlata = db.Hamlatot.Where(x=>x.Id==id).ToList<Hamlatot>(),
                    shpVac= db.Vaccinations.Where(x=>x.Id==id).ToList<Vaccinations>()
                };
                return View(shpVM) ;
            }
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
                db.maintable.Remove(emp);
                db.SaveChanges();
                //return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
                return View("ShowMyHome");
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
                if (emp.Id == 0)
                {
                    db.maintable.Add(emp);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    db.Entry(emp).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);

                }
            }


        }

        [HttpPost]
        public ActionResult AddNewSheep(maintable shp)
        {
            using (DBModel db = new DBModel())
            {
                    db.maintable.Add(shp);
                    db.SaveChanges();
                    return View("ShowMyHome");
            }
        }

        [HttpGet]
        public ActionResult AddNewSheep()
        
        {
            return View(new maintable());
        }

        [HttpPost]
        public ActionResult MoveGroupPage(int sid)
        {
            using (DBModel db = new DBModel())
            {
                var sheepDetails = db.maintable.Where(e => e.Group == sid);
                return View(sheepDetails);
            }
        }

        [HttpGet]
        public ActionResult MoveGroupPage()
        {
            return View();
        }




        public ActionResult MoveGroup()
        {
            return View();
        }

        public void updateDetails (UserViewModel shp)
        {
            using (DBModel db = new DBModel())
            {
                var sheepDetails = db.Details.FirstOrDefault(e => e.Id == shp.maintblSheeps.Id);
                sheepDetails.Information = shp.shpDetail.Information;
                db.Entry(sheepDetails).State = EntityState.Modified;
                db.SaveChanges();
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
        public void updateDiseases(UserViewModel shp)
        {

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
                //updateDetails(shp);
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

            return Json(new { success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);

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
    }
}