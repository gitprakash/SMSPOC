using DataServiceLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DataModelLibrary;

namespace SMSPOCWeb.Controllers
{
    public class AccountController : Controller
    {
        IAccountService maccountService;
        public AccountController(IAccountService accountService)
        {
            maccountService = accountService;
        }
        public ActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> Register()
        {
            ViewBag.AccountTypeID = await maccountService.Accounttypes();
            ViewBag.GenderTypeID = await maccountService.Gendertypes();
            return View();
        }
        [HttpPost]
        public  ActionResult Register(FormCollection subscriber)
        {
            if (ModelState.IsValid)
            {
                //var useradd = await maccountService.Add(subscriber);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                throw new Exception("error while processing input");

            }
        }
        [HttpPost]
        public async Task<bool> IsUserNameExists(string username)
        {
            return await maccountService.IsUserNameExists(username.Trim());
        }
        [HttpPost]
        public async Task<bool> IsUserEmailExists(string email)
        {
            return await maccountService.IsUserEmailExists(email.Trim());
        }
    }
}
