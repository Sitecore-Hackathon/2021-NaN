using System.Collections.Generic;
using Hackathon.MLBox.Foundation.Common.Models.Sitecore;
using Sitecore.Processing.Engine.ML.Abstractions;

namespace Hackathon.MLBox.Foundation.Engine.Train.Models
{
    public class RfmStatistics: ModelStatistics
    {
        public List<CustomerItem> Customers { get; set; }
    }
}
