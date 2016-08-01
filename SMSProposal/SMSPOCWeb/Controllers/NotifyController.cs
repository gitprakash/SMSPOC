using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DataModelLibrary;
using DataServiceLibrary;

namespace SMSPOCWeb.Controllers
{
    public class NotifyController : Controller
    {
        // GET: Notify
        IMessageService m_messageService;
        public NotifyController(IMessageService messageService)
        {
            m_messageService = messageService;
        }
        public ActionResult Index()
        {
            return View();
        }
        public async Task<JsonResult> SendMessage(List<MessageViewModel> messageViewModel,string message,int messagecount)
        {
            try
            {
                bool result = false;
                if (ModelState.IsValid)
                {
                    result= await  m_messageService.Send(messageViewModel, message, messagecount);
                }
                else
                {
                    string messages = GetModelStateError();
                    throw new Exception(messages);
                }
                var jsonresult=new {Status=result == true ? "success" : "successwithnoinsertion", JsonRequestBehavior.AllowGet};
                return Json(jsonresult, JsonRequestBehavior.AllowGet);

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