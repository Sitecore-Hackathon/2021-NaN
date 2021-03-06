using System.Collections.Generic;

namespace Hackathon.NaN.MLBox.Foundation.ProcessingEngine.Models
{
    public class Customer : CustomerBusinessValue
    {
        public Customer()
        {
            Invoices = new List<PurchaseInvoice>();
        }
        public int CustomerId { get; set; }
        public IList<PurchaseInvoice> Invoices { get; set; }
    }
}
