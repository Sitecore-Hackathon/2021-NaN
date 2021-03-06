using System;
using System.Threading;
using System.Threading.Tasks;
using Hackathon.MLBox.Foundation.Engine.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Sitecore.Processing.Engine.Abstractions;
using Sitecore.Processing.Engine.Agents;

namespace Hackathon.MLBox.Foundation.Engine.Agents
{
    // used for register tasks from xConnect Processing Engine
    public class DemoAgent : RecurringAgent
    {
        private readonly ILogger<IAgent> _logger;
        private readonly ITaskManager _taskManager;

        public DemoAgent(IConfiguration options, ILogger<IAgent> logger, ITaskManager taskManager) : base(options, logger)
        {
            _logger = logger;
            _taskManager = taskManager;
        }

        // run once a day 
        protected override async Task RecurringExecuteAsync(CancellationToken token)
        {
            _logger.LogInformation("RecurringExecuteAsync: RegisterRfmModelTaskChain");
            await _taskManager.RegisterRfmModelTaskChainAsync(TimeSpan.FromDays(1));
        }
    }
}
