using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnimeRecs.RecEngine.MAL;
using AnimeRecs.RecService.DTO;

namespace AnimeRecs.RecService.Registrations.RecSources
{
    [JsonRecSource(RecSourceTypes.AverageScore)]
    internal class AverageScoreJsonRecSource :
        TrainableJsonRecSource<MalAverageScoreRecSource, MalUserListEntries, IEnumerable<RecEngine.AverageScoreRecommendation>,
        RecEngine.AverageScoreRecommendation, GetMalRecsResponse<DTO.AverageScoreRecommendation>, DTO.AverageScoreRecommendation>
    {
        public AverageScoreJsonRecSource(LoadRecSourceRequest<AverageScoreRecSourceParams> request)
            : base(CreateRecSourceFromRequest(request))
        {
            ;
        }

        private static MalAverageScoreRecSource CreateRecSourceFromRequest(LoadRecSourceRequest<AverageScoreRecSourceParams> request)
        {
            return new MalAverageScoreRecSource(
                minEpisodesToCountIncomplete: request.Params.MinEpisodesToCountIncomplete,
                useDropped: request.Params.UseDropped,
                minUsersToCountAnime: request.Params.MinUsersToCountAnime
            );
        }
        
        protected override MalUserListEntries GetRecSourceInputFromRequest(MalUserListEntries animeList, GetMalRecsRequest recRequest)
        {
            return animeList;
        }

        protected override void SetSpecializedRecommendationProperties(DTO.AverageScoreRecommendation dtoRec, RecEngine.AverageScoreRecommendation engineRec)
        {
            dtoRec.AverageScore = engineRec.AverageScore;
            dtoRec.NumRatings = engineRec.NumRatings;
        }

        protected override string RecommendationType { get { return RecommendationTypes.AverageScore; } }
        public override string RecSourceType { get { return RecSourceTypes.AverageScore; } }
    }
}
