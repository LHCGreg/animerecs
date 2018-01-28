using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimeRecs.WebCore.Models
{
    public class RecResultsAsHtmlJson
    {
        public string Html { get; set; }

        public RecResultsAsHtmlJson()
        {
            ;
        }

        public RecResultsAsHtmlJson(string html)
        {
            Html = html;
        }
    }
}
