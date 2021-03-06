namespace Hackathon.MLBox.Foundation.Common.Models.Sitecore
{
    using System;

    public class InvoiceItem
    {
        /// <summary>
        /// Customer contcat Id
        /// </summary>
        public Guid ContactId { get; set; }

        /// <summary>
        /// Transaction time
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Amount
        /// </summary>
        public double Value { get; set; }
    }
}
