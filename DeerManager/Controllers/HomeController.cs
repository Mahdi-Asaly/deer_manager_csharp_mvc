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
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult ShowMyHome()
        {            
            return View("ShowMyHome");
        }
        public JsonResult get_data()
        {
            DataSet ds = dblayer.Show_Data();
            List<maintable> listbl = new List<maintable>();
            foreach(DataRow dr in ds.Tables[0].Rows)
            {

                listbl.Add(new maintable
                {
                    Id = Convert.ToInt32(dr["Id"]),
                    Birthday = (DateTime)dr["Birthday"],
                    Gender = dr["Gender"].ToString()
                }); 
            }
            return Json(listbl, JsonRequestBehavior.AllowGet);
        }
    }
}