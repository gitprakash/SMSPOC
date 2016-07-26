using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataModelLibrary;
using SMSPOCWeb.Models;

namespace SMSPOCWeb.Controllers
{
    [Authorize(Roles = "Subscriber")]
    public class SubscriberStandardSectionsController : Controller
    {
        private Model1 db = new Model1();

        // GET: SubscriberStandardSections
        public async Task<ActionResult> Index()
        {
            var subscriberStandardSections = db.SubscriberStandardSections.Include(s => s.Sections).Include(s => s.SubscriberStandards);
            return View(await subscriberStandardSections.ToListAsync());
        }

        // GET: SubscriberStandardSections/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubscriberStandardSections subscriberStandardSections = await db.SubscriberStandardSections.FindAsync(id);
            if (subscriberStandardSections == null)
            {
                return HttpNotFound();
            }
            return View(subscriberStandardSections);
        }

        // GET: SubscriberStandardSections/Create
        public async Task<ActionResult> Create()
        {
            var authuser = ((CustomIdentity)User.Identity).User.Id;
            ViewBag.SectionId = new SelectList(db.Sections, "Id", "Name");
            var standards = await db.SubscriberStandards.Where(s => s.Subscriber.Id == authuser).ToArrayAsync();
            ViewBag.SubscriberStandardsId = new SelectList(standards.Select(s => s.Standard).OrderBy(s => s.Name), "Id", "Name");
            return View();
        }

        // POST: SubscriberStandardSections/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,SubscriberStandardsId,SectionId,Active")] SubscriberStandardSections postsss)
        {
            if (ModelState.IsValid)
            {
                try
                {
                   // if(await db.SubscriberStandardSections.AnyAsync(ss=>ss.SubscriberStandardsId==))
                    db.SubscriberStandardSections.Add(subscriberStandardSections);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            var authuser = ((CustomIdentity)User.Identity).User.Id;
            ViewBag.SectionId = new SelectList(db.Sections, "Id", "Name", subscriberStandardSections.SectionId);
            var standards = await db.SubscriberStandards.Where(s => s.Subscriber.Id == authuser).ToArrayAsync();
            ViewBag.SubscriberStandardsId = new SelectList(standards.Select(s => s.Standard).OrderBy(s => s.Name), "Id", "Name");
            return View(subscriberStandardSections);
        }

        // GET: SubscriberStandardSections/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubscriberStandardSections subscriberStandardSections = await db.SubscriberStandardSections.FindAsync(id);
            var authuser = ((CustomIdentity)User.Identity).User.Id;

            if (subscriberStandardSections == null)
            {
                return HttpNotFound();
            }
            ViewBag.SectionId = new SelectList(db.Sections, "Id", "Name", subscriberStandardSections.SectionId);
            ViewBag.SubscriberStandardsId = new SelectList(db.SubscriberStandards, "Id", "Id", subscriberStandardSections.SubscriberStandardsId);
            return View(subscriberStandardSections);
        }

        // POST: SubscriberStandardSections/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,SubscriberStandardsId,SectionId,Active")] SubscriberStandardSections subscriberStandardSections)
        {
            var authuser = ((CustomIdentity)User.Identity).User.Id;
            if (ModelState.IsValid)
            {
                db.Entry(subscriberStandardSections).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.SectionId = new SelectList(db.Sections, "Id", "Name", subscriberStandardSections.SectionId);
            var standards = await db.SubscriberStandards.Where(s => s.Subscriber.Id == authuser).ToArrayAsync();
            ViewBag.SubscriberStandardsId = new SelectList(standards.Select(s => s.Standard).OrderBy(s => s.Name), "Id", "Name");
            return View(subscriberStandardSections);
        }

        // GET: SubscriberStandardSections/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubscriberStandardSections subscriberStandardSections = await db.SubscriberStandardSections.FindAsync(id);
            if (subscriberStandardSections == null)
            {
                return HttpNotFound();
            }
            return View(subscriberStandardSections);
        }

        // POST: SubscriberStandardSections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            SubscriberStandardSections subscriberStandardSections = await db.SubscriberStandardSections.FindAsync(id);
            db.SubscriberStandardSections.Remove(subscriberStandardSections);
            await db.SaveChangesAsync();
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
