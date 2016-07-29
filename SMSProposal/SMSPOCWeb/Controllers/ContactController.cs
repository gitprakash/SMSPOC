using DataModelLibrary;
using DataServiceLibrary;
using SMSPOCWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SMSPOCWeb.Controllers
{
    [Authorize(Roles = "Subscriber")]
    public class ContactController : Controller
    {
        IContactService mcontactService;
        ISubscriberStandardService mclassService;
        public ContactController(IContactService contactService, ISubscriberStandardService classService)
        {
            mcontactService = contactService;
            mclassService = classService;
        }
        public ActionResult GetContactView()
        {
            return View();
        }
        public async Task<JsonResult> Index(string sidx, string sort, int page, int rows)
        {
            try
            {
                var identity = (CustomIdentity)User.Identity;
                sort = sort ?? "asc";
                int pageIndex = Convert.ToInt32(page) - 1;
                int pageSize = rows;
                var contacts = await mcontactService.Contacts(identity.User.Id, pageIndex * pageSize, pageSize, sidx, sort.ToUpper() == "DESC");
                int totalRecords = await mcontactService.TotalContacts(identity.User.Id);
                var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page,
                    records = totalRecords,
                    rows = contacts
                };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var result = new { Status = "error", error = ex.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public async Task<JsonResult> Add([Bind(Exclude = "Id")]ContactViewModel contactvm)
        {
            try
            {
                Contact contact;
                if (ModelState.IsValid)
                {
                    contact = await mcontactService.AddContact(contactvm);
                }
                else
                {
                    string messages = GetModelStateError();
                    throw new Exception(messages);
                }
                var resultstatus = new { Status = "success", Id = contact.Id };
                return Json(resultstatus, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                var result = new { Status = "error", error = ex.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> Edit(ContactViewModel contactvm)
        {
            try
            {
                int saved = 0;
                if (ModelState.IsValid)
                {
                    saved = await mcontactService.EditContact(contactvm);
                }
                else
                {
                    string messages = GetModelStateError();
                    throw new Exception(messages);
                }
                var resultstatus = new { Status = "success", Id = saved };
                return Json(resultstatus, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                var result = new { Status = "error", error = ex.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> Delete(int Id)
        {
            try
            {
                SubscriberStandardContacts contact;
                int saved = 0;
                if (ModelState.IsValid)
                {
                    contact = await mcontactService.FindContact(Id);
                    if (contact == null)
                    {
                        throw new Exception("Unable to find student contact details");
                    }
                    contact.Active = false;
                    contact.Contact.Active = false;
                    saved = await mcontactService.SaveAsync();
                }
                else
                {
                    string messages = GetModelStateError();
                    throw new Exception(messages);
                }
                var resultstatus = new { Status = "success", Id = saved };
                return Json(resultstatus, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                var result = new { Status = "error", error = ex.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        private string GetModelStateError()
        {
            string messages = string.Join("; ", ModelState.Values
                                .SelectMany(x => x.Errors)
                                .Select(x => x.ErrorMessage));
            return messages;
        }
        public Contact GetContact(ContactViewModel cv, Contact contact)
        {
            contact.Name = cv.Name;
            contact.Mobile = cv.Mobile;
            contact.RollNo = cv.RollNo;
            contact.BloodGroup = cv.BloodGroup;
            return contact;
        }
        public async Task<JsonResult> GetStandards()
        {
            var identity = (CustomIdentity)User.Identity;
            var Standards = await mclassService.GetStandards(identity.User.Id);
            return Json(Standards, JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> GetSections(int subscriberStandardId)
        {
            var Sections = await mclassService.GetSections(subscriberStandardId);
            return Json(Sections, JsonRequestBehavior.AllowGet);
        }
    }
}