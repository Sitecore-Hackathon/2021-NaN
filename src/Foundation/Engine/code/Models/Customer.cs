using System;
using System.Collections.Generic;

namespace Hackathon.NaN.MLBox.Foundation.ProcessingEngine.Models
{
    public class Customer : CustomerBusinessValue
    {
        public Customer()
        {
            Invoices = new List<PurchaseInvoice>();
        }
        public Guid CustomerId { get; set; }
        public IList<PurchaseInvoice> Invoices { get; set; }
    }

    public class CustomerExcel : CustomerBusinessValue
    {
        public CustomerExcel()
        {
            Invoices = new List<PurchaseInvoiceExcel>();
        }
        public int CustomerId { get; set; }
        public IList<PurchaseInvoiceExcel> Invoices { get; set; }
    }
}
