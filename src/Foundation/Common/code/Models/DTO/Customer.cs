using System.Collections.Generic;

namespace Hackathon.MLBox.Foundation.Common.Models.DTO
{
    /// <summary>
    /// Customer
    /// </summary>
    public class Customer
    {
        public Customer()
        {
            Invoices = new List<Invoice>();
        }

        /// <summary>
        /// Customer Id
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Transaction list
        /// </summary>
        public IList<Invoice> Invoices { get; set; }
    }
}
