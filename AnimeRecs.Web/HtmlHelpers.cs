using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnimeRecs.DAL;
using AnimeRecs.RecEngine.MAL;
using AnimeRecs.Web.Models;
using MalApi;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace AnimeRecs.Web
{
    // MUST be public, not internal, for razor to access it
    public static class HtmlHelpers
    {
        public static IHtmlContent Attribute(string text)
        {
            return new HtmlString(AttributeString(text));
        }

        /// <summary>
        /// Puts the text in double quotes and escapes necessary attribute characters.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static string AttributeString(string text)
        {
            // " & <
            StringBuilder encodedString = new StringBuilder(text.Length + 2);
            encodedString.Append("\"");
            foreach (char c in text)
            {
                switch (c)
                {
                    case '"':
                        encodedString.Append("&quot;");
                        break;
                    case '&':
                        encodedString.Append("&amp;");
                        break;
                    case '<':
                        encodedString.Append("&lt;");
                        break;
                    default:
                        encodedString.Append(c);
                        break;
                }
            }
            encodedString.Append("\"");
            return encodedString.ToString();
        }

        private static string EncodeToString(string text)
        {
            return System.Text.Encodings.Web.HtmlEncoder.Default.Encode(text);
        }

        public static IHtmlContent GetStreamLinksHtml(int malAnimeId, GetRecsViewModel model, IUrlHelper url)
        {
            List<string> streamLinks = new List<string>();
            if (model.StreamsByAnime.ContainsKey(malAnimeId))
            {
                foreach (streaming_service_anime_map serviceMap in model.StreamsByAnime[malAnimeId]
                    .OrderBy(map => map.streaming_service_id))
                {
                    StreamingService service = (StreamingService)serviceMap.streaming_service_id;
                    string imagePath;
                    string altText;
                    string titleText;
                    switch (service)
                    {
                        case StreamingService.Crunchyroll:
                            imagePath = url.Content("Content/crunchyroll_icon.png");
                            altText = "Crunchyroll";
                            titleText = "Watch on Crunchyroll";
                            break;
                        case StreamingService.Funimation:
                            imagePath = url.Content("Content/funimation_icon.png");
                            altText = "Funimation";
                            titleText = "Watch on Funimation";
                            break;
                        case StreamingService.Viz:
                            imagePath = url.Content("Content/viz_icon.png");
                            altText = "Viz";
                            titleText = "Watch on Viz";
                            break;
                        case StreamingService.Hulu:
                            imagePath = url.Content("Content/hulu_icon.png");
                            altText = "Hulu";
                            titleText = "Watch on Hulu";
                            break;
                        case StreamingService.Viewster:
                            imagePath = url.Content("Content/viewster_icon.png");
                            altText = "Viewster";
                            titleText = "Watch on Viewster";
                            break;
                        case StreamingService.Daisuki:
                            imagePath = url.Content("Content/daisuki_icon.png");
                            altText = "Daisuki";
                            titleText = "Watch on Daisuki";
                            break;
                        case StreamingService.AnimeNetwork:
                            imagePath = url.Content("Content/anime_network_icon.png");
                            altText = "Anime Network";
                            titleText = "Watch on The Anime Network";
                            break;
                        case StreamingService.AmazonPrime:
                            imagePath = url.Content("Content/amazon_prime_icon.png");
                            altText = "Amazon Prime";
                            titleText = "Watch on Amazon Prime";
                            break;
                        case StreamingService.AmazonAnimeStrike:
                            imagePath = url.Content("Content/amazon_anime_strike_icon.png");
                            altText = "Anime Strike";
                            titleText = "Watch on Amazon Anime Strike";
                            break;
                        case StreamingService.Hidive:
                            imagePath = url.Content("Content/hidive_icon.png");
                            altText = "Hidive";
                            titleText = "Watch on Hidive";
                            break;
                        default:
                            continue;
                    }

                    string imgHtml = string.Format(@"<img src={0} title={1} alt={2} />",
                        AttributeString(imagePath), AttributeString(titleText), AttributeString(altText));

                    string linkHtml = string.Format(@"<a href={0}>{1}</a>", AttributeString(serviceMap.streaming_url), imgHtml);
                    streamLinks.Add(linkHtml);
                }
            }

            string linksHtml = string.Join(" ", streamLinks);
            return new HtmlString(linksHtml);
        }

        public static string MalAnimeTypeAsString(MalAnimeType type)
        {
            // These could come from the DB...
            switch (type)
            {
                case MalAnimeType.Tv:
                    return "TV";
                case MalAnimeType.Movie:
                    return "Movie";
                case MalAnimeType.Ova:
                    return "OVA";
                case MalAnimeType.Ona:
                    return "ONA";
                case MalAnimeType.Special:
                    return "Special";
                case MalAnimeType.Music:
                    return "Music";
                default:
                    return "???";
            }
        }

        public static string MalAnimeStatusAsString(CompletionStatus status)
        {
            // This could come from the DB...
            switch (status)
            {
                case CompletionStatus.Completed:
                    return "Completed";
                case CompletionStatus.Dropped:
                    return "Dropped";
                case CompletionStatus.OnHold:
                    return "On Hold";
                case CompletionStatus.PlanToWatch:
                    return "Plan to Watch";
                case CompletionStatus.Watching:
                    return "Watching";
                default:
                    return "???";
            }
        }

        public static string GetMalRatingString(decimal? num)
        {
            if (num == null)
            {
                return "-";
            }
            else
            {
                return ((int)num.Value).ToString(CultureInfo.InvariantCulture);
            }
        }

        public static IHtmlContent GetRecommendedMalAnimeHtml(int malAnimeId, GetRecsViewModel model)
        {
            string animeTitle = model.Results.AnimeInfo[malAnimeId].Title;
            string encodedString = string.Format(@"<a href={0} class=""recommendation"">{1}</a>",
                AttributeString(GetMalAnimeUrl(malAnimeId, animeTitle)), EncodeToString(animeTitle));

            return new HtmlString(encodedString);
        }

        public static IHtmlContent GetWithheldMalAnimeHtml(int malAnimeId, string animeTitle)
        {
            string encodedString = string.Format(@"<a href={0} class=""recommendation"">{1}</a>",
                AttributeString(GetMalAnimeUrl(malAnimeId, animeTitle)), EncodeToString(animeTitle));

            return new HtmlString(encodedString);
        }

        public static string GetMalAnimeUrl(int malAnimeId, string animeTitle)
        {
            // TODO: Add url-sanitized anime name at end for a friendlier URL?
            return string.Format("https://myanimelist.net/anime/{0}", malAnimeId.ToString(CultureInfo.InvariantCulture));
        }

        public static string GetMalListUrl(string username)
        {
            return string.Format("https://myanimelist.net/animelist/{0}", Uri.EscapeUriString(username));
        }

        public static AnimeRecsRecommendationType GetRecommendationType(MalAnimeRecsRecommenderUser recommender,
            MalAnimeRecsRecommenderRecommendation rec, IDictionary<int, MalListEntry> userAnimeList)
        {
            if (recommender.RecsNotInCommon.Contains(rec))
            {
                return AnimeRecsRecommendationType.UsefulRecommendation;
            }
            else if (recommender.RecsInconclusive.Contains(rec))
            {
                if (userAnimeList.ContainsKey(rec.MalAnimeId) &&
                    (userAnimeList[rec.MalAnimeId].Status == CompletionStatus.Watching
                    || userAnimeList[rec.MalAnimeId].Status == CompletionStatus.PlanToWatch
                    || userAnimeList[rec.MalAnimeId].Status == CompletionStatus.OnHold))
                {
                    return AnimeRecsRecommendationType.UsefulRecommendation;
                }
                else
                {
                    return AnimeRecsRecommendationType.Inconclusive;
                }
            }
            else if (recommender.RecsLiked.Contains(rec))
            {
                return AnimeRecsRecommendationType.Liked;
            }
            else
            {
                return AnimeRecsRecommendationType.NotLiked;
            }
        }
    }
}
