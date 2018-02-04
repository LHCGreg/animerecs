using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.DAL
{
    internal static class PgHelpers
    {
        /// <summary>
        /// Creates a string literal suitable for use with PostgreSQL by placing the string in "double quotes" and replacing
        /// any quote characters (') with two quote characters ('').
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string QuotePgString(string str)
        {
            return "'" + str.Replace("'", "''") + "'";
        }
    }
}
