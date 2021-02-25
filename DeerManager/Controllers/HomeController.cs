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

namespace DeerManager.Controllers
{
    public class HomeController : Controller
    {
        DB_AccessLayer.DB dblayer = new DB_AccessLayer.DB();

        public ActionResult AdvancedDetails(int id)
        {
            //here you should import the informations from tables: Hamlatot,Vaccinations,Disesaes,Details

            return View();
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
                maintable emp = db.maintables.Where(x => x.Id == id).FirstOrDefault<maintable>();
                db.maintables.Remove(emp);
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
                    return View(db.maintables.Where(x => x.Id == id).FirstOrDefault<maintable>());
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
                    db.maintables.Add(emp);
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
                    db.maintables.Add(shp);
                    db.SaveChanges();
                    return View("ShowMyHome");
            }
        }

        [HttpGet]
        public ActionResult AddNewSheep()
        
        {
            return View(new maintable());
        }


        public ActionResult ContactUs()
        {
            return View();
        }
        public ActionResult GetList()
        {
            using (DBModel db = new DBModel())
            {
                var shpList = db.maintables.ToList<maintable>();
                return Json(new { data = shpList }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}