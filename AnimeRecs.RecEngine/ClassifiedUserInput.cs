using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.RecEngine
{
    public class ClassifiedUserInput<TInput>
    {
        public ClassifiedUserInput(TInput liked, TInput notLiked, TInput other)
        {
            Liked = liked;
            NotLiked = notLiked;
            Other = other;
        }

        public TInput Liked { get; private set; }
        public TInput NotLiked { get; private set; }

        /// <summary>
        /// "Normal" input should be either Liked or NotLiked. But some input types may have additional entries, such as "plan to watch",
        /// which do not belong in either. Such entries should be put here. Liked + NotLiked + Other should be a complete picture of the
        /// input.
        /// </summary>
        public TInput Other { get; private set; }
    }
}
