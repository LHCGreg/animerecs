using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.RecEngine
{
    public class PercentageSplit<T>
    {
        public ICollection<T> LowerPart { get; set; }
        public ICollection<T> UpperPart { get; set; }
    }
}
