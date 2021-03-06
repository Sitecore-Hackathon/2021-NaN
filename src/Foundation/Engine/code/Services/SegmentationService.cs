using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Hackathon.MLBox.Foundation.Common.Extensions;
using Hackathon.MLBox.Foundation.Common.Helpers;
using Hackathon.MLBox.Foundation.Common.Models.Sitecore;
using Hackathon.NaN.MLBox.Foundation.ProcessingEngine.Storage;
using ProtoBuf;

namespace Hackathon.MLBox.Foundation.Engine.Services
{
    public class SegmentationService
    {
        private int Q1 = 25;
        private int Q3 = 75;

        public Segments RebuildSegments(List<InvoiceItem> list)
        {
            if (list != null)
            {
                var customers = list.GroupBy(x => x.ContactId)
                .Select(x => new CustomerItem
                {
                    ContactId = x.Key,
                    Invoices = list
                }).ToList();


                if (customers.Count > 0)
                {
                    int length = customers.Count();
                    double[] monetaryList = new double[length];
                    double[] recencyList = new double[length];
                    double[] frequencyList = new double[length];

                    for (int i = 0; i < length; i++)
                    {
                        double m = 0;
                        foreach (InvoiceItem invoice in customers[i].Invoices)
                        {
                            m += invoice.Value;
                        }

                        monetaryList[i] = m;

                        // Recency

                        DateTime? minDate = customers[i].Invoices.Where(x => x.Timestamp.Year != 1).DefaultIfEmpty().Min(x => x?.Timestamp);
                        DateTime? maxDate = customers[i].Invoices.Where(x => x.Timestamp.Year != 1).DefaultIfEmpty().Max(x => x?.Timestamp);
                        recencyList[i] = ((int)(minDate.HasValue && maxDate.HasValue ? (maxDate - minDate).Value.TotalDays + 1 : 0));

                        // Frequency

                        frequencyList[i] = ((int)customers[i].Invoices.Count);
                    }

                    StatHelper stat = new StatHelper();
                    Segments segments = new Segments()
                    {
                        MonetaryQ1 = stat.Percentile(monetaryList, Q1),
                        MonetaryQ3 = stat.Percentile(monetaryList, Q3),
                        MonetaryMin = monetaryList.Min(),
                        MonetaryMax = monetaryList.Max(),

                        FrequencyQ1 = stat.Percentile(frequencyList, Q1),
                        FrequencyQ3 = stat.Percentile(frequencyList, Q3),
                        FrequencyMax = frequencyList.Max(),
                        FrequencyMin = frequencyList.Min(),

                        RecencyQ1 = stat.Percentile(recencyList, Q1),
                        RecencyQ3 = stat.Percentile(recencyList, Q3),
                        RecencyMin = recencyList.Min(),
                        RecencyMax = recencyList.Max()
                    };

                    SaveSegments(segments);
                }
            }

            return null;
        }


        public List<CustomerItem> RebuildRFMScores(List<InvoiceItem> list)
        {
            var customers = list.GroupBy(x => x.ContactId)
                .Select(x => new CustomerItem
                {
                    ContactId = x.Key,
                    Invoices =  list
                }).ToList();

            foreach (CustomerItem customer in customers)
            {
                // Monetary

                double m = 0;
                foreach (InvoiceItem invoice in customer.Invoices)
                {
                    m +=invoice.Value;
                }

                if (m <= 0) // broken data
                    m = 1;

                customer.RFM.M = (int)m;

                // Recency

                DateTime? minDate = customer.Invoices.Where(x => x.Timestamp.Year != 1).DefaultIfEmpty().Min(x => x?.Timestamp);
                DateTime? maxDate = customer.Invoices.Where(x => x.Timestamp.Year != 1).DefaultIfEmpty().Max(x => x?.Timestamp);

                customer.RFM.R = (int)(minDate.HasValue && maxDate.HasValue ? (maxDate - minDate).Value.TotalDays + 1 : 1);

                // Frequency

                customer.RFM.F = (int)customer.Invoices.Count;

                if (customer.RFM.F == 0) // broken data
                    customer.RFM.F = 1;
            }

            // Calculate 

            int maxScore = 3;

            var rList = customers.OrderByDescending(x => x.RFM.R).ToList().Partition(maxScore);
            int rValue = maxScore;
            foreach (var rPart in rList)
            {
                foreach (var customer in rPart)
                {
                    customer.RFM.R = (int)rValue;
                }

                rValue = rValue - 1;
            }

            var mList = customers.OrderByDescending(x => x.RFM.M).ToList().Partition(maxScore);

            int mValue = maxScore;
            foreach (var mPart in mList)
            {
                foreach (var customer in mPart)
                {
                    customer.RFM.M = (int)mValue;
                }

                mValue = mValue - 1;
            }

            var fList = customers.OrderByDescending(x => x.RFM.F).ToList().Partition(maxScore);
            int fValue = maxScore;
            foreach (var fPart in fList)
            {
                foreach (var customer in fPart)
                {
                    customer.RFM.F = (int)fValue;
                }

                fValue = fValue - 1;
            }

            return customers;
        }

        public Segments GetSegmentsWithoutRebuild()
        {
            return Load();
        }

        // storage

        private void Save(Segments segments)
        {
            using (var file = File.Create(string.Format($"{Consts.WorkFolder}/{Consts.SegmentsModel}")))
            {
                Serializer.Serialize(file, segments);
            }
        }

        private Segments Load()
        {
            Segments model = null;
            using (var file = File.OpenRead(string.Format($"{Consts.WorkFolder}/{Consts.SegmentsModel}")))
            {
                model = Serializer.Deserialize<Segments>(file);
            }

            return model;
        }

    }
}
