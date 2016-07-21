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
    [Authorize(Roles="Subscriber")]
    public class ContactController : Controller
    {
        IContactService mcontactService;
        public ContactController(IContactService contactService)
        {
            mcontactService = contactService;
        }
        public ActionResult GetContactView()
        {
            return View();
        }
        public async Task<JsonResult> Index(string sidx, string sort, int page, int rows)
        {
            var identity = (CustomIdentity)User.Identity;
            sort = sort ?? "asc";
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            var contacts = await mcontactService.Contacts(identity.User.ID, pageIndex * pageSize, pageSize, sidx, sort.ToUpper() == "DESC");
            int totalRecords = await mcontactService.TotalContacts(identity.User.ID);
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
	}
}