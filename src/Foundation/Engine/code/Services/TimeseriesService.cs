using Microsoft.ML;
using Microsoft.ML.Transforms.TimeSeries;
using Sitecore.XConnect;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hackathon.MLBox.Foundation.Engine.Services
{
    public class TimeseriesService : ITimeseriesService
    {
        public float PredictMonetary(Contact contact, int window)
        {
            var data = contact.Interactions.SelectMany(x => x.Events.OfType<Outcome>())
                .Where(x => x.MonetaryValue > 0)
                .Select(x => new TimeSlice
                {
                    Value = (float)x.MonetaryValue,
                    Timestamp = x.Timestamp.Date
                }).OrderBy(x => x.Timestamp).ToList();


            var trainingData = new List<ModelInput>();
            while (true)
            {
                trainingData.Clear();
                var dateStart = data.Min(x => x.Timestamp);
                var dateEnd = data.Max(x => x.Timestamp);

               
                while (dateStart < dateEnd)
                {
                    var periodData = data.Where(x => x.Timestamp >= dateStart && x.Timestamp < dateStart.AddDays(window)).ToList();
                    trainingData.Add(new ModelInput
                    {
                        Value = periodData.Sum(x => x.Value)
                    });
                    dateStart = dateStart.AddDays(window);
                }

                if (trainingData.Count * 2 < window)
                {
                    window /= 2;
                    if (window == 0) return 0;
                }
                break;
            }
           

            MLContext mlContext = new MLContext();

            var min = data.Min(x => x.Value);
            var max = data.Max(x => x.Value);

            var test = trainingData.Select(x => x.Value).ToList();

            var dataView = mlContext.Data.LoadFromEnumerable(trainingData);

            var forecastingPipeline = mlContext.Forecasting.ForecastBySsa(outputColumnName: "ForecastedRentals",
                 inputColumnName: "Value", windowSize: window, seriesLength: trainingData.Count, trainSize: trainingData.Count, horizon: 1,
                 confidenceLevel: 0.95f, confidenceLowerBoundColumn: "LowerBoundRentals",
            confidenceUpperBoundColumn: "UpperBoundRentals");

            SsaForecastingTransformer forecaster = forecastingPipeline.Fit(dataView);

            var forecastEngine = forecaster.CreateTimeSeriesEngine<ModelInput, ModelOutput>(mlContext);
            ModelOutput forecast = forecastEngine.Predict();

            return forecast.ForecastedRentals.Average();
        }
    }

    public interface ITimeseriesService
    {
        float PredictMonetary(Contact contact, int window);
    }

    public class TimeSlice
    {
        public DateTime Timestamp { get; set; }
        public float Value { get; set; }
    }
    public class ModelInput
    {
        public float Value { get; set; }
    }

    public class ModelOutput
    {
        public float[] ForecastedRentals { get; set; }

        public float[] LowerBoundRentals { get; set; }

        public float[] UpperBoundRentals { get; set; }
    }
}
