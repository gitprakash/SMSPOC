using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DataModelLibrary;

namespace SMSPOCWeb.Controllers
{
    public class NotifyController : Controller
    {
        // GET: Notify
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult SendMessage(MessageViewModel[] messageViewModel,string message,int messagecount)
        {
            try
            {
                if (ModelState.IsValid)
                {

                }
                else
                {
                    string messages = GetModelStateError();
                    throw new Exception(messages);
                }
                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var result = new { Status = "error", error = ex.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult SendSms()
        {
            return View();
        }
        private string GetModelStateError()
        {
            string messages = string.Join("; ", ModelState.Values
                                .SelectMany(x => x.Errors)
                                .Select(x => x.ErrorMessage));
            return messages;
        }
    }
}