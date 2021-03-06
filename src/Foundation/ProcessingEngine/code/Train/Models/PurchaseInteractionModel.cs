﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hackathon.NaN.MLBox.Foundation.ProcessingEngine.Services;
using Sitecore.ContentTesting.ML.Workers;
using Sitecore.Processing.Engine.ML.Abstractions;
using Sitecore.Processing.Engine.Projection;
using Sitecore.Processing.Engine.Storage.Abstractions;
using Sitecore.XConnect;

namespace Hackathon.NaN.MLBox.Foundation.ProcessingEngine.Train.Models
{
 
    public class PurchaseInteractionModel : BaseWorker, IModel<Contact>
    {
        private readonly IMLNetService _mlNetService;
        private readonly ITableStoreFactory _tableStoreFactory;
        public PurchaseInteractionModel(IReadOnlyDictionary<string, string> options, IMLNetService mlNetService,  ITableStoreFactory tableStoreFactory) : base (tableStoreFactory)
        {

            _tableStoreFactory = tableStoreFactory;
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