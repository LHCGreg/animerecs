using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.RecEngine.Evaluation
{
    public class ItemsForInputAndEvaluation<TInput>
    {
        public TInput ItemsForInput { get; set; }
        public ICollection<int> LikedItemsForEvaluation { get; set; }
        public ICollection<int> UnlikedItemsForEvaluation { get; set; }
    }
}
