using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hackathon.NaN.MLBox.Foundation.ProcessingEngine.Models;
using Hackathon.NaN.MLBox.Foundation.ProcessingEngine.Predict.Models;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace Hackathon.NaN.MLBox.Foundation.ProcessingEngine.Services
{
    public class CustomersSegmentator
    {
        private static MLContext _mlContext;
        private static string TrainedModelFile => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Segmentator.zip");

        private IDataView _testingDataView;
        private static IDataView _dataView;
        private static ITransformer _transformer;
        private static ITransformer _transformerCountry;

        private int RfmMaxForTests = 3;


        static CustomersSegmentator()
        {
            _mlContext = new MLContext(1);
        }

        private static ITransformer TrainModel
        {
            get
            {
                if (_transformer != null) return _transformer;
                return LoadModel();
            }
            set
            {
                _transformer = value;
                SaveModel(_transformer, _dataView.Schema);
            }
        }
       
        public ClusteringMetrics Train(List<Rfm> list)
        {
            _mlContext = new MLContext(6);
            _dataView = _mlContext.Data.LoadFromEnumerable(list);

            var trainingDataView = _mlContext.Data.TrainTestSplit(_dataView);

            _testingDataView = trainingDataView.TestSet;

            string featuresColumnName = "Features";

            var pipeline = _mlContext.Transforms
                .Concatenate(featuresColumnName, "R", "M", "F")
                .Append(_mlContext.Clustering.Trainers.KMeans(featuresColumnName, numberOfClusters: 5));

            var model = pipeline.Fit(trainingDataView.TrainSet);
            var metrics = Evaluate(model);

            TrainModel = model;
            return metrics;
        }

        public ClusteringMetrics Evaluate(ITransformer model)
        {
            Console.WriteLine("=============== Evaluating Model with Test data===============");

            var predictions = model.Transform(_testingDataView);

            var metrics = _mlContext.Clustering.Evaluate(predictions, scoreColumnName: "Score", featureColumnName: "Features");

            Console.WriteLine();
            Console.WriteLine("Model quality metrics evaluation");
            Console.WriteLine("--------------------------------");
            Console.WriteLine($"AverageDistance: {metrics.AverageDistance:P2}");
            Console.WriteLine($"DaviesBouldinIndex: {metrics.DaviesBouldinIndex:P2}");
            Console.WriteLine($"NormalizedMutualInformation: {metrics.NormalizedMutualInformation:P2}");
            Console.WriteLine("=============== End of model evaluation ===============");

            TrainModel = model;

            
            // Run test cases to identify clusters
            var predictionFunction = _mlContext.Model.CreatePredictionEngine<ClusteringData, ClusteringPrediction>(TrainModel);
            var tests = new List<TestCase>();
            for (var r = 1; r <= RfmMaxForTests; r++)
            {
                for (var f = 1; f <= RfmMaxForTests; f++)
                {
                    for (var m = 1; m <= RfmMaxForTests; m++)
                    {
                        var data = new ClusteringData
                        {
                            R = r,
                            M = f,
                            F = m
                        };
                        var prediction = predictionFunction.Predict(data);
                        tests.Add(new TestCase
                        {
                            Data = data,
                            Cluster = prediction.SelectedClusterId
                        });
                    }
                }
            }

            //var fileService = new FileService();
            //fileService.ExportToCsv(tests);


            return metrics;
        }

        public List<int> Predict(List<ClusteringData> records)
        {
            var list = new List<ClusteringPrediction>();
            var predictionFunction = _mlContext.Model.CreatePredictionEngine<ClusteringData, ClusteringPrediction>(TrainModel);

            foreach (var record in records)
            {
                list.Add(predictionFunction.Predict(record));
            }
            return list.Select(x => (int)x.SelectedClusterId).ToList();
        }

        private static ITransformer LoadModel(string path = null)
        {
            ITransformer model;
            var fname = path == null ? TrainedModelFile : path;
            using (var stream = new FileStream(fname, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                model = _mlContext.Model.Load(stream, out _);
            }

            return model;
        }

        private static void SaveModel(ITransformer model, DataViewSchema schema, string path = null)
        {
            var fname = path == null ? TrainedModelFile : path;
            using (var fileStream = new FileStream(fname, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                _mlContext.Model.Save(model, schema, path);
            }
        }


    }

    public class TestCase
    {
        public ClusteringData Data { get; set; }
        public uint Cluster { get; set; }
    }
}
