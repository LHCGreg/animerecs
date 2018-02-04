using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.RecEngine.Utils
{
    // Why isn't this class in the BCL? -_-
    public class DelegateComparer<T> : IComparer<T>
    {
        private Comparison<T> m_comparison;

        public DelegateComparer(Comparison<T> comparison)
        {
            m_comparison = comparison;
        }

        public int Compare(T x, T y)
        {
            return m_comparison(x, y);
        }
    }
}
