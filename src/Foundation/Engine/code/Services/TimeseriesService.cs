using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML;
using Microsoft.ML.TimeSeries;
using Sitecore.XConnect;

namespace Hackathon.MLBox.Foundation.Engine.Services
{
    public class TimeseriesService
    {
        public float PredictMonetary(Contact contact, int window)
        {
            var data = contact.Interactions.SelectMany(x => x.Events.OfType<Outcome>())
                .Where(x => x.MonetaryValue > 0)
                .Select(x => new ModelInput
                {
                    Value = (double)x.MonetaryValue,
                    Timestamp = x.Timestamp
                });

            MLContext mlContext = new MLContext();

            var dataView = mlContext.Data.LoadFromEnumerable(data);

           var forecastingPipeline = mlContext.Forecasting.ForecastBySsa(outputColumnName: "ForecastedRentals",
                inputColumnName: "Value", windowSize: window, seriesLength: 30, trainSize: 365, horizon: 7,
                confidenceLevel: 0.95f);
            
      

            return 0;
        }
    }

    public class ModelInput
    {
        public DateTime Timestamp { get; set; }

        public double Value { get; set; }
    }

    public class ModelOutput
    {
        public float[] ForecastedRentals { get; set; }

        public float[] LowerBoundRentals { get; set; }

        public float[] UpperBoundRentals { get; set; }
    }
}
