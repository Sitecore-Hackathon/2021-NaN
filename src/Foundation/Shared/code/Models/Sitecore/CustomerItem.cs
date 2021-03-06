using Hackathon.MLBox.Foundation.Shared.Models.Metrics;

namespace Hackathon.MLBox.Foundation.Shared.Models.Sitecore
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Customer data from sitecore
    /// </summary>
    public class CustomerItem
    {
        public CustomerItem()
        {
            Invoices = new List<InvoiceItem>();
            RFM= new RFMScore();
        }

        /// <summary>
        /// Contact Id
        /// </summary>
        public Guid ContactId { get; set; }

        /// <summary>
        /// Invoices
        /// </summary>
        public IList<InvoiceItem> Invoices { get; set; }

        public RFMScore RFM { get; set; }
    }
}
