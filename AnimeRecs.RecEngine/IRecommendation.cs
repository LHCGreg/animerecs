using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.RecEngine
{
    public interface IRecommendation
    {
        int ItemId { get; }
    }
}
