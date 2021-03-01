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
            //here you should import the informations from tables: Hamlatot,Vaccinations,Disesaes,Details
            using (DBModel db = new DBModel())
            {
                var shpVM = new UserViewModel
                {
                    shpDiseases = db.Diseases.Where(x => x.Id == id).ToList(),
                    maintblSheeps = db.maintable.FirstOrDefault(x => x.Id == id),
                    shpDetail = db.Details.Where(x => x.Id == id).ToList<Details>(),
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


        //[HttpPost]
        //public ActionResult AdvancedDetailsUpdate(UserViewModel shp)
        //{

        //    try
        //    {
        //        using (DBModel db = new DBModel())
        //        {
        //            maintable user = db.maintable.FirstOrDefault(x => x.Id == shp.maintblSheeps.Id);
        //            if (user != null)
        //            {
        //                user.Details = shp.shpDetail;
        //                user.Diseases = shp.shpDiseases;
        //                user.Vaccinations = shp.shpVac;
        //                user.Hamlatot = shp.shpHamlata;
        //                db.Entry(user).State = EntityState.Modified;
        //                db.SaveChanges();
        //                return Json(new { success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
        //            }
        //            else
        //            {
        //                TempData["msg"] = "משתמש לא נמצא";
        //                TempData["succ"] = false;
        //                return Json(new { success = true, message = "Updated UnSucc" }, JsonRequestBehavior.AllowGet);
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["msg"] = "פרטי משתמש לא עודכנו";
        //        TempData["succ"] = false;
        //    }
        //    return Redirect("https://localhost:44330/");
        //}
        [HttpPost]
        public ActionResult AdvancedDetailsUpdate(UserViewModel shp)
        {
            try { 
                using (DBModel db = new DBModel())
                {
                    maintable user = db.maintable.FirstOrDefault(x => x.Id == shp.maintblSheeps.Id);
                    if (user != null)
                    {
                        user.Details = shp.shpDetail;
                        user.Diseases = shp.shpDiseases;
                        user.Vaccinations = shp.shpVac[0]; //check this
                        user.Hamlatot = shp.shpHamlata[0]; //check this
                        db.Entry(user).State = EntityState.Modified;
                        db.SaveChanges();
                       
                    }

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
                        // raise a new exception nesting  
                        // the current instance as InnerException  
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