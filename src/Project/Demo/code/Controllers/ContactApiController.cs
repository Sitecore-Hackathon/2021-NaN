using System;
using System.Threading.Tasks;
using System.Web.Http;
using Hackathon.MLBox.Foundation.Engine.Agents;
using Hackathon.MLBox.Project.Demo.Models;
using Hackathon.NaN.MLBox.Foundation.ProcessingEngine.Import;
using Hackathon.NaN.MLBox.Foundation.ProcessingEngine.Services;

namespace Hackathon.MLBox.Project.Demo.Controllers
{
    public class ContactApiController : ApiController
    {
        // used for register processing engine tasks from sitecore website (for debugging purposes) by calling /api/contactapi/registertasks
        [HttpGet]
        public async Task<bool> RegisterTasks()
        {
            var x = new SitecoreTaskManager();
            await x.RegisterAll();
            return true;
        }

        [HttpGet]
        public async Task<bool> Test()
        {
           return true;
        }

        // used for upload orders history from excel to xConnect by using Demo-Data-Explorer(can takes ~15 minutes)
        [HttpPost]
        public async Task<ParseDataResult> UploadClientsHistory()
        {
            try
            {
                var stream = await Request.Content.ReadAsStreamAsync();
                var customers = new ExcelImportProcessor().GetImportData(stream);

                var count = customers.Count;
                var index = 0;

                var purchaseService = new XConnectService();

                foreach (var c in customers)
                {
                    index++;
                    var added = await purchaseService.Add(c, true);
                    if (added)
                    {
                        Sitecore.Diagnostics.Log.Info($"Excel import: {index} from {count}: CustomerID={c.CustomerId}", this);
                    }
                    else
                    {
                        Sitecore.Diagnostics.Log.Error($"Excel import: {index} from {count}: CustomerID={c.CustomerId}", this);
                    }
                }

                return new ParseDataResult
                {
                    CustomersCount = count,
                    InteractionsCount = count,
                    PurchasesCount = customers.Count
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
         
        }
    }
}