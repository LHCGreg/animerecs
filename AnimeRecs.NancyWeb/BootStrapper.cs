using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;
using Nancy.Bootstrapper;
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
            if (AppGlobals.Config == null)
            {
                AppGlobals.Config = Config.FromAppConfig();
            }

            if (!AppGlobals.Config.EnableDiagnosticsDashboard)
            {
                DiagnosticsHook.Disable(pipelines);
            }
        }

        protected override DiagnosticsConfiguration DiagnosticsConfiguration
        {
            get
            {
                if (AppGlobals.Config == null)
                {
                    AppGlobals.Config = Config.FromAppConfig();
                }

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
            // This seems to be called before ApplicationStartup, so read config if it hasn't been read yet
            if (AppGlobals.Config == null)
            {
                AppGlobals.Config = Config.FromAppConfig();
            }

            container.Register<IAnimeRecsClientFactory>((c, x) => new RecClientFactory(AppGlobals.Config.RecServicePort, AppGlobals.Config.SpecialRecSourcePorts));
            container.Register<IConfig>(AppGlobals.Config);
        }

        // Called once per request
        protected override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context)
        {
            
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