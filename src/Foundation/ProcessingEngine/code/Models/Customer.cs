using System;
using System.Collections.Generic;

namespace Hackathon.NaN.MLBox.Foundation.ProcessingEngine.Models
{
    public class Customer : CustomerBusinessValue
    {
        public Customer()
        {
            Invoices = new List<PurchaseInvoiceExcel>();
        }
        public Guid CustomerId { get; set; }
        public IList<PurchaseInvoiceExcel> Invoices { get; set; }
    }
}
