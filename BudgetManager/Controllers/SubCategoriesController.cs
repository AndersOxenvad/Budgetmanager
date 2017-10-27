using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BudgetManager.Models;

namespace BudgetManager.Controllers
{
    public class SubCategoriesController : Controller
    {
        private BudgetManagerEntities db = new BudgetManagerEntities();

        // GET: SubCategories
        public ActionResult Index()
        {
            var subCategory = db.SubCategory.Include(s => s.Category);
            return View(subCategory.ToList());
        }

        // GET: SubCategories/Create
        public ActionResult Create()
        {
            ViewBag.FK_CategoryName = new SelectList(db.Category, "CategoryName", "CategoryName");
            return View();
        }

        // POST: SubCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SubCategoryName,FK_CategoryName")] SubCategory subCategory)
        {
            if (ModelState.IsValid)
            {
                db.SubCategory.Add(subCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FK_CategoryName = new SelectList(db.Category, "CategoryName", "CategoryName", subCategory.FK_CategoryName);
            return View(subCategory);
        }

        // GET: SubCategories/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubCategory subCategory = db.SubCategory.Find(id);
            if (subCategory == null)
            {
                return HttpNotFound();
            }
            ViewBag.FK_CategoryName = new SelectList(db.Category, "CategoryName", "CategoryName", subCategory.FK_CategoryName);
            return View(subCategory);
        }

        // POST: SubCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SubCategoryName,FK_CategoryName")] SubCategory subCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FK_CategoryName = new SelectList(db.Category, "CategoryName", "CategoryName", subCategory.FK_CategoryName);
            return View(subCategory);
        }

        // GET: SubCategories/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubCategory subCategory = db.SubCategory.Find(id);
            if (subCategory == null)
            {
                return HttpNotFound();
            }
            return View(subCategory);
        }

        // POST: SubCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            SubCategory subCategory = db.SubCategory.Find(id);

            if (subCategory.Transactions.Count == 0)
            {
                db.SubCategory.Remove(subCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "The subcategory is currently in use by a transaction";
                return View("Index", db.SubCategory.ToList());
            }
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
