using System;

namespace Hackathon.MLBox.Foundation.Shared.Models.DTO
{
    /// <summary>
    /// Invoice
    /// </summary>
    public class Invoice
    {
        /// <summary>
        /// Price
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Quantity
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Currency
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Invoice number
        /// </summary>
        /// 
        public int Number { get; set; }

        /// <summary>
        /// Transaction time
        /// </summary>
        public DateTime TimeStamp { get; set; }
    }
}
