using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnimeRecs.RecEngine;
using AnimeRecs.RecEngine.MAL;

namespace AnimeRecs.RecService.ClientLib
{
    public class MalRecResults<TResults>
        where TResults : IEnumerable<IRecommendation>
    {
        public TResults Results { get; private set; }
        public IDictionary<int, MalAnime> AnimeInfo { get; private set; }
        public string RecommendationType { get; private set; }

        public MalRecResults(TResults results, IDictionary<int, MalAnime> animeInfo, string recommendationType)
        {
            Results = results;
            AnimeInfo = animeInfo;
            RecommendationType = recommendationType;
        }
    }
}
