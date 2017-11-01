﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BudgetManagerV2.Models;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;

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

        [HttpGet]
        public static string GetImage(Transaction trans) {
            return GetImageHelper(trans).Result;
        }
        public static async Task<string> GetImageHelper(Transaction trans)
        {
            string imageURL;
            var client = new HttpClient();

            var response = client.GetAsync("http://image-search9000.herokuapp.com/Description?Beskrivelse=" + trans.Text).Result;
            string result = await response.Content.ReadAsStringAsync();
            imageURL = result;
            return imageURL;
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
        public ActionResult Create([Bind(Include = "Id,Value,Text,Date,FK_Category,ImageURL")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                
                 transaction.ImageURL = GetImage(transaction);
                
                
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
        [HttpGet]
        public ActionResult Mail()
        {


            return View(new Transaction());
        }
        [HttpPost]
        public ActionResult Mail(Transaction trans)
        {

            string mail = trans.Email;

            if (mail != null)
            {
                string template = "Title: [[Text]]\nValue: [[Value]]\nDate: [[Date]]\nCategory: [[Category]]";

                string apilink = "http://budgetmanagerxena.azurewebsites.net/api/transaction/Gettransaction/" + trans.Id;

                var client = new HttpClient();

                string content = string.Format("template={0}&api_link={1}&email={2}", template, apilink, mail);
                var response = client.GetAsync("http://mailmicroservice.herokuapp.com/api/sendEmail?" + content).Result;

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("index");

                }

            }
            return RedirectToAction("index");
        }
    }
}
