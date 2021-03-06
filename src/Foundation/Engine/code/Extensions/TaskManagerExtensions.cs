using Hackathon.MLBox.Foundation.Engine.Train.Models;
using Sitecore.Processing.Engine.Abstractions;
using Sitecore.Processing.Tasks.Options;
using Sitecore.Processing.Tasks.Options.DataSources.DataExtraction;
using Sitecore.XConnect;
using Sitecore.XConnect.Collection.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hackathon.MLBox.Foundation.Engine.Extensions
{
    public static class TaskManagerExtensionsCustom
    {
        public static async Task RegisterRfmModelTaskChainAsync(
          this ITaskManager taskManager,
          TimeSpan expiresAfter)
        {
            // datasource for ContactModel protection 
            var contactDataSourceOptionsDictionary = new ContactDataSourceOptionsDictionary(new ContactExpandOptions(PersonalInformation.DefaultFacetKey,
                    EmailAddressList.DefaultFacetKey,
                    ContactBehaviorProfile.DefaultFacetKey
                    )
            {
                Interactions = new RelatedInteractionsExpandOptions()
            }
                , 5, 10);

            var modelTrainingOptions = new ModelTrainingTaskOptions(
                // assembly name of our processing engine model (ContactModel:IModel<Interaction>) 
                typeof(ContactModel).AssemblyQualifiedName,
                // assembly name of entity for our processing engine model  (ContactModel:IModel<Interaction>) 
                typeof(Contact).AssemblyQualifiedName,
                // custom options that we pass to ContactModel
                new Dictionary<string, string> { ["TestCaseId"] = "Id" },
                // projection tableName of PurchaseOutcomeModel, must be equal to first parameter of 'CreateTabular' method => PurchaseOutcomeModel.cs: CreateTabular("PurchaseOutcome", ...)
                "PurchaseOutcome",
                // name of resulted table (any name)
                "DemoResultTable");
            
            await taskManager.RegisterModelTrainingTaskChainAsync(modelTrainingOptions, contactDataSourceOptionsDictionary, expiresAfter);
            // Register chain of Tasks

        }
    }
}
