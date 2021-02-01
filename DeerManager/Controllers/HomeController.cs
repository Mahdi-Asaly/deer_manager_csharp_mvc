using DeerManager.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DeerManager.Controllers
{
    public class HomeController : Controller
    {
        DB_AccessLayer.DB dblayer = new DB_AccessLayer.DB();

  
        public ActionResult ShowMyHome()
        {
            return View("ShowMyHome");
        }
        public ActionResult EditSheep()
        {
            return View();
        }
        public ActionResult ContactUs()
        {
            return View();
        }
        public JsonResult get_data()
        {
            DataSet ds = dblayer.Show_Data();
            List<maintable> listbl = new List<maintable>();
            foreach(DataRow dr in ds.Tables[0].Rows)
            {
                listbl.Add(new maintable
                {    
                    RowNum = Convert.ToInt32(dr["Number"]),
                    Id = Convert.ToInt32(dr["Id"]),
                    SheepNum = Convert.ToInt32(dr["SheepNum"]),
                    BloodType = dr["Blood"].ToString(),
                    Gender = dr["Gender"].ToString(),
                    Group = Convert.ToInt32(dr["Group"]),
                    Birthday = (DateTime)dr["Birthday"],
                    IsAlive = true 
                }); 
            }
            return Json(listbl, JsonRequestBehavior.AllowGet);
        }
    }
}