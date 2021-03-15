using DeerManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DeerManager.Controllers
{
    public class SettingsController : Controller
    {
        // GET: Settings
        DB_AccessLayer.DB dblayer = new DB_AccessLayer.DB();
        public ActionResult SettingPage()
        {
            using (DBModel db = new DBModel())
            {
                return View(db.Settings);
            }
        }
        [HttpGet]
        public ActionResult EditSettings()
        {
            return View();
        }
        [HttpPost]
        public ActionResult EditSettings(Settings person)
        {
            using (DBModel db = new DBModel())
            {
                return View(db.Settings);
            }
        }
    }
}