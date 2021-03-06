namespace Hackathon.MLBox.Foundation.Shared.Models.Metrics
{
    /// <summary>
    /// RFM metric
    /// </summary>
    public class RFMScore
    {
        /// <summary>
        /// Recency
        /// </summary>
        public int R { get; set; }

        /// <summary>
        /// Frequency
        /// </summary>
        public int F { get; set; }

        /// <summary>
        /// Monetary
        /// </summary>
        public int M { get; set; }
    }
}
