using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.RecEngine
{
    internal static class ConfidenceInterval
    {
        public static Tuple<double, double> Get95PercentConfidenceInterval(double p, int n)
        {
            return GetConfidenceInterval(p, n, 1.96);
        }

        private static Tuple<double, double> GetConfidenceInterval(double p, int n, double zAlphaOver2)
        {
            double q = 1 - p;

            double lowerLimit = (p + ((zAlphaOver2 * zAlphaOver2) / (2 * n)) - (zAlphaOver2 *
                Math.Sqrt(((p * q) / n) + ((zAlphaOver2 * zAlphaOver2) / (4 * n * n)))))

                /

                (1 + ((zAlphaOver2 * zAlphaOver2) / n));

            double upperLimit = (p + ((zAlphaOver2 * zAlphaOver2) / (2 * n)) + (zAlphaOver2 *
                Math.Sqrt(((p * q) / n) + ((zAlphaOver2 * zAlphaOver2) / (4 * n * n)))))

                /

                (1 + ((zAlphaOver2 * zAlphaOver2) / n));

            // Clamp the interval to [0,1] to account for any floating point weirdness bringing an endpoint outside
            return new Tuple<double, double>(lowerLimit < 0 ? 0 : lowerLimit, upperLimit > 1 ? 1 : upperLimit);
        }
    }
}
