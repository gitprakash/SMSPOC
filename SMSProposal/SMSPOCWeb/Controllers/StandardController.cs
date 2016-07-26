using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataModelLibrary;
using System.Threading.Tasks;
using SMSPOCWeb.Models;

namespace SMSPOCWeb.Controllers
{
    [Authorize(Roles = "Subscriber")]
    public class StandardController : Controller
    {
        private Model1 db = new Model1();
        // GET: /Standard/
        public ActionResult Index()
        {
            var authuser = ((CustomIdentity)User.Identity).User.Id;
            var subscriberstandards = db.SubscriberStandards.Where(s=>s.SubscriberId==authuser)
                .Include(s => s.Standard)
                .Include(s => s.Subscriber);
            return View(subscriberstandards.ToList());
        }

        // GET: /Standard/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubscriberStandards subscriberstandards = db.SubscriberStandards.Find(id);
            if (subscriberstandards == null)
            {
                return HttpNotFound();
            }
            return View(subscriberstandards);
        }

        // GET: /Standard/Create
        public ActionResult Create()
        {
            ViewBag.StandardId = new SelectList(db.Standards, "Id", "Name");
            return View();
        }

        // POST: /Standard/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="Id,StandardId,Active")] SubscriberStandards subscriberstandards)
        {
            var authuser = ((CustomIdentity)User.Identity).User.Id;
            if (ModelState.IsValid)
            {
                try
                {
                    if (await db.SubscriberStandards.AnyAsync(s => s.SubscriberId==authuser && s.StandardId == subscriberstandards.StandardId))
                    {
                        throw new Exception("Standard already exists");
                    }
                    subscriberstandards.SubscriberId = ((CustomIdentity)User.Identity).User.Id;
                    subscriberstandards.CreatedAt = DateTime.Now;
                    db.SubscriberStandards.Add(subscriberstandards);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            ViewBag.StandardId = new SelectList(db.Standards, "Id", "Name", subscriberstandards.StandardId);
           // ViewBag.SubscriberId = new SelectList(db.Subscribers, "Id", "Username", subscriberstandards.SubscriberId);
            return View(subscriberstandards);
        }

        // GET: /Standard/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubscriberStandards subscriberstandards = db.SubscriberStandards.Find(id);
            if (subscriberstandards == null)
            {
                return HttpNotFound();
            }
            ViewBag.StandardId = new SelectList(db.Standards, "Id", "Name", subscriberstandards.StandardId);
           // ViewBag.SubscriberId = new SelectList(db.Subscribers, "Id", "Username", subscriberstandards.SubscriberId);
            return View(subscriberstandards);
        }

        // POST: /Standard/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="Id,SubscriberId,StandardId,Active")] SubscriberStandards subscriberstandards)
        {
            var authuser = ((CustomIdentity)User.Identity).User.Id;
            if (ModelState.IsValid)
            {
                try
                {
                    if (await db.SubscriberStandards.AnyAsync(s => s.Id!=subscriberstandards.Id 
                        && s.SubscriberId==authuser 
                        && s.StandardId == subscriberstandards.StandardId))
                    {
                        throw new Exception("Standard already exists");
                    }
                    subscriberstandards.CreatedAt = DateTime.Now;
                    db.Entry(subscriberstandards).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            ViewBag.StandardId = new SelectList(db.Standards, "Id", "Name", subscriberstandards.StandardId);
            ViewBag.SubscriberId = new SelectList(db.Subscribers, "Id", "Username", subscriberstandards.SubscriberId);
            return View(subscriberstandards);
        }

        // GET: /Standard/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubscriberStandards subscriberstandards = db.SubscriberStandards.Find(id);
            if (subscriberstandards == null)
            {
                return HttpNotFound();
            }
            return View(subscriberstandards);
        }

        // POST: /Standard/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SubscriberStandards subscriberstandards = db.SubscriberStandards.Find(id);
            db.SubscriberStandards.Remove(subscriberstandards);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
