using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnimeRecs.RecEngine.MAL;
using AnimeRecs.RecService.DTO;

namespace AnimeRecs.RecService.Registrations.RecSources
{
    [JsonRecSource(RecSourceTypes.MostPopular)]
    internal class MostPopularJsonRecSource : TrainableJsonRecSource<MalMostPopularRecSource, MalUserListEntries,
        IEnumerable<RecEngine.MostPopularRecommendation>, RecEngine.MostPopularRecommendation, GetMalRecsResponse<DTO.MostPopularRecommendation>,
        DTO.MostPopularRecommendation>
    {
        public MostPopularJsonRecSource(LoadRecSourceRequest<MostPopularRecSourceParams> request)
            : base(CreateRecSourceFromRequest(request))
        {
            ;
        }

        private static MalMostPopularRecSource CreateRecSourceFromRequest(LoadRecSourceRequest<MostPopularRecSourceParams> request)
        {
            return new MalMostPopularRecSource(
                minEpisodesToCountIncomplete: request.Params.MinEpisodesToCountIncomplete,
                useDropped: request.Params.UseDropped
            );
        }

        protected override MalUserListEntries GetRecSourceInputFromRequest(MalUserListEntries animeList, GetMalRecsRequest recRequest)
        {
            return animeList;
        }

        protected override void SetSpecializedRecommendationProperties(DTO.MostPopularRecommendation dtoRec, RecEngine.MostPopularRecommendation engineRec)
        {
            dtoRec.NumRatings = engineRec.NumRatings;
            dtoRec.PopularityRank = engineRec.PopularityRank;
        }

        protected override string RecommendationType { get { return RecommendationTypes.MostPopular; } }
        public override string RecSourceType { get { return RecSourceTypes.MostPopular; } }
    }
}
