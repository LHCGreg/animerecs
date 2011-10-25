using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AnimeRecs.Common;

namespace AnimeRecs.Models
{
    public class MockRecommendorCache : IRecommendorCache
    {
        public IEnumerable<RecommendorJson> GetRecommendors()
        {
            return new List<RecommendorJson>()
            {
                new RecommendorJson()
                {
                    Name = "LordHighCaptain",
                    Recommendations = new List<RecommendedAnimeJson>()
                    {
                        new RecommendedAnimeJson()
                        {
                            MalId = 24,
                            Name = "School Rumble",
                            Rating = 8,
                        },
                        new RecommendedAnimeJson()
                        {
                            MalId = 43,
                            Name = "Ghost in the Shell",
                            Rating = 8,
                        },
                        new RecommendedAnimeJson()
                        {
                            MalId = 66,
                            Name = "Azumanga Daioh",
                            Rating = 8
                        },
                        new RecommendedAnimeJson()
                        {
                            MalId = 72,
                            Name = "Full Metal Panic? Fumoffu",
                            Rating = 8
                        },
                        new RecommendedAnimeJson()
                        {
                            MalId = 73,
                            Name = "Full Metal Panic! The Second Raid",
                            Rating = 8,
                        },
                        new RecommendedAnimeJson()
                        {
                            MalId = 121,
                            Name = "Fullmetal Alchemist",
                            Rating = 9
                        },
                        new RecommendedAnimeJson()
                        {
                            MalId = 134,
                            Name = "Gunslinger Girl",
                            Rating = 10
                        }

                    }
                },
                new RecommendorJson()
                {
                    Name = "TheAlmightyMoo",
                    Recommendations = new List<RecommendedAnimeJson>()
                    {
                        new RecommendedAnimeJson()
                        {
                            MalId = 134,
                            Name = "Gunslinger Girl",
                            Rating = 10
                        }
                    }
                }
            };
        }

        public void Dispose()
        {
            ;
        }
    }
}