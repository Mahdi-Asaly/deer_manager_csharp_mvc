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
        public ActionResult ShowMyHome()
        {
            return View("ShowMyHome");
        }


        [HttpPost]
        public ActionResult AddNewSheep(maintable shp)
        {
            using (DBModel db = new DBModel())
            {
                if (shp.Id == 0)
                {
                    db.maintables.Add(shp);
                    db.SaveChanges();
                    return View("ShowMyHome");
                    //return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    db.Entry(shp).State = EntityState.Modified;
                    db.SaveChanges();
                    return View("ShowMyHome");
                    //return Json(new { success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpGet]
        public ActionResult AddNewSheep(int id = 0)
        {
            return View(new maintable());
        }

        public ActionResult EditSheep()
        {
            return View();
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