using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using AnimeCompatibility;

namespace AnimeRecs.Models
{
    public class AnimeRecsInputJson
    {
        [Required]
        public string MalName { get; set; }
        
        public decimal? GoodCutoff { get; set; }

        [Range(0, 100)]
        public decimal? GoodPercentile { get; set; }

        public override string ToString()
        {
            return MalName;
        }
    }
}