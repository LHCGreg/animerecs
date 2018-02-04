using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.RecEngine.MAL
{
    public class MalPositiveFeedbackInput : IInputForUser
    {
        public MalUserListEntries AnimeList { get; private set; }
        public double? TargetFraction { get; private set; }
        public decimal? TargetScore { get; private set; }

        public MalPositiveFeedbackInput(MalUserListEntries animeList, double targetFraction)
        {
            AnimeList = animeList;
            TargetFraction = targetFraction;
        }

        public MalPositiveFeedbackInput(MalUserListEntries animeList, decimal targetScore)
        {
            AnimeList = animeList;
            TargetScore = targetScore;
        }

        public bool ItemIsOkToRecommend(int itemId)
        {
            return AnimeList.ItemIsOkToRecommend(itemId);
        }

        public bool ContainsItem(int itemId)
        {
            return AnimeList.ContainsItem(itemId);
        }
    }
}
