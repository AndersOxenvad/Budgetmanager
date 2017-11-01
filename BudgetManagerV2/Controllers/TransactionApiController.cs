using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using BudgetManagerV2.Models;
using Newtonsoft.Json;
using System.Data;
using System.Data.Entity;
using System;
using System.Net.Http;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace BudgetManagerV2.Controllers
{

    public class TransactionController : ApiController
    {
        private static readonly HttpClient client = new HttpClient();
        private string ApiKey = "DYtEt0TuEE-N7PaRnuYtk-7IADgw1rIx";
        private BudgetManagerEntities db = new BudgetManagerEntities();
        Transaction trans = new Transaction();
        Category cat = new Category();

     
        [System.Web.Http.HttpGet]
        public IHttpActionResult Documentation()
        {
            //Replace with azure website link
            string url = "http://budgetmanagerxena.azurewebsites.net/Home/Documentation";

            Uri uri = new Uri(url);

            return Redirect(uri);
        }
        public IHttpActionResult GetTransaction(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;


            foreach (var item in db.Transaction.Include(t => t.Category).Where(x => x.Id == id))
            {

                var transaction = new
                {
                    ID = item.Id,
                    Text = item.Text,
                    Value = item.Value,
                    Date = item.Date,
                    Category = item.Category.Name
                };

                if (transaction == null)
                {
                    //return Redirect("~/Views/Home/index.cshtml");
                }

                Log("Got general information on transaction with id: " + id, ApiKey);

                string transactionJson = new JavaScriptSerializer().Serialize(transaction);
                //return Ok(new JavaScriptSerializer().Deserialize<object>(transactionJson));
                return Json(transactionJson);
            }

            return BadRequest("No transactions with that id");
        }


        public IHttpActionResult GetTransactionList(string start, string end)
        {
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            List<object> transactionList = new List<object>();

            start += "T00:00:00";
            end += "T00:00:00";

            DateTime? startTime = Convert.ToDateTime(start);
            DateTime? endTime = Convert.ToDateTime(end);



            foreach (var item in db.Transaction.Include(t => t.Category).Where(x => x.Date >= startTime && x.Date <= endTime))
            {
                var transaction = new
                {
                    ID = item.Id,
                    Text = item.Text,
                    Value = item.Value,
                    Date = item.Date,
                    Category = item.Category.Name
                };
                if (transaction == null)
                {
                    return Redirect("~/Views/Home/index.cshtml");
                }
                transactionList.Add(transaction);

            }
            if (transactionList == null)
            {
                return BadRequest("Request error");
            }
            Log("Got a list of transactions in the given time interval: " + start + " to " + end, ApiKey);
            return Ok(JsonConvert.SerializeObject(transactionList));

        }
        public static void Log(string Information, string apiKey)
        {
            var client = new HttpClient();

            var pairs = new List<KeyValuePair<string, string>>
    {
        new KeyValuePair<string, string>("information", Information),
        new KeyValuePair<string, string>("api_key", apiKey)
    };

            var content = new FormUrlEncodedContent(pairs);

            var response = client.PostAsync("http://mailmicroservice.herokuapp.com/api/log?", content).Result;

            if (response.IsSuccessStatusCode)
            {


            }
        }
    }
}
