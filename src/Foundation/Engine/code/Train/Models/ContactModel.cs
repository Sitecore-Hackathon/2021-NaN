using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hackathon.MLBox.Foundation.Engine.Services;
using Sitecore.ContentTesting.ML.Workers;
using Sitecore.Processing.Engine.ML.Abstractions;
using Sitecore.Processing.Engine.Projection;
using Sitecore.Processing.Engine.Storage.Abstractions;
using Sitecore.XConnect;

namespace Hackathon.MLBox.Foundation.Engine.Train.Models
{
 
    public class ContactModel : BaseWorker, IModel<Contact>
    {
        private readonly IForecastService _forecastService;
        private readonly ITableStoreFactory _tableStoreFactory;
        public ContactModel(IReadOnlyDictionary<string, string> options, IForecastService forecastService,  ITableStoreFactory tableStoreFactory) : base (tableStoreFactory)
        {

            _tableStoreFactory = tableStoreFactory;
            _forecastService = forecastService;

        }

        public async Task<ModelStatistics> TrainAsync(string schemaName, CancellationToken cancellationToken, params TableDefinition[] tables)
        {
            var tableStore = _tableStoreFactory.Create(schemaName);
            var data = await GetDataRowsAsync(tableStore, tables.First().Name, cancellationToken);
            
            return _forecastService.Train(data);
        }

        public Task<IReadOnlyList<object>> EvaluateAsync(string schemaName, CancellationToken cancellationToken, params TableDefinition[] tables)
        {
            throw new NotImplementedException();
        }

        public IProjection<Contact> Projection =>
            Sitecore.Processing.Engine.Projection.Projection.Of<Contact>().CreateTabular(
                "PurchaseOutcome",
                contact =>
                contact.Interactions.SelectMany(x => x.Events.OfType<Outcome>())
                        .Select(x => new
                        {
                            ContactId = contact.Id,
                            Value = (double)x.MonetaryValue,
                            Timestamp = x.Timestamp
                        }),
                cfg => cfg
                    .Attribute("ContactId", x => x.ContactId)
                    .Attribute("Value", x => x.Value)
                    .Attribute("Timestamp", x => x.Timestamp)
            );


    }
}