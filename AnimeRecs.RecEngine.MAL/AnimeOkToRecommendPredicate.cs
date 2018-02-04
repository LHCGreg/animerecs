using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.RecEngine.MAL
{
    public delegate bool AnimeOkToRecommendPredicate(MalUserListEntries userAnimeList, int animeId);
}
