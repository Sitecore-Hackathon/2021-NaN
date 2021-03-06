using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hackathon.NaN.MLBox.Foundation.ProcessingEngine.Models;
using Hackathon.NaN.MLBox.Foundation.ProcessingEngine.Services;
using Sitecore.ContentTesting.ML.Workers;
using Sitecore.Processing.Engine.ML.Abstractions;
using Sitecore.Processing.Engine.Projection;
using Sitecore.Processing.Engine.Storage.Abstractions;
using Sitecore.XConnect;
using Sitecore.XConnect.Collection.Model;

namespace Hackathon.NaN.MLBox.Foundation.ProcessingEngine.Train.Models
{
 
    public class PurchaseInteractionModel : BaseWorker, IModel<Interaction>
    {
        private readonly IMLNetService _mlNetService;
        private readonly ITableStoreFactory _tableStoreFactory;
        public PurchaseInteractionModel(IReadOnlyDictionary<string, string> options, IMLNetService mlNetService,  ITableStoreFactory tableStoreFactory) : base (tableStoreFactory)
        {

            _tableStoreFactory = tableStoreFactory;

            Projection = Sitecore.Processing.Engine.Projection.Projection.Of<Interaction>()
                .CreateTabular("PurchaseOutcome",
                    interaction => interaction.Events.OfType<PurchaseOutcome>().Select(p => 
                        new
                        {
                            Purchase = p,
                            interaction.IpInfo()?.Country
                        }),
                    cfg => cfg.Key("ID", x => x.Purchase.Id)
                        .Attribute("InvoiceId", x => x.Purchase.InvoiceId)
                        .Attribute("Quantity", x => x.Purchase.Quantity)
                        .Attribute("Timestamp", x => x.Purchase.Timestamp)
                        .Attribute("UnitPrice", x => x.Purchase.UnitPrice)
                        .Attribute("CustomerId", x => x.Purchase.CustomerId)
                        .Attribute("ProductId", x => x.Purchase.ProductId)
                        .Attribute("Country", x => x.Country, true)
                );

            _mlNetService = mlNetService;

        }

        public async Task<ModelStatistics> TrainAsync(string schemaName, CancellationToken cancellationToken, params TableDefinition[] tables)
        {
            var tableStore = _tableStoreFactory.Create(schemaName);
            var data = await GetDataRowsAsync(tableStore, tables.First().Name, cancellationToken);
            
            return _mlNetService.Train(data);
        }

        public Task<IReadOnlyList<object>> EvaluateAsync(string schemaName, CancellationToken cancellationToken, params TableDefinition[] tables)
        {
            throw new NotImplementedException();
        }

        public IProjection<Interaction> Projection { get; set; }

    }
}