using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DataModelLibrary;
using DataServiceLibrary;
using SMSPOCWeb.Models;

namespace SMSPOCWeb.Controllers
{
    [Authorize(Roles = "Subscriber")]
    public class TemplateController : Controller
    {
        // GET: Template
        private ITemplateService mtemplateService;
        public TemplateController(ITemplateService templateService)
        {
            mtemplateService = templateService;
        }

        public ActionResult TemplateView()
        {
            return View();
        }

        public async Task<JsonResult> Index(string sidx, string sort, int page, int rows)
        {
            var identity = (CustomIdentity)User.Identity;
            sort = sort ?? "asc";
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            var contacts = await mtemplateService.Templates(identity.User.Id, pageIndex * pageSize, pageSize, sidx, sort.ToUpper() == "DESC");
            int totalRecords = await mtemplateService.TotalTemplates(identity.User.Id);
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
        [HttpPost]
        public async Task<JsonResult> Add(TemplateViewModel templatevm)
        {
            try
            {

                SubscriberTemplate st;
                if (ModelState.IsValid)
                {
            var identity = (CustomIdentity)User.Identity;

                    st = await mtemplateService.FindTemplate(identity.User.Id, templatename: templatevm.Name);
                }
                else
                {
                   // string messages = GetModelStateError();
                    throw new Exception("");
                }
                var resultstatus = new { Status = "success", Id = st.Id };
                return Json(resultstatus, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                var result = new { Status = "error", error = ex.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }


    }
}