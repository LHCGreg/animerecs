using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AnimeRecs.DAL;
using AnimeRecs.NancyWeb.Modules.GetRecs;
using AnimeRecs.NancyWeb.Modules.Home;
using AnimeRecs.RecEngine;
using MalApi;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Conventions;
using Nancy.Diagnostics;
using Nancy.Responses.Negotiation;
using Nancy.TinyIoc;
using Nancy.ViewEngines;
using Nancy.ViewEngines.Razor;

namespace AnimeRecs.NancyWeb
{
    public class BootStrapper : DefaultNancyBootstrapper
    {
        // Called once
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            // DiagnosticsConfiguration seems to be read before this method is called.
            // So set the config in whichever gets called first, because Nancy could change that.
            LoadConfigIfNotLoaded();

            KickOffViewPrecompiling(container);

            if (!AppGlobals.Config.EnableDiagnosticsDashboard)
            {
                DiagnosticsHook.Disable(pipelines);
            }

            StaticConfiguration.DisableErrorTraces = !AppGlobals.Config.ShowErrorTraces;

            IResponseNegotiator responder = container.Resolve<IResponseNegotiator>();
            pipelines.OnError += (ctx, ex) => ErrorHandler.HandleException(ctx, ex, responder);
        }

        protected override DiagnosticsConfiguration DiagnosticsConfiguration
        {
            get
            {
                LoadConfigIfNotLoaded();

                if (AppGlobals.Config.EnableDiagnosticsDashboard)
                {
                    return new DiagnosticsConfiguration { Password = AppGlobals.Config.DiagnosticsDashboardPassword };
                }
                else
                {
                    return base.DiagnosticsConfiguration;
                }
            }
        }

        // Called once
        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            List<IDisposable> disposablesInitialized = new List<IDisposable>();
            try
            {
                // This seems to be called before ApplicationStartup, so read config if it hasn't been read yet
                LoadConfigIfNotLoaded();

                container.Register<IConfig>(AppGlobals.Config);

                IAnimeRecsClientFactory recServiceClientFactory = new RecClientFactory(AppGlobals.Config.RecServicePort, AppGlobals.Config.SpecialRecSourcePorts);
                container.Register<IAnimeRecsClientFactory>(recServiceClientFactory);

                IAnimeRecsDbConnectionFactory dbConnectionFactory = new AnimeRecsDbConnectionFactory(AppGlobals.Config.PostgresConnectionString);
                container.Register<IAnimeRecsDbConnectionFactory>(dbConnectionFactory);

                IMyAnimeListApi api;
                if (AppGlobals.Config.UseLocalDbMalApi)
                {
                    api = new PgMyAnimeListApi(AppGlobals.Config.PostgresConnectionString);
                }
                else
                {
                    api = new MyAnimeListApi()
                    {
                        UserAgent = AppGlobals.Config.MalApiUserAgentString,
                        TimeoutInMs = AppGlobals.Config.MalTimeoutInMs
                    };
                }
                disposablesInitialized.Add(api);

                CachingMyAnimeListApi cachingApi = new CachingMyAnimeListApi(api, AppGlobals.Config.AnimeListCacheExpiration, ownApi: true);
                disposablesInitialized.Add(cachingApi);

                SingletonMyAnimeListApiFactory factory = new SingletonMyAnimeListApiFactory(cachingApi);

                // TinyIoC will dispose of the factory when the Nancy host stops
                container.Register<IMyAnimeListApiFactory>(factory);
            }
            catch
            {
                foreach (IDisposable disposable in disposablesInitialized)
                {
                    disposable.Dispose();
                }
                throw;
            }
        }

        // Called once per request
        protected override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context)
        {

        }

        protected override void ConfigureConventions(NancyConventions nancyConventions)
        {
            base.ConfigureConventions(nancyConventions);

            LoadConfigIfNotLoaded();
            if (AppGlobals.Config.HandleStaticContent)
            {
                // Content is already allowed by Nancy
                nancyConventions.StaticContentsConventions.AddDirectory("Scripts");
            }
        }

        private void LoadConfigIfNotLoaded()
        {
            if (AppGlobals.Config == null)
            {
                Logging.Log.Debug("Loading config");
                AppGlobals.Config = Config.FromAppConfig();
                Logging.Log.Debug("Config loaded.");
            }
        }

        private static Dictionary<string, object> ViewPathsAndModels = new Dictionary<string, object>()
        {
            { "Modules/Home/Home", new HomeViewModel("default", true, true, true, false) },
            { "Modules/GetRecs/AnimeRecs", GetDummyAnimeRecsViewModel() },
            { "Modules/GetRecs/AnimeRecs_complex", GetDummyAnimeRecsViewModel() },
            { "Modules/GetRecs/AverageScore", GetDummyRecsViewModel() },
            { "Modules/GetRecs/Fallback", GetDummyRecsViewModel() },
            { "Modules/GetRecs/MostPopular", GetDummyRecsViewModel() },
            { "Modules/GetRecs/RatingPrediction", GetDummyRecsViewModel() },
            { "Error", new ErrorViewModel(new Exception("blah")) }
        };

        private static List<string> ViewsFinishedPrecompiling = new List<string>();

        private void KickOffViewPrecompiling(TinyIoCContainer container)
        {
            Logging.Log.Info("Precompiling razor views in background");
            RazorViewEngine engine = container.Resolve<RazorViewEngine>();
            IViewLocator locator = container.Resolve<Nancy.ViewEngines.IViewLocator>();
            IRenderContextFactory renderContextFactory = container.Resolve<IRenderContextFactory>();
            RazorPreloader preloader = new RazorPreloader(engine, locator, renderContextFactory);

            foreach (var pathAndModel in ViewPathsAndModels)
            {
                // Avoid capturing the loop variable, it does not work how you expect it to work in some .NET versions.
                // No idea what mono does with it.
                string viewPath = pathAndModel.Key;
                object viewModel = pathAndModel.Value;
                ThreadPool.QueueUserWorkItem(x => PrecompileView(viewPath, viewModel, engine, locator, renderContextFactory, preloader));
            }
        }

        private static void PrecompileView(string viewPath, object viewModel, RazorViewEngine engine, IViewLocator locator,
            IRenderContextFactory renderContextFactory, RazorPreloader preloader)
        {
            try
            {
                Logging.Log.DebugFormat("Precompiling view {0}", viewPath);
                preloader.PreloadRazorView(viewPath, viewModel);
                Logging.Log.DebugFormat("Finished precompiling view {0}", viewPath);
            }
            catch (Exception ex)
            {
                Logging.Log.ErrorFormat("Error precompiling view {0}: {1}", ex, viewPath, ex.Message);
            }

            lock (ViewsFinishedPrecompiling)
            {
                ViewsFinishedPrecompiling.Add(viewPath);
                if (ViewsFinishedPrecompiling.Count == ViewPathsAndModels.Count)
                {
                    Logging.Log.Info("Finished precompiling views");
                }
            }
        }

        private static GetRecsViewModel GetDummyRecsViewModel()
        {
            return new GetRecsViewModel(
                results: new RecService.ClientLib.MalRecResults<IEnumerable<RecEngine.IRecommendation>>(
                    results: new List<IRecommendation>(),
                    animeInfo: new Dictionary<int, RecEngine.MAL.MalAnime>(),
                    recommendationType: "blah"
                ),
                userId: 9,
                userName: "blah",
                userLookup: new MalUserLookupResults(9, "blah", new List<MyAnimeListEntry>()),
                userAnimeList: new Dictionary<int, RecEngine.MAL.MalListEntry>(),
                maximumRecommendationsToReturn: 100,
                maximumRecommendersToReturn: 5,
                animeWithheld: new Dictionary<int, RecEngine.MAL.MalListEntry>(),
                dbConnectionFactory: null
            );
        }

        private static GetRecsViewModel GetDummyAnimeRecsViewModel()
        {
            return new GetRecsViewModel(
                results: new RecService.ClientLib.MalRecResults<IEnumerable<RecEngine.IRecommendation>>(
                    results: new AnimeRecs.RecEngine.MAL.MalAnimeRecsResults(
                        recommendations: new List<AnimeRecsRecommendation>(),
                        recommenders: new List<RecEngine.MAL.MalAnimeRecsRecommenderUser>(),
                        targetScoreUsed: 8m
                    ),
                    animeInfo: new Dictionary<int, RecEngine.MAL.MalAnime>(),
                    recommendationType: "blah"
                ),
                userId: 9,
                userName: "blah",
                userLookup: new MalUserLookupResults(9, "blah", new List<MyAnimeListEntry>()),
                userAnimeList: new Dictionary<int, RecEngine.MAL.MalListEntry>(),
                maximumRecommendationsToReturn: 100,
                maximumRecommendersToReturn: 5,
                animeWithheld: new Dictionary<int, RecEngine.MAL.MalListEntry>(),
                dbConnectionFactory: null
            );
        }
    }
}

// Copyright (C) 2014 Greg Najda
//
// This file is part of AnimeRecs.NancyWeb.
//
// AnimeRecs.NancyWeb is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// AnimeRecs.NancyWeb is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with AnimeRecs.NancyWeb.  If not, see <http://www.gnu.org/licenses/>.