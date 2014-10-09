using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnimeRecs.DAL;
using MalApi;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Conventions;
using Nancy.Diagnostics;
using Nancy.TinyIoc;

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

            if (!AppGlobals.Config.EnableDiagnosticsDashboard)
            {
                DiagnosticsHook.Disable(pipelines);
            }

            StaticConfiguration.DisableErrorTraces = !AppGlobals.Config.ShowErrorTraces;
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
                AppGlobals.Config = Config.FromAppConfig();
            }
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