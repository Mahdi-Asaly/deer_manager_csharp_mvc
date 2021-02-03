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
        public ActionResult Details()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Delete(string id)
        {
            using (DBModel db = new DBModel())
            {
                maintable shp = db.maintables.Where(x => x.Id == id).FirstOrDefault<maintable>();
                db.maintables.Remove(shp);
                db.SaveChanges();
                return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
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