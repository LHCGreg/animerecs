using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Newtonsoft.Json;
using System.IO;
using MongoDB.Driver;
using AnimeCompatibility;
using AnimeRecs.Common;
using MiscUtil.Collections.Extensions;
using MongoDB.Driver.Builders;

namespace AnimeRecs.UpdateCache
{
    class Program
    {
        static void Main(string[] args)
        {
            Logging.SetUpLogging();

            try
            {
                int delayBetweenRequestsMs = int.Parse(ConfigurationManager.AppSettings["DelayBetweenRequestsMs"]);

                // Read list of recommendors from file
                string recommendorFilePath = ConfigurationManager.AppSettings["RecommendorFilePath"];
                RecommendorsInputJson recommendors = ReadRecommendorListFromFile(recommendorFilePath);

                // Connect to mongo
                string mongoConnectionString = ConfigurationManager.ConnectionStrings["Mongo"].ToString();

                Logging.Log.DebugFormat("Creating MongoServer.");
                MongoServer mongo = MongoServer.Create(mongoConnectionString);

                string dbName = "AnimeRecs";
                Logging.Log.DebugFormat("Creating MongoDatabase {0}.", dbName);
                MongoDatabase recommendorDb = mongo.GetDatabase(dbName);

                string collectionName = "Recommendors";
                Logging.Log.DebugFormat("Getting collection {0}.", collectionName);
                MongoCollection<RecommendorJson> recommendorCollection = recommendorDb.GetCollection<RecommendorJson>(collectionName);

                MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<RecommendorJson>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIdMember(cm.GetMemberMap(c => c.Name));
                });

                AddRecommendorsToMongo(recommendors, recommendorCollection, delayBetweenRequestsMs);

                // Remove any recommendors in mongo that are not in the recomendors file
                RemoveUnusedRecommendors(recommendorCollection, recommendors.Recommendors);
            }
            catch (Exception ex)
            {
                Logging.Log.Fatal("Fatal error!", ex);
                Environment.Exit(1);
            }
        }

        private static RecommendorsInputJson ReadRecommendorListFromFile(string recommendorFilePath)
        {
            Logging.Log.DebugFormat("Getting recommendor list from {0}", recommendorFilePath);

            string recommendorFileText = File.ReadAllText(recommendorFilePath, Encoding.UTF8);
            Logging.Log.DebugFormat("{0} read into memory.", recommendorFilePath);

            RecommendorsInputJson recommendors = JsonConvert.DeserializeObject<RecommendorsInputJson>(recommendorFileText);
            Logging.Log.InfoFormat("Read list of recommendors. Recommendor count = {0}", recommendors.Recommendors.Count);

            return recommendors;
        }

        private static void AddRecommendorsToMongo(RecommendorsInputJson recommendors,
            MongoCollection<RecommendorJson> recommendorCollection, int delayBetweenRequestsMs)
        {
            OfficialMalApi malApi = new OfficialMalApi();

            // For each recommendor, get MAL anime list, calculate recommendations, and add to mongo
            foreach (var recommendorSmartEnum in recommendors.Recommendors.AsSmartEnumerable())
            {
                RecommendorInputJson recommendor = recommendorSmartEnum.Value;

                if (!recommendorSmartEnum.IsFirst)
                {
                    Logging.Log.DebugFormat("Sleeping for {0} ms.", delayBetweenRequestsMs);
                    System.Threading.Thread.Sleep(delayBetweenRequestsMs);
                }

                ICollection<MyAnimeListEntry> animeList = null;
                try
                {
                    animeList = malApi.GetAnimeListForUser(recommendor.MalName);
                }
                catch (MalUserNotFoundException ex)
                {
                    Logging.Log.ErrorFormat("User '{0}' does not have an anime list. Skipping.", recommendor.MalName);
                    continue;
                }
                catch (MalApiException ex)
                {
                    Logging.Log.ErrorFormat("Error getting anime list for {0}. Skipping.", ex, recommendor.MalName);
                    continue;
                }

                RecommendorJson json = new RecommendorJson();
                json.Name = recommendor.MalName;
                json.Recommendations = new List<RecommendedAnimeJson>();

                Logging.Log.InfoFormat("Calculating recommended anime for {0}.", recommendor.MalName);
                GoodOkBadAnime filteredAnime = GetGoodOkBadAnime(recommendor, animeList);
                Logging.Log.InfoFormat("Recommended anime calculated.");

                foreach (MyAnimeListEntry recommendation in filteredAnime.GoodAnime.Select(anime => (MyAnimeListEntry)anime))
                {
                    RecommendedAnimeJson recJson = new RecommendedAnimeJson()
                    {
                        MalId = recommendation.Id,
                        Name = recommendation.Name,
                        Rating = recommendation.Score
                    };
                    json.Recommendations.Add(recJson);
                }

                Logging.Log.InfoFormat("Saving recommendations by {0}...", recommendor.MalName);
                recommendorCollection.Save(json);
                Logging.Log.InfoFormat("Saved.");
            }
        }

        private static GoodOkBadAnime GetGoodOkBadAnime(RecommendorInputJson recommendor, ICollection<MyAnimeListEntry> animeList)
        {
            IGoodOkBadFilter partitioner = null;

            if (recommendor.RecommendedCutoff != null)
            {
                partitioner = new CutoffGoodOkBadFilter()
                {
                    GoodCutoff = recommendor.RecommendedCutoff.Value,
                    OkCutoff = 0
                };
            }
            else if (recommendor.RecommendedPercentile != null)
            {
                partitioner = new PercentileGoodOkBadFilter()
                {
                    RecommendedPercentile = recommendor.RecommendedPercentile.Value,
                    DislikedPercentile = 0
                };
            }
            else
            {
                partitioner = new PercentileGoodOkBadFilter();
            }

            GoodOkBadAnime partitioned = partitioner.GetGoodOkBadAnime(animeList);

            return partitioned;
        }

        private static void RemoveUnusedRecommendors(MongoCollection<RecommendorJson> recommendorCollection,
            IList<RecommendorInputJson> recommendors)
        {
            Logging.Log.InfoFormat("Removing recommendations in the DB that are not in the file.");
            HashSet<string> recommendorsInFile = new HashSet<string>(recommendors.Select(inputJson => inputJson.MalName));
            
            MongoCursor<RecommendorJson> allRecommendorsInDb = recommendorCollection.FindAllAs<RecommendorJson>();
            HashSet<string> recommendorsInDb = new HashSet<string>(allRecommendorsInDb.Select(recommendor => recommendor.Name));

            // Remove recommendors that are in DB but not in file
            HashSet<string> recommendorsInDbButNotFile = new HashSet<string>(recommendorsInDb.Except(recommendorsInFile, StringComparer.Ordinal));
            Logging.Log.InfoFormat("{0} recommendors in DB but not in file.", recommendorsInDbButNotFile.Count);

            foreach(string recommendorToRemove in recommendorsInDbButNotFile)
            {
                Logging.Log.InfoFormat("Removing {0} from the DB.", recommendorToRemove);
                recommendorCollection.Remove(Query.EQ("_id", recommendorToRemove));
                Logging.Log.DebugFormat("Removed.");
            }

            Logging.Log.InfoFormat("Done removing recommendations in DB that are not in the file.");
        }
    }
}
