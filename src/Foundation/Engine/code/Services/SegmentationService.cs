using System;
using System.Collections.Generic;
using System.Linq;
using Hackathon.MLBox.Foundation.Common.Extensions;
using Hackathon.MLBox.Foundation.Common.Models.Sitecore;

namespace Hackathon.MLBox.Foundation.Engine.Services
{
    public class SegmentationService
    {

        public void GetSegments

        // RFM from 111 to 333
        private int MaxValue = 3;
      
        public List<CustomerItem> CalculateRfmScores(List<InvoiceItem> list)
        {
            var customers = list.GroupBy(x => x.ContactId)
                .Select(x => new CustomerItem
                {
                    ContactId = x.Key,
                    Invoices =  list
                }).ToList();
            

            // Calculate pre-values

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

            int maxScore = MaxValue;

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
    }
}
