using Framework48Mvc.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Framework48Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly HomeService _homeService = new HomeService();
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetMessage()
        {
            var result = _homeService.GetMessage();

            return Json(
                result,
                JsonRequestBehavior.AllowGet
            );
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}