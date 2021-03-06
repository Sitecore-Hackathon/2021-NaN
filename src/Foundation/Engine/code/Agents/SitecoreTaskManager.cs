﻿using System;
using System.Threading.Tasks;
using Hackathon.MLBox.Foundation.Engine.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;
using Sitecore.Diagnostics;
using Sitecore.Processing.Engine.Abstractions;

namespace Hackathon.MLBox.Foundation.Engine.Agents
{
    // used for register tasks from sitecore website (for debugging purposes) by calling /api/contactapi/registertasks
    public class SitecoreTaskManager
    {
        public async Task RegisterAll()
        {
            try
            {
                var taskManager = ServiceLocator.ServiceProvider.GetService<ITaskManager>();
                await taskManager.RegisterRfmModelTaskChainAsync(TimeSpan.FromDays(1));
                Log.Info("Cortex RegisterAll", this);
            }
            catch (Exception ex)
            {
                Log.Error("Cortex RegisterAll exception", ex, this);
            }

        }
    }

   
}