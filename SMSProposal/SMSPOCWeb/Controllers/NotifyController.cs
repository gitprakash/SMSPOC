﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DataModelLibrary;
using DataServiceLibrary;
using Newtonsoft.Json.Schema;
using SMSPOCWeb.Models;

namespace SMSPOCWeb.Controllers
{
    [Authorize(Roles = "Subscriber")]
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
                 var identity = (CustomIdentity)User.Identity;
                 if (messageViewModel != null && messagecount >= 1 && !string.IsNullOrEmpty(message))
                 {
                     if (ModelState.IsValid)
                     {
                         if (!await m_messageService.CheckMessageBalance(messageViewModel.Count(), messagecount, identity.User.Id))
                         {
                             throw new Exception("Insufficient Message Balance, Contact Administator to update Your Package");
                         }
                         result = await m_messageService.LogAllMessage(messageViewModel, message, messagecount,identity.User.Id);
                     }
                     else
                     {
                         string messages = GetModelStateError();
                         throw new Exception(messages);
                     }
                     var jsonresult = new { Status = result == true ? "success" : "successwithnoinsertion", JsonRequestBehavior.AllowGet };
                     return Json(jsonresult, JsonRequestBehavior.AllowGet);
                 }
                 else
                 {
                     throw new Exception("Invalid inputs, check your selected contact, message details");
                 }

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

        public ActionResult GetMessageHistoryView()
        {
            return View();
        }

        public async Task<JsonResult> GetMessageHistory(JgGridParam jgGridParam)
        {
            var identity = (CustomIdentity)User.Identity;
            if (jgGridParam != null)
            {
                var messages = await m_messageService.MessageHistory(jgGridParam, identity.User.Id);
                int totalRecords = await m_messageService.TotalMessageHistory(identity.User.Id);
                var totalPages = (int) Math.Ceiling((float) totalRecords/(float) jgGridParam.rows);
                var jsonData = new
                {
                    total = totalPages,
                    jgGridParam.page,
                    records = totalRecords,
                    rows = messages
                };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            throw  new Exception("invalid inputs ");
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