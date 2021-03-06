﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Hackathon.NaN.MLBox.Foundation.ProcessingEngine.Train.Workers;
using Microsoft.Extensions.Logging;
using Sitecore.Processing.Engine.Abstractions;
using Sitecore.Processing.Engine.ML;
using Sitecore.Processing.Engine.ML.Abstractions;
using Sitecore.Processing.Engine.Projection;
using Sitecore.Processing.Engine.Storage.Abstractions;
using Sitecore.XConnect;

namespace Hackathon.MLBox.Foundation.Engine.Train.Workers
{
    
    public class RfmTrainingWorker : IDeferredWorker
    {
        private readonly IModel<Interaction> _model;
        private readonly RfmTrainingWorkerOptionsDictionary _options;
        private readonly ITableStore _tableStore;
        private readonly ILogger<RfmTrainingWorker> _logger;
        private readonly IServiceProvider _serviceProvider;

        public RfmTrainingWorker(
            ITableStoreFactory tableStoreFactory,
            IServiceProvider provider,
            ILogger<RfmTrainingWorker> logger,
            AllowedModelsDictionary modelsDictionary,
            RfmTrainingWorkerOptionsDictionary options,
            IServiceProvider serviceProvider)
        {

            this._tableStore = tableStoreFactory.Create(options.SchemaName);
            this._options = options;
            this._logger = logger;
            this._model = modelsDictionary.CreateModel<Interaction>(provider, options.ModelType, options.ModelOptions);
            this._serviceProvider = serviceProvider;
        }

        public RfmTrainingWorker(
            ITableStoreFactory tableStoreFactory,
            IServiceProvider provider,
            ILogger<RfmTrainingWorker> logger,
            AllowedModelsDictionary modelsDictionary,
            IReadOnlyDictionary<string, string> options,
            IServiceProvider serviceProvider)
            : this(tableStoreFactory, provider, logger, modelsDictionary, RfmTrainingWorkerOptionsDictionary.Parse(options), serviceProvider)
        {
        }



        public async Task RunAsync(CancellationToken token)
        {
            _logger.LogInformation("RfmTrainingWorker.RunAsync");
            
            IReadOnlyList<string> tableNames = _options.TableNames;
            List<Task<TableStatistics>> tableStatisticsTasks = new List<Task<TableStatistics>>(tableNames.Count);
            foreach (string tableName in tableNames)
                tableStatisticsTasks.Add(this._tableStore.GetTableStatisticsAsync(tableName, token));
            TableStatistics[] tableStatisticsArray = await Task.WhenAll(tableStatisticsTasks).ConfigureAwait(false);
            List<TableDefinition> tableDefinitionList = new List<TableDefinition>(tableStatisticsTasks.Count);
            for (int index = 0; index < tableStatisticsTasks.Count; ++index)
            {
                TableStatistics result = tableStatisticsTasks[index].Result;
                if (result == null)
                    this._logger.LogWarning(string.Format("Statistics data for {0} table could not be retrieved. It will not participate in model training.", (object)tableNames[index]));
                else
                    tableDefinitionList.Add(result.Definition);
            }
            ModelStatistics modelStatistics = await _model.TrainAsync(_options.SchemaName, token, tableDefinitionList.ToArray()).ConfigureAwait(false);
        
        }

       
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool dispose)
        {
        }
    }
}
