using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.RecEngine
{
    public interface IInputForUserWithItemIds
    {
        ICollection<int> ItemIds { get; }
    }
}
