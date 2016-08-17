using DataModelLibrary;
using DataServiceLibrary;
using SMSPOCWeb.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Excel;

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
        public ActionResult UploadView()
        {
            return View("UploadStudent");
        }
        public async Task<JsonResult> Index(JgGridParam jgGridParam)
        {
            try
            {

                var identity = (CustomIdentity)User.Identity;
                var contacts = await mcontactService.Contacts(identity.User.Id, jgGridParam);
                int totalRecords = await mcontactService.TotalContacts(identity.User.Id);
                var totalPages = (int)Math.Ceiling((float)totalRecords / (float)jgGridParam.rows);
                var jsonData = new
                {
                    total = totalPages,
                    jgGridParam.page,
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

        private async Task<ActionResult> SaveBulkUpload(List<ContactViewModel> cvmresult)
        {
            var identity = (CustomIdentity)User.Identity;
            var lstexistrollno = await mcontactService.CheckExcelBuilkRollNoExistsTask(identity.User.Id, cvmresult);
            if (lstexistrollno.Count > 0)
            {
                lstexistrollno.ForEach(r =>
                {
                    if (!string.IsNullOrEmpty(r.Section))
                    {
                        ModelState.AddModelError("Error",
                            string.Format(
                                "Roll No {0} already exists in Class {1} Section {2}",
                                r.RollNo, r.Class, r.Section));
                    }
                    else
                    {
                        ModelState.AddModelError("Error",
                            string.Format(
                                "Roll No {0} already exists in  Class {1}",
                                r.RollNo, r.Class));
                    }
                });
                return View("UploadStudent");
            }
            //var
            var newclasslstadded = await mclassService.AddBulkClassifNotExists(cvmresult, identity.User.Id);
            ViewBag.Class = newclasslstadded;
            var newsectionlist = await mclassService.AddBulkSectionsifNotExists(cvmresult, identity.User.Id);
           // ViewBag.Section = newsectionlist;
            var newclasssectionlink = await mclassService.AddBulkClassSectionLinkIfNotExists(cvmresult, identity.User.Id);
            ViewBag.ClassSection = newclasssectionlink;
           var contacts= await mcontactService.ExcelBulkUploadContact(identity.User.Id, cvmresult);
            ViewBag.Contacts = contacts;
            return View("UploadStudent");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Upload(HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (upload != null && upload.ContentLength > 0)
                    {
                        using (Stream stream = upload.InputStream)
                        {
                            IExcelDataReader reader = null;
                            if (upload.FileName.EndsWith(".xls"))
                            {
                                reader = ExcelReaderFactory.CreateBinaryReader(stream);
                            }
                            else if (upload.FileName.EndsWith(".xlsx"))
                            {
                                reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                            }
                            else
                            {
                                ModelState.AddModelError("File", "This file format is not supported");
                                return View("UploadStudent");
                            }
                            reader.IsFirstRowAsColumnNames = true;
                            DataSet result = reader.AsDataSet();
                            reader.Close();
                            var tupledsstatus = result.ValidateStudentTemplate();
                            if (!tupledsstatus.Item1)
                            {
                                ModelState.AddModelError("Error", tupledsstatus.Item2);
                                return View("UploadStudent");
                            }
                            var cvmresult = mcontactService.GetContactViewModels(result.Tables[0]);
                            await SaveBulkUpload(cvmresult);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("File", "Please Upload Your file");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Exception occured", ex.Message);
                    if (ex.InnerException != null)
                    {
                        ModelState.AddModelError("Exception occured", ex.InnerException.Message);

                    }
                }
            }
            return View("UploadStudent");
        }
    }
}