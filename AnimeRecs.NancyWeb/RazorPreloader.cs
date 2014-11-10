using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;
using Nancy.Diagnostics;
using Nancy.ViewEngines;
using Nancy.ViewEngines.Razor;

namespace AnimeRecs.NancyWeb
{
    /// <summary>
    /// Precompiles Razor views so they the first use of one takes less time.
    /// Savings of about 200-500 ms are possible.
    /// </summary>
    class RazorPreloader
    {
        private RazorViewEngine _engine;
        private IViewLocator _viewLocator;
        private IRenderContextFactory _renderContextFactory;
        
        public RazorPreloader(RazorViewEngine engine, IViewLocator viewLocator, IRenderContextFactory renderContextFactory)
        {
            _engine = engine;
            _viewLocator = viewLocator;
            _renderContextFactory = renderContextFactory;
        }
        
        public void PreloadRazorView(string viewPath, object viewModel)
        {
            // It seems the only way to precompile a view in Nancy is to actually use it?
            using (NancyContext context = GetDummyContext())
            {
                ViewLocationResult viewLocation = _viewLocator.LocateView(viewPath, context);
                ViewLocationContext locationContext = new ViewLocationContext() { Context = context };
                IRenderContext renderContext = _renderContextFactory.GetRenderContext(locationContext);
                using (Response renderedView = _engine.RenderView(viewLocation, viewModel, renderContext, isPartial: true))
                using (MemoryStream stream = new MemoryStream())
                {
                    renderedView.Contents(stream);
                    stream.Position = 0;
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        // Disable Mono warning about renderedHtml not being used. It's useful to see it when debugging.
#pragma warning disable 219
                        string renderedHtml = reader.ReadToEnd();
#pragma warning restore 219
                    }
                }
            }
        }

        private NancyContext GetDummyContext()
        {
            return new NancyContext()
            {
                Culture = CultureInfo.InvariantCulture,
                CurrentUser = null,
                Parameters = new DynamicDictionary(),
                ResolvedRoute = null,
                Trace = new DefaultRequestTrace() { Items = new Dictionary<string, object>(), TraceLog = new DefaultTraceLog() },
            };
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