using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMSPOCWeb.Controllers
{
    public class NotifyController : Controller
    {
        // GET: Notify
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Sendsms()
        {
            return View();
        }
    }
}