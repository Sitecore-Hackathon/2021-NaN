using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sitecore.Processing.Engine.ML.Abstractions;
using Sitecore.Processing.Engine.ML.Workers;
using Sitecore.Processing.Tasks.Options.Workers.ML;
using Sitecore.XConnect;
using Sitecore.XConnect.Collection.Model;

namespace Hackathon.MLBox.Foundation.Engine.Predict.Workers
{
    public class RfmEvaluationWorker : EvaluationWorker<Contact>
    {
        private readonly ILogger<RfmEvaluationWorker> _logger;
        private readonly IServiceProvider _serviceProvider;
        public RfmEvaluationWorker(IModelEvaluator evaluator, IReadOnlyDictionary<string, string> options, ILogger<RfmEvaluationWorker> logger, IServiceProvider serviceProvider) : base(evaluator, options)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public RfmEvaluationWorker(IModelEvaluator evaluator, EvaluationWorkerOptionsDictionary options, ILogger<RfmEvaluationWorker> logger, IServiceProvider serviceProvider) : base(evaluator, options)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }


        protected override async Task ConsumeEvaluationResultsAsync(IReadOnlyList<Contact> entities, IReadOnlyList<object> evaluationResults, CancellationToken token)
        {

        }
    }
}
