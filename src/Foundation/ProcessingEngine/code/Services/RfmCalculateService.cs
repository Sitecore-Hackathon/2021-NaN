using System;
using System.Collections.Generic;
using System.Linq;
using Hackathon.NaN.MLBox.Foundation.ProcessingEngine.Models;
using Hackathon.NaN.MLBox.Foundation.ProcessingEngine.Extensions;

namespace Hackathon.NaN.MLBox.Foundation.ProcessingEngine.Services
{
    public class RfmCalculateService
    {
        // RFM from 111 to 333
        private int MaxValue = 3;
      
        public List<Customer> CalculateRfmScores(List<PurchaseInvoice> list)
        {
            var customers = list.GroupBy(x => x.ContactId)
                .Select(x => new Customer
                {
                    CustomerId = x.Key,
                    Invoices =  list
                }).ToList();
            

            // Calculate pre-values

            foreach (Customer customer in customers)
            {
                // Monetary

                double m = 0;
                foreach (PurchaseInvoice invoice in customer.Invoices)
                {
                    m +=invoice.Value;
                }

                if (m <= 0) // broken data
                    m = 1;

                customer.Monetary = m;

                // Recency

                DateTime? minDate = customer.Invoices.Where(x => x.Timestamp.Year != 1).DefaultIfEmpty().Min(x => x?.Timestamp);
                DateTime? maxDate = customer.Invoices.Where(x => x.Timestamp.Year != 1).DefaultIfEmpty().Max(x => x?.Timestamp);

                customer.Recency = minDate.HasValue && maxDate.HasValue ? (maxDate - minDate).Value.TotalDays + 1 : 1;

                // Frequency

                customer.Frequency = customer.Invoices.Count;

                if (customer.Frequency == 0) // broken data
                    customer.Frequency = 1;
            }

            // Calculate 

            int maxScore = MaxValue;

            var rList = customers.OrderByDescending(x => x.Recency).ToList().Partition(maxScore);
            int rValue = maxScore;
            foreach (var rPart in rList)
            {
                foreach (Customer customer in rPart)
                {
                    customer.R = rValue;
                }

                rValue = rValue - 1;
            }

            var mList = customers.OrderByDescending(x => x.Monetary).ToList().Partition(maxScore);

            int mValue = maxScore;
            foreach (var mPart in mList)
            {
                foreach (Customer customer in mPart)
                {
                    customer.M = mValue;
                }

                mValue = mValue - 1;
            }

            var fList = customers.OrderByDescending(x => x.Frequency).ToList().Partition(maxScore);
            int fValue = maxScore;
            foreach (var fPart in fList)
            {
                foreach (Customer customer in fPart)
                {
                    customer.F = fValue;
                }

                fValue = fValue - 1;
            }

            return customers;

        }
    }
}
