using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AnimeCompatibility;

namespace AnimeRecs.Models
{
    public class RecommendorCompatibility
    {
        public ICollection<IAnimeListEntry> RecommendedAnime { get; set; }
        public ICollection<IAnimeListEntry> RecommendedAnimeLiked { get; set; }
        
        /// <summary>
        /// Anime not liked out of the recommendations in common
        /// </summary>
        public ICollection<IAnimeListEntry> RecommendedAnimeNotliked { get; set; }
        public ICollection<IAnimeListEntry> RecommendedAnimeInCommon { get; set; }
        public ICollection<IAnimeListEntry> RecommendedAnimeNotInCommon { get; set; }

        public Tuple<double, double> Compatibility80PercentConfidenceInterval { get; set; }
        public Tuple<double, double> Compatibility90PercentConfidenceInterval { get; set; }
        public Tuple<double, double> Compatibility92PercentConfidenceInterval { get; set; }
        public Tuple<double, double> Compatibility95PercentConfidenceInterval { get; set; }

        public double FractionLiked { get { return ((double)RecommendedAnimeLiked.Count) / RecommendedAnimeInCommon.Count; } }

        public RecommendorCompatibility(ICollection<IAnimeListEntry> recommendedAnime, ICollection<IAnimeListEntry> recommendedAnimeLiked, ICollection<IAnimeListEntry> recommendedAnimeInCommon)
        {
            RecommendedAnime = recommendedAnime;
            RecommendedAnimeLiked = recommendedAnimeLiked;
            RecommendedAnimeInCommon = recommendedAnimeInCommon;

            RecommendedAnimeNotliked = new HashSet<IAnimeListEntry>(recommendedAnime.Where(
                anime => recommendedAnimeInCommon.Contains(anime) && !recommendedAnimeLiked.Contains(anime)));

            RecommendedAnimeNotInCommon = new HashSet<IAnimeListEntry>(recommendedAnime.Where(
                anime => !recommendedAnimeInCommon.Contains(anime)));

            Compatibility80PercentConfidenceInterval = GetCompatibility80PercentConfidenceInterval();
            Compatibility90PercentConfidenceInterval = GetCompatibility90PercentConfidenceInterval();
            Compatibility92PercentConfidenceInterval = GetCompatibility92PercentConfidenceInteval();
            Compatibility95PercentConfidenceInterval = GetCompatibility95PercentConfidenceInterval();
        }

        public Tuple<double, double> GetCompatibility80PercentConfidenceInterval()
        {
            return GetCompatibilityConfidenceInterval(1.28);
        }

        public Tuple<double, double> GetCompatibility90PercentConfidenceInterval()
        {
            return GetCompatibilityConfidenceInterval(1.645);
        }

        public Tuple<double, double> GetCompatibility92PercentConfidenceInteval()
        {
            return GetCompatibilityConfidenceInterval(1.75);
        }

        public Tuple<double, double> GetCompatibility95PercentConfidenceInterval()
        {
            return GetCompatibilityConfidenceInterval(1.96);
        }

        private Tuple<double, double> GetCompatibilityConfidenceInterval(double zAlphaOver2)
        {
            Tuple<double, double> unclampedConfidenceInterval = GetConfidenceInterval(FractionLiked, zAlphaOver2, RecommendedAnimeInCommon.Count);
            
            // Clamp the interval to [0,1] to account for any floating point weirdness bringing an endpoint outside
            return new Tuple<double, double>(unclampedConfidenceInterval.Item1 < 0 ? 0 : unclampedConfidenceInterval.Item1,
                unclampedConfidenceInterval.Item2 > 1 ? 1 : unclampedConfidenceInterval.Item2);
        }

        private static Tuple<double, double> GetConfidenceInterval(double p, double zAlphaOver2, int n)
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

            return new Tuple<double, double>(lowerLimit, upperLimit);
        }
    }
}