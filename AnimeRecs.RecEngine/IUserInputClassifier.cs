using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.RecEngine
{
    /// <summary>
    /// Classifies input for a user into liked, unliked and other. Normal input should be either liked or unliked.
    /// "Other" is for anything else in the input needed to form the complete original input. For example, if the input includes
    /// items the user plans to watch but has not watched yet, that would go into "Other".
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    public interface IUserInputClassifier<TInput>
    {
        ClassifiedUserInput<TInput> Classify(TInput inputForUser);
    }
}

// Copyright (C) 2012 Greg Najda
//
// This file is part of AnimeRecs.RecEngine.
//
// AnimeRecs.RecEngine is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// AnimeRecs.RecEngine is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with AnimeRecs.RecEngine.  If not, see <http://www.gnu.org/licenses/>.