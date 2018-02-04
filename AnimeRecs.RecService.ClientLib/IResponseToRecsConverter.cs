using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnimeRecs.RecService.DTO;
using AnimeRecs.RecEngine;

namespace AnimeRecs.RecService.ClientLib
{
    internal interface IResponseToRecsConverter<TResponse>
        where TResponse : GetMalRecsResponse
    {
        IEnumerable<IRecommendation> ConvertResponseToRecommendations(TResponse response);
    }
}

namespace AnimeRecs.RecService.ClientLib.Registrations
{
    internal partial class ResponseToRecsConverter
    {
        internal static readonly string ConvertMethodName = "ConvertResponseToRecommendations";
    }
}
