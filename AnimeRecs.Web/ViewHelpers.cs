using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using MalApi;
using System.Web.Mvc;
using AnimeRecs.RecEngine.MAL;
using AnimeRecs.Web.Models.ViewModels;
using AnimeRecs.DAL;

namespace AnimeRecs.Web
{
    public static class ViewHelpers
    {
        public static IHtmlString GetStreamLinksHtml(this HtmlHelper html, int malAnimeId, GetRecsViewModel model, UrlHelper url)
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
                            imagePath = url.Content("~/Content/crunchyroll_icon.png");
                            altText = "Crunchyroll";
                            titleText = "Watch on Crunchyroll";
                            break;
                        case StreamingService.Funimation:
                            imagePath = url.Content("~/Content/funimation_icon.png");
                            altText = "Funimation";
                            titleText = "Watch on Funimation";
                            break;
                        default:
                            continue;
                    }

                    string imgHtml = string.Format(@"<img src=""{0}"" title=""{1}"" alt=""{2}"" />",
                        html.AttributeEncode(imagePath), html.AttributeEncode(titleText), html.AttributeEncode(altText));

                    string linkHtml = string.Format(@"<a href=""{0}"">{1}</a>", serviceMap.streaming_url, imgHtml);
                    streamLinks.Add(linkHtml);
                }
            }

            string linksHtml = string.Join(" ", streamLinks);
            return new HtmlString(linksHtml);
        }
        
        public static IHtmlString GetRecommendedMalAnimeHtml(this HtmlHelper html, int malAnimeId, GetRecsViewModel model)
        {
            string animeTitle = model.Results.AnimeInfo[malAnimeId].Title;
            string encodedString = string.Format(@"<a href=""{0}"" class=""recommendation"">{1}</a>",
                html.AttributeEncode(GetMalAnimeUrl(malAnimeId, animeTitle)), html.Encode(animeTitle));
            return new HtmlString(encodedString);
        }
        
        public static string GetMalAnimeUrl(int malAnimeId, string animeTitle)
        {
            // TODO: Add url-sanitized anime name at end for a friendlier URL
            return string.Format("http://myanimelist.net/anime/{0}", malAnimeId.ToString(CultureInfo.InvariantCulture));
        }

        public static IHtmlString GetRecommenderHtml(this HtmlHelper html, string username)
        {
            string encodedString = string.Format(@"<a href=""{0}"" class=""recommender"">{1}</a>", html.AttributeEncode(GetMalListUrl(username)), html.Encode(username));
            return new HtmlString(encodedString);
        }

        public static string GetMalListUrl(string username)
        {
            return string.Format("http://myanimelist.net/animelist/{0}", Uri.EscapeUriString(username));
        }

        public static string MalAnimeTypeAsString(this HtmlHelper html, MalAnimeType type)
        {
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
                default:
                    return "???";
            }
        }

        public static string MalAnimeStatusAsString(this HtmlHelper html, CompletionStatus status)
        {
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

    public enum AnimeRecsRecommendationType
    {
        // Useful recommendations include "plan to watch", "watching", and "on hold"
        UsefulRecommendation = 0,
        Liked = 1,
        Inconclusive = 2,
        NotLiked = 3
    }
}

// Copyright (C) 2012 Greg Najda
//
// This file is part of AnimeRecs.Web.
//
// AnimeRecs.Web is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// AnimeRecs.Web is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with AnimeRecs.Web.  If not, see <http://www.gnu.org/licenses/>.