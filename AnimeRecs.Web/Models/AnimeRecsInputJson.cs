using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AnimeRecs.Web.Models
{
    public class AnimeRecsInputJson
    {
        [Required]
        public string MalName { get; set; }

        /// <summary>
        /// Ids of anime to remove from the user's anime list before passing it to the recommendation engine.
        /// This is useful for subjectively evaluating a rec source. If I remove my absolute favorite anime, does the
        /// rec source put it high in the recommendations?
        /// </summary>
        public IList<int> AnimeIdsToWithhold { get; set; }

        /// <summary>
        /// If non-zero, removes the given percentage of the user's anime list randomly before passing it to the recommendation engine.
        /// This is useful for subjectively evaluating a rec source.
        /// </summary>
        [Range(0, 100)]
        public decimal PercentOfAnimeToWithhold { get; set; }

        // If both GoodCutoff and GoodPercentile are null, use a default

        public decimal? GoodCutoff { get; set; }

        [Range(0, 100)]
        public decimal? GoodPercentile { get; set; }

        // Only applicable for AnimeRecs...should think about how to take parameter specific to a rec source type.
        public bool DisplayDetailedResults { get; set; }

        public string RecSourceName { get; set; }

        public AnimeRecsInputJson()
        {
            AnimeIdsToWithhold = new List<int>();
            PercentOfAnimeToWithhold = 0m;
            DisplayDetailedResults = false;
        }

        public override string ToString()
        {
            return MalName;
        }
    }
}
