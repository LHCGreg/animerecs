using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;

namespace AnimeRecs.NancyWeb
{
    public class BootStrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            // Called once
            AppGlobals.Config = Config.FromAppConfig();
        }

        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            // Called once
            container.Register<IAnimeRecsClientFactory>((c, x) => new RecClientFactory(AppGlobals.Config.RecServicePort, AppGlobals.Config.SpecialRecSourcePorts));
        }

        protected override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context)
        {
            // Called once per request
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