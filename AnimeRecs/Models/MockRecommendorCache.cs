using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnimeRecs.Models
{
    public class MockRecommendorCache : IRecommendorCache
    {
        public IEnumerable<Recommendor> Recommendors
        {
            get
            {
                return new List<Recommendor>()
                {
                    new Recommendor()
                    {
                        Name = "LordHighCaptain",
                        Recommendations = new List<AnimeJson>()
                        {
                            new AnimeJson()
                            {
                                MalId = 24,
                                Name = "School Rumble",
                                Rating = 8,
                            },
                            new AnimeJson()
                            {
                                MalId = 43,
                                Name = "Ghost in the Shell",
                                Rating = 8,
                            },
                            new AnimeJson()
                            {
                                MalId = 66,
                                Name = "Azumanga Daioh",
                                Rating = 8
                            },
                            new AnimeJson()
                            {
                                MalId = 72,
                                Name = "Full Metal Panic? Fumoffu",
                                Rating = 8
                            },
                            new AnimeJson()
                            {
                                MalId = 73,
                                Name = "Full Metal Panic! The Second Raid",
                                Rating = 8,
                            },
                            new AnimeJson()
                            {
                                MalId = 121,
                                Name = "Fullmetal Alchemist",
                                Rating = 9
                            },
                            new AnimeJson()
                            {
                                MalId = 134,
                                Name = "Gunslinger Girl",
                                Rating = 10
                            }

                        }
                    },
                    new Recommendor()
                    {
                        Name = "TheAlmightyMoo",
                        Recommendations = new List<AnimeJson>()
                        {
                            new AnimeJson()
                            {
                                MalId = 134,
                                Name = "Gunslinger Girl",
                                Rating = 10
                            }
                        }
                    }
                };
            }
        }

        public void Dispose()
        {
            ;
        }
    }
}