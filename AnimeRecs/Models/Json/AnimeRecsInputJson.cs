using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using AnimeCompatibility;

namespace AnimeRecs.Models
{
    public class AnimeRecsInputJson : IValidatableObject
    {
        [Required]
        public string MalName { get; set; }
        
        public decimal? GoodCutoff { get; set; }
        public decimal? OkCutoff { get; set; }

        [Range(0, 100)]
        public decimal? GoodPercentile { get; set; }
        [Range(0, 100)]
        public decimal? DislikedPercentile { get; set; }

        public IGoodOkBadFilter GoodOkBadFilter
        {
            get
            {
                if (GoodCutoff.HasValue)
                {
                    return new CutoffGoodOkBadFilter() { GoodCutoff = GoodCutoff.Value, OkCutoff = OkCutoff.Value };
                }
                else if (GoodPercentile.HasValue)
                {
                    return new PercentileGoodOkBadFilter() { RecommendedPercentile = decimal.ToDouble(GoodPercentile.Value), DislikedPercentile = decimal.ToDouble(this.DislikedPercentile.Value) };
                }
                else
                {
                    return new PercentileGoodOkBadFilter();
                }
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (GoodCutoff.HasValue)
            {
                if (!OkCutoff.HasValue)
                {
                    yield return new ValidationResult("An Ok cutoff is required.");
                }
                else
                {
                    if (OkCutoff.Value > GoodCutoff.Value)
                    {
                        yield return new ValidationResult("Ok cutoff must not be greater than good cutoff.");
                    }
                }
            }
            else if (GoodPercentile.HasValue)
            {
                if (!DislikedPercentile.HasValue)
                {
                    yield return new ValidationResult("A disliked percentile is required");
                }
                else
                {
                    if (GoodPercentile.Value + DislikedPercentile.Value > 100)
                    {
                        yield return new ValidationResult("Good percentile plus disliked percentile must not be greater than 100");
                    }
                }
            }
        }
    }
}