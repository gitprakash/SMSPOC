using DataServiceLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DataModelLibrary;
using SMSPOCWeb.Models;
using System.Web.Security;

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

        public Subscriber GetSubscriber(SubscriberViewModel subscriberviewmodel)
        {
            Subscriber subscriber = new Subscriber
            {
                AccountTypeId = subscriberviewmodel.AccountTypeId,
                Active = true,
                Email = subscriberviewmodel.Email,
                FirstName = subscriberviewmodel.FirstName,
                Mobile = subscriberviewmodel.Mobile,
                GenderTypeId = subscriberviewmodel.GenderTypeId,
                LastName = subscriberviewmodel.LastName,
                Password = subscriberviewmodel.Password,
                Username = subscriberviewmodel.Username
            };
            return subscriber;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(SubscriberViewModel subscriberviewmodel)
        {
            if (ModelState.IsValid)
            {
                Subscriber subscriber = GetSubscriber(subscriberviewmodel);
                var useradd = await maccountService.Add(subscriber);
                return RedirectToAction("Index", "Home");
            }
            return View(subscriberviewmodel);
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel l, string ReturnUrl = "")
        {
            Tuple<bool,bool> tupleuser = await maccountService.CheckLogin(l.Username, l.Password);

            if (tupleuser.Item2)
            {
                FormsAuthentication.SetAuthCookie(l.Username, l.RememberMe);
                if (Url.IsLocalUrl(ReturnUrl))
                {
                    return Redirect(ReturnUrl);
                }
                else
                {
                    return RedirectToAction("Start", "Home");
                }
            }
            else if (tupleuser.Item1)
            {
                ModelState.AddModelError("", "Invalid Password");
            }
            else
            {
                ModelState.AddModelError("", "Invalid User,please register");
            }
            ModelState.Remove("Password");
            return View(l);
        }
        [Authorize]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<JsonResult> IsUserNameExists(string userName)
        {
            var userexists = await maccountService.IsUserNameExists(userName.Trim());
            if (userexists)
            {
                return Json(string.Format("User Name {0} already exists", userName), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<JsonResult> IsUserEmailExists(string email)
        {
            var emailexists = await maccountService.IsUserEmailExists(email.Trim());
            if (emailexists)
            {
                return Json(string.Format("Email {0} already exists", email), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<JsonResult> IsUniqueMobile(long mobile)
        {
            var emailexists = await maccountService.IsUniqueMobile(mobile);
            if (emailexists)
            {
                return Json(string.Format("Mobile number  {0} already exists", mobile), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
