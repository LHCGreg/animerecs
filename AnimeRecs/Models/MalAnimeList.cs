using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AnimeCompatibility;

namespace AnimeRecs.Models
{
    public class MalAnimeList
    {
        private IList<MyAnimeListEntry> m_list = new List<MyAnimeListEntry>();
        public IList<MyAnimeListEntry> List { get { return m_list; } set { m_list = value; } }
    }
}