using System.Collections.Generic;
using System.Linq;
using Hackathon.MLBox.Foundation.Common.Models.Metrics;
using Hackathon.MLBox.Foundation.Engine.Mappers;
using Microsoft.Extensions.Configuration;
using Sitecore.Processing.Engine.Projection;

namespace Hackathon.MLBox.Foundation.Engine.Services
{
    public partial class ForecastService : IForecastService
    {

        public ForecastService(IConfiguration configuration)
        {
       
        }

        public void Train(IReadOnlyList<IDataRow> data)
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
        }

        public int Predict(int value)
        {
            throw new System.NotImplementedException();
        }
    }

    public interface IForecastService
    {
         void Train(IReadOnlyList<IDataRow> data);
         int Predict(int value);
    }
}
