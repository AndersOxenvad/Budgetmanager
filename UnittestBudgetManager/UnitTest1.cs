using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BudgetManagerV2.Controllers;
using System.Net.Http;
using System.Web.Http;
using BudgetManagerV2.Models;
using System.Collections.Generic;

namespace UnittestBudgetManager
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void GetTransaction()
        {
            foreach (var item in GetTestTransaction())
            {
               
                var transaction = new
                {
                    ID = item.Id,
                    Text = item.Text,
                    Value = item.Value,
                    Date = item.Date

                };
                Assert.AreNotEqual(transaction, null);
            }
           
        }
        [TestMethod]
        public void GetTransactionList()
        {
            string startTime = "2017-01-01";
            string endTime = "2017-02-06";

            DateTime? start = Convert.ToDateTime(startTime);
            DateTime? end = Convert.ToDateTime(endTime);
            List<object> transList = new List<object>();
            List < Transaction > testItems = GetTestTransaction().FindAll(x => x.Date >= start && x.Date <= end);
            foreach (var item in testItems)
            {

                var transaction = new
                {
                    ID = item.Id,
                    Text = item.Text,
                    Value = item.Value,
                    Date = item.Date

                };
                transList.Add(transaction);
                
            }
            Assert.AreEqual(transList.Count, 2);

        }
        [TestMethod]
        public void Log()
        {
            var client = new HttpClient();
            string Information = "Unittest information";
            string apiKey = "unittest apikey";
            var pairs = new List<KeyValuePair<string, string>>
    {
        new KeyValuePair<string, string>("information", Information),
        new KeyValuePair<string, string>("api_key", apiKey)
    };
            string compare = "";
            var content = new FormUrlEncodedContent(pairs);
            foreach (var item in pairs)
            {
                compare += item.Key + "=" + item.Value + "&";
            }
           int index = compare.LastIndexOf("&");
           string actuall = compare.Remove(index, 1);
            Assert.AreEqual("information=" + Information + "&" + "api_key=" + apiKey, actuall);
        }
        public  List<Transaction> GetTestTransaction()
        {
            var testProducts = new List<Transaction>();
            testProducts.Add(new Transaction { Id = 1, Value = 452525, Text = "Hej hej, dette er en test", Date = Convert.ToDateTime("2017-01-02") });
            testProducts.Add(new Transaction { Id = 2, Value = 452525, Text = "Hej hej, dette er en test", Date = Convert.ToDateTime("2017-02-05") });
            return testProducts;
        }
    }
}
