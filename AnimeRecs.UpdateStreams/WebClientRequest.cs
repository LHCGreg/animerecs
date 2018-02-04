using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AnimeRecs.UpdateStreams
{
    class WebClientRequest
    {
        public Uri URL { get; set; }
        public string UserAgent { get; set; }
        public string Accept { get; set; }
        public List<KeyValuePair<string, string>> Headers { get; set; }
        public List<KeyValuePair<string, string>> PostParameters { get; set; }

        public WebClientRequest(string url)
        {
            URL = new Uri(url);
            Headers = new List<KeyValuePair<string, string>>();
            PostParameters = new List<KeyValuePair<string, string>>();
        }
    }
}
