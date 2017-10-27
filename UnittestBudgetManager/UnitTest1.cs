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

                };
                Assert.AreNotEqual(transaction, null);
            }
           
        }
        public  List<Transaction> GetTestTransaction()
        {
            var testProducts = new List<Transaction>();
            testProducts.Add(new Transaction { Id = 1, Value = 452525, Text = "Hej hej, dette er en test" });
            testProducts.Add(new Transaction { Id = 2, Value = 452525, Text = "Hej hej, dette er en test" });
            return testProducts;
        }
    }
}
