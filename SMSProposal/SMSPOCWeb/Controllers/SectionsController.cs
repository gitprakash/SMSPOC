using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DataModelLibrary;

namespace SMSPOCWeb.Controllers
{
    [Authorize(Roles = "Subscriber")]
    public class SectionsController : Controller
    {
        private Model1 db = new Model1();

        // GET: Sections
        public ActionResult Index()
        {
          //  db.Sections.Include(s => s.Standard);
            return View(db.Sections.ToList());
        }

        // GET: Sections/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Section section = db.Sections.Find(id);
            if (section == null)
            {
                return HttpNotFound();
            }
            return View(section);
        }

        // GET: Sections/Create
        public ActionResult Create()
        {
            ViewBag.Standards = db.Standards.ToArray();
            return View();
        }

        // POST: Sections/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Section section)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //if (await db.Sections.AnyAsync(s => s.StandardId == section.StandardId && s.Name == section.Name))
                    //{
                    //   throw new Exception("Duplicate found in Standard and Section");
                    //}
                    db.Sections.Add(section);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            ViewBag.Standards = db.Standards.ToArray();
            return View(section);
        }

        // GET: Sections/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Section section = db.Sections.Find(id);
            ViewBag.Standards = db.Standards.ToArray();
            if (section == null)
            {
                return HttpNotFound();
            }
            return View(section);
        }

        // POST: Sections/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Section section)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //if (await db.Sections.AnyAsync(s => s.Id !=section.Id && s.StandardId == section.StandardId && s.Name == section.Name))
                    //{
                    //   throw new Exception("Duplicate found in Standard and Section");
                    //}
                    db.Entry(section).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            ViewBag.Standards = db.Standards.ToArray();
            return View(section);
        }

        // GET: Sections/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Section section = db.Sections.Find(id);
            if (section == null)
            {
                return HttpNotFound();
            }
            return View(section);
        }

        // POST: Sections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Section section = db.Sections.Find(id);
            db.Sections.Remove(section);
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
