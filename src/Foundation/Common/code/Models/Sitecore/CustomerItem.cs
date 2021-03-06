namespace Hackathon.MLBox.Foundation.Common.Models.Sitecore
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
            Invoices = new List<InvoceItem>();
        }

        /// <summary>
        /// Contact Id
        /// </summary>
        public Guid ContactId { get; set; }

        /// <summary>
        /// Invoices
        /// </summary>
        public IList<InvoceItem> Invoices { get; set; }
    }
}
