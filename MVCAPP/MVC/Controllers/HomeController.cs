using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using BuisnessLayer;

namespace MVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }





        [HttpPost]
        public ActionResult Index(Inventory smp)
        {

            Apiclass apiclass;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string body = serializer.Serialize(smp);
            apiclass = new Apiclass(HttpMethod.Post, "Invntory", "SaveItemData", body, Apiclass.TypeContent.JSON);
            string Semeseterdar = apiclass.apicall();
            string[] semdata = Semeseterdar.Split('*');
            if (semdata[1] == "Created")
            {
                TempData["Showmsg"] = "Saved Successfully";
            }


            return View();
        }
        public JsonResult Getdetails()
        {
            Apiclass apiclass;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string body = "";
            apiclass = new Apiclass(HttpMethod.Get, "Invntory", "GetInventoryDetails", body, Apiclass.TypeContent.STRING);
            string query = apiclass.apicall();
            string[] query1 = query.Split('*');
            List<Inventory> InventoryDetails = serializer.Deserialize<List<Inventory>>(query1[0]);
            return Json(InventoryDetails, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateData(Inventory smp)
        {
            int id = Convert.ToInt32(smp.Item_id);
            string status = "";
            Apiclass apiclass;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            //string body = "Rollno=" + _login.Rollno + "&password=" + _login.Password;
            string body =  serializer.Serialize(smp);
            apiclass = new Apiclass(HttpMethod.Post, "Invntory", "UpdateInventDetals", body, Apiclass.TypeContent.JSON);
            string Semeseterdar = apiclass.apicall();
            string[] semdata = Semeseterdar.Split('*');
            if (semdata[1] == "Created")
            {
                return Json("1");
            }
            else
            {
                return Json("0");
            }
           
        }

        [HttpPost]
        public JsonResult DeleteData(string Item_id)
        {
           
            string status = "";
            Apiclass apiclass;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string body = "id=" + Item_id;
            //string body = serializer.Serialize(smp);
            apiclass = new Apiclass(HttpMethod.Get, "Invntory", "DeleteInventDetals", body, Apiclass.TypeContent.STRING);
            string Semeseterdar = apiclass.apicall();
            string[] semdata = Semeseterdar.Split('*');
            if (semdata[1] == "Created")
            {
                return Json("1");
            }
            else
            {
                return Json("0");
            }

        }
    }
}