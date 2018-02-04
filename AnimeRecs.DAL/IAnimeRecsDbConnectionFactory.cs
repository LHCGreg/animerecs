using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace AnimeRecs.DAL
{
    public interface IAnimeRecsDbConnectionFactory
    {
        IAnimeRecsDbConnection GetConnection();
    }
}
