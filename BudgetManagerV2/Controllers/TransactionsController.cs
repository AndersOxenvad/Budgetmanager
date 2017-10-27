using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BudgetManagerV2.Models;

namespace BudgetManagerV2.Controllers
{
    public class TransactionsController : Controller
    {
        private BudgetManagerEntities db = new BudgetManagerEntities();

        // GET: Transactions
        public ActionResult Index(string sortOrder)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.ValueSortParm = sortOrder == "Value" ? "value_desc" : "Value";
            ViewBag.CatSortParm = sortOrder == "FK_Category" ? "cat_desc" : "FK_Category";

            var transaction = db.Transaction.Include(t => t.Category);

            
            switch (sortOrder)
            {
                case "name_desc":
                    transaction = transaction.OrderByDescending(t => t.Text);
                    break;
                case "Value":
                    transaction = transaction.OrderBy(t => t.Value);
                    break;
                case "value_desc":
                    transaction = transaction.OrderByDescending(t => t.Value);
                    break;
                case "Date":
                    transaction = transaction.OrderByDescending(t => t.Date);
                    break;
                case "cat_desc":
                    transaction = transaction.OrderBy(t => t.FK_Category);
                    break;
                case "Category":
                    transaction = transaction.OrderByDescending(t => t.FK_Category);
                    break;
                default:
                    transaction = transaction.OrderBy(t => t.Text);
                    break;
                    
            }
            return View(transaction.ToList());
        }

        // GET: Transactions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transaction.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // GET: Transactions/Create
        public ActionResult Create()
        {
            ViewBag.FK_Category = new SelectList(db.Category, "Id", "Name");
            return View();
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Value,Text,Date,FK_Category")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                db.Transaction.Add(transaction);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FK_Category = new SelectList(db.Category, "Id", "Name", transaction.FK_Category);
            return View(transaction);
        }

        // GET: Transactions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transaction.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            ViewBag.FK_Category = new SelectList(db.Category, "Id", "Name", transaction.FK_Category);
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Value,Text,Date,FK_Category")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(transaction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FK_Category = new SelectList(db.Category, "Id", "Name", transaction.FK_Category);
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transaction.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Transaction transaction = db.Transaction.Find(id);
            db.Transaction.Remove(transaction);
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
