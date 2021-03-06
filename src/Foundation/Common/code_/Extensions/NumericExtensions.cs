namespace Hackathon.MLBox.Foundation.Common.Extensions
{
    using System;
    
    /// <summary>
    /// Numeric extensions
    /// </summary>
    public static class NumericExtensions
    {
        
        public static double Percentile(double[] sortedData, double p)
        {
            if (p >= 100.0d) 
                return sortedData[sortedData.Length - 1];

            double position = (sortedData.Length + 1) * p / 100.0;
            double left = 0.0d;
            double right = 0.0d;

            double n = p / 100.0d * (sortedData.Length - 1) + 1.0d;

            if (position >= 1)
            {
                left = sortedData[(int)Math.Floor(n) - 1];
                right = sortedData[(int)Math.Floor(n)];
            }
            else
            {
                left = sortedData[0];
                right = sortedData[1];
            }

            if (Equals(left, right))
                return left;

            double part = n - Math.Floor(n);
            return left + part * (right - left);
        } 
    }
}
