using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnimeRecs.Common;

namespace AnimeRecs.Models
{
    public interface IRecommendorCache : IDisposable
    {
        IEnumerable<RecommendorJson> GetRecommendors();
    }
}
