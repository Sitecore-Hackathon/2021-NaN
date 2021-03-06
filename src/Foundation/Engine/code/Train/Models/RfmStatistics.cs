using System.Collections.Generic;
using Hackathon.NaN.MLBox.Foundation.ProcessingEngine.Models;
using Sitecore.Processing.Engine.ML.Abstractions;

namespace Hackathon.NaN.MLBox.Foundation.ProcessingEngine.Train.Models
{
    public class RfmStatistics: ModelStatistics
    {
        public List<Customer> Customers { get; set; }
    }
}
