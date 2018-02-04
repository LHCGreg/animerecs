using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AnimeRecs.UpdateStreams
{
    static class Utils
    {
        public static string PossiblyRelativeUrlToAbsoluteUrl(string possiblyRelativeUrl, string sourceUrl)
        {
            Uri possiblyRelativeUri = new Uri(possiblyRelativeUrl, UriKind.RelativeOrAbsolute);
            if (possiblyRelativeUri.IsAbsoluteUri)
            {
                return possiblyRelativeUri.ToString();
            }
            else
            {
                return new Uri(new Uri(sourceUrl), possiblyRelativeUrl).ToString();
            }
        }

        public static string DecodeHtmlAttribute(string rawAttributeText)
        {
            return WebUtility.HtmlDecode(rawAttributeText);
        }

        public static string DecodeHtmlBody(string rawBody)
        {
            return WebUtility.HtmlDecode(rawBody);
        }
    }
}
