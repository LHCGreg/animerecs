using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.RecEngine
{
    public class BasicRecommendation : IRecommendation
    {
        public int ItemId { get; private set; }

        public BasicRecommendation(int itemId)
        {
            ItemId = itemId;
        }
    }
}
