namespace Hackathon.MLBox.Foundation.Common.Models.Metrics
{
    /// <summary>
    /// RFM metric
    /// </summary>
    public class RFMScore
    {
        /// <summary>
        /// Recency
        /// </summary>
        public float R { get; set; }

        /// <summary>
        /// Frequency
        /// </summary>
        public float F { get; set; }

        /// <summary>
        /// Monetary
        /// </summary>
        public float M { get; set; }
    }
}
