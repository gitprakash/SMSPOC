using DataModelLibrary;
using DataServiceLibrary;
using Microsoft.AspNet.Identity.EntityFramework;
using SMSPOCWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SMSPOCWeb.Controllers
{
    public class RoleController : Controller
    {
        // GET: Role
        ApplicationDbContext context;
        IRoleService mroleService;
        public RoleController(IRoleService roleService)
        {
            context = new ApplicationDbContext();
            mroleService = roleService;
        }

        /// Get All Roles
        /// </summary>
        /// <returns></returns>
        /// 

        public ActionResult GetRoleView()
        {
            return View("Index");
        }

        public async Task<JsonResult> Index(string sidx, string sort, int page, int rows)
        {
            sort = sort ?? "asc";
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            var Rolelist = await mroleService.GetRoles(pageIndex * pageSize, pageSize, sidx, sort.ToUpper() == "DESC");
            int totalRecords = await mroleService.TotalRoles();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);
            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = Rolelist
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Create a New Role
        /// </summary>
        /// <param name="Role"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> Create(Role Role)
        {
            try
            {
                Role.CreatedBy = "prakash";
                Role.CreatedDate = DateTime.Now;
                Role dbrole= await mroleService.Add(Role);
                var jsonData = new
                {
                    Name=dbrole.Name
                };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [HttpPost]
        public string Edit(IdentityRole Model)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            string msg;
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(Model).State = EntityState.Modified;
                    db.SaveChanges();
                    msg = "Saved Successfully";
                }
                else
                {
                    msg = "Error in input data";
                }
            }
            catch (Exception ex)
            {
                msg = "Error occured:" + ex.Message;
            }
            return msg;
        }
        [HttpPost]
        public string Delete(string Id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var role = db.Roles.Find(Id);
            db.Roles.Remove(role);
            db.SaveChanges();
            return "Deleted successfully";
        }


    }
}