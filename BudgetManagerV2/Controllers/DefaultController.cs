using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using BudgetManagerV2.Models;
using Newtonsoft.Json;
using System.Data;
using System.Data.Entity;
using System.Net;

namespace BudgetManagerV2.Controllers
{

    public class DefaultController : ApiController
    {
     
        private BudgetManagerEntities db = new BudgetManagerEntities();
        Transaction trans = new Transaction();
        Category cat = new Category();

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
                    return Redirect("~/Views/Home/index.cshtml");
                }


                return Ok(JsonConvert.SerializeObject(transaction));
                
            }

            return BadRequest("No transactions with that id");
        }
    }
}
