using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnimeRecs.RecService.ClientLib;
using FluentValidation;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Validation;

namespace AnimeRecs.Web.Modules.Home
{
    public class HomeModule : NancyModule
    {
        private IConfig _config;
        private IAnimeRecsClientFactory _recClientFactory;
        
        public HomeModule(IConfig config, IAnimeRecsClientFactory recClientFactory)
        {
            _config = config;
            _recClientFactory = recClientFactory;

            Get["/"] = HomePage;
        }

        public class MainPageParams
        {
            public string algorithm { get; set; }
            public bool? detailedResults { get; set; }
            public bool? debugMode { get; set; }
        }

        public class MainPageParamsValidator : AbstractValidator<MainPageParams>
        {
            public MainPageParamsValidator()
            {
                //RuleFor(p => p.algorithm).NotNull();
            }
        }

        private object HomePage(dynamic arg)
        {
            // TODO: miniprofiler
            MainPageParams parameters = this.BindAndValidate<MainPageParams>();
            //if (!ModelValidationResult.IsValid)
            //{
            //    StringBuilder errors = new StringBuilder();
            //    foreach (var x in ModelValidationResult.Errors.SelectMany(p => p.Value.Select(e => new { Property = p.Key, Properties = e.MemberNames.ToList(), Error = e.ErrorMessage })))
            //    {
            //        errors.AppendLine(string.Format("Property = {0}, Properties = {1}, Error = {2}", x.Property, string.Join(", ", x.Properties), x.Error));
            //    }
            //    // TODO: Do something with the error...or not, since there can't be an error
            //    return errors.ToString();
            //}

            return DoHomePage(parameters);
        }

        private object DoHomePage(MainPageParams parameters)
        {
            string algorithm = parameters.algorithm ?? _config.DefaultRecSource;
            bool displayDetailedResults = parameters.detailedResults ?? false;
            bool debugModeOn = parameters.debugMode ?? false;

            string recSourceType = null;

            using (AnimeRecsClient client = _recClientFactory.GetClient(algorithm))
            {
                try
                {
                    recSourceType = client.GetRecSourceType(algorithm);
                }
                catch
                {
                    ;
                }
            }

            bool algorithmAvailable = recSourceType != null;

            bool targetScoreNeeded = false;
            if (AnimeRecs.RecService.DTO.RecSourceTypes.AnimeRecs.Equals(recSourceType, StringComparison.OrdinalIgnoreCase) && displayDetailedResults)
            {
                targetScoreNeeded = true;
            }

            HomeViewModel viewModel = new HomeViewModel(
                algorithm: algorithm,
                algorithmAvailable: algorithmAvailable,
                targetScoreNeeded: targetScoreNeeded,
                displayDetailedResults: displayDetailedResults,
                debugModeOn: debugModeOn
            );

            return View["Modules/Home/Home", viewModel];
        }
    }
}

// Copyright (C) 2014 Greg Najda
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