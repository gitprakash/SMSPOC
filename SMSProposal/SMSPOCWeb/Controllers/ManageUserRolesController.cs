using DataModelLibrary;
using DataServiceLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SMSPOCWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ManageUserRolesController : Controller
    {
        IUserRoleService muserroleService;
        IAccountService maccountService;
        IRoleService mroleService;
        public ManageUserRolesController(IUserRoleService userroleService,
            IRoleService roleService,
            IAccountService accountService)
        {
            muserroleService = userroleService;
            mroleService = roleService;
            maccountService = accountService;
        }
        public ActionResult GetUserRolesView()
        {
            return View("UserRoles");
        }

        public async Task<JsonResult> UserRoles(string sidx, string sort, int page, int rows)
        {
            sort = sort ?? "asc";
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            var usersrolelist = await maccountService.GetUserRole();
            var usersroles = usersrolelist.Select(t => new { Id = t.Item1, User = t.Item2, Role = t.Item3 });
            int totalRecords = await maccountService.TotalUserRoles();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);
            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = usersroles
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<JsonResult> Add(string User,string Role)
        {
            try
            {
                Subscriber subscriber = await maccountService.FinduserAsync(User);
                Role dbRole = await mroleService.FindRole(Role);
                if (subscriber==null)
                {
                    throw new Exception(string.Format("user {0} not exists", User, Role));
                }
                if (dbRole == null)
                {
                    throw new Exception(string.Format("Role {0} not exists", User, Role));
                }
                if (await muserroleService.CheckExists(User, Role))
                {
                    throw new Exception(string.Format("User {0} and role {1} already mapped", User, Role));
                }
                SubscriberRoles sroles = new SubscriberRoles { RoleId=dbRole.Id,SubscriberId=subscriber.ID, Active=true };
                await muserroleService.AddUserRole(sroles);
                var result = new { Status = "success"};
                return Json(result, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                var result = new { Status = "error", error = ex.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
    }
}