using System.Collections.Generic;
using System.Linq;
using Hackathon.MLBox.Foundation.Common.Models.Metrics;
using Hackathon.MLBox.Foundation.Engine.Mappers;
using Hackathon.MLBox.Foundation.Engine.Train.Models;
using Hackathon.NaN.MLBox.Foundation.ProcessingEngine.Predict.Models;
using Microsoft.Extensions.Configuration;
using Sitecore.Processing.Engine.ML.Abstractions;
using Sitecore.Processing.Engine.Projection;

namespace Hackathon.MLBox.Foundation.Engine.Services
{
    public class MLNetService : IMLNetService
    {

        public MLNetService(IConfiguration configuration)
        {
       
        }

        public ModelStatistics Train(IReadOnlyList<IDataRow> data)
        {
            var customersData = CustomerModelMapper.MapToCustomers(data);

            var rfmCalculateService = new RfmCalculateService();
            var calculatedScores = rfmCalculateService.CalculateRfmScores(customersData);

            var businessData = calculatedScores.Select(x => new RFMScore()
            {
                R = x.RFM.R,
                F = x.RFM.F,
                M = x.RFM.M
            }).ToList();

            return new RfmStatistics{ Customers = calculatedScores };
        }

        public IReadOnlyList<PredictionResult> Evaluate(IReadOnlyList<IDataRow> data)
        {
            //var validContacts = data.Where(x => x.Enabled() && !string.IsNullOrEmpty(x.GetContactEmail())).ToList();
            //var rfmList = validContacts.Select(x => x.MapToRfmFacet()).Select(rfm => new ClusteringData
            //{
            //    R = rfm.R,
            //    F = rfm.F,
            //    M = rfm.M
            //}).ToList();

            //var predictions = new CustomersSegmentator().Predict(rfmList);
            //return validContacts.Select((t, i) => new PredictionResult {Email = t.GetContactEmail(), Cluster = predictions[i]}).ToList();
            throw new System.NotImplementedException();
        }

        public int Predict(int value)
        {
            throw new System.NotImplementedException();
        }
    }

    public interface IMLNetService
    {
        ModelStatistics Train(IReadOnlyList<IDataRow> data);
        IReadOnlyList<PredictionResult> Evaluate(IReadOnlyList<IDataRow> data);
        int Predict(int value);
    }
}
