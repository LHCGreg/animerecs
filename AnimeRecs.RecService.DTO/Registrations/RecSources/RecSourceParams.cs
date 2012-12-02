using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.RecService.DTO
{
    public class RecSourceParams
    {
        public virtual string GetRecSourceTypeName()
        {
            return "Unknown";
        }
    }

    public class AverageScoreRecSourceParams : RecSourceParams
    {
        public int MinEpisodesToCountIncomplete { get; set; }
        public int MinUsersToCountAnime { get; set; }
        public bool UseDropped { get; set; }

        public AverageScoreRecSourceParams()
        {
            MinEpisodesToCountIncomplete = 26;
            MinUsersToCountAnime = 50;
            UseDropped = true;
        }

        public AverageScoreRecSourceParams(int minEpisodesToCountIncomplete, int minUsersToCountAnime, bool useDropped)
        {
            MinEpisodesToCountIncomplete = minEpisodesToCountIncomplete;
            MinUsersToCountAnime = minUsersToCountAnime;
            UseDropped = useDropped;
        }

        public override string GetRecSourceTypeName()
        {
            return RecSourceTypes.AverageScore;
        }
    }

    public class MostPopularRecSourceParams : RecSourceParams
    {
        public int MinEpisodesToCountIncomplete { get; set; }
        public bool UseDropped { get; set; }

        public MostPopularRecSourceParams()
        {
            MinEpisodesToCountIncomplete = 26;
            UseDropped = false;
        }

        public MostPopularRecSourceParams(int minEpisodesToCountIncomplete, bool useDropped)
        {
            MinEpisodesToCountIncomplete = minEpisodesToCountIncomplete;
            UseDropped = useDropped;
        }

        public override string GetRecSourceTypeName()
        {
            return RecSourceTypes.MostPopular;
        }
    }

    public class AnimeRecsRecSourceParams : RecSourceParams
    {
        public int NumRecommendersToUse { get; set; }
        public double FractionConsideredRecommended { get; set; }
        public int MinEpisodesToClassifyIncomplete { get; set; }

        public AnimeRecsRecSourceParams()
        {
            NumRecommendersToUse = 100;
            FractionConsideredRecommended = 0.35;
            MinEpisodesToClassifyIncomplete = 26;
        }

        public AnimeRecsRecSourceParams(int numRecommendersToUse, double fractionConsideredRecommended, int minEpisodesToClassifyIncomplete)
        {
            NumRecommendersToUse = numRecommendersToUse;
            FractionConsideredRecommended = fractionConsideredRecommended;
            MinEpisodesToClassifyIncomplete = minEpisodesToClassifyIncomplete;
        }

        public override string GetRecSourceTypeName()
        {
            return RecSourceTypes.AnimeRecs;
        }
    }

    public class BiasedMatrixFactorizationRecSourceParams : RecSourceParams
    {
        public int MinEpisodesToCountIncomplete { get; set; }
        public bool UseDropped { get; set; }
        public int MinUsersToCountAnime { get; set; }

        public float? BiasLearnRate { get; set; }
        public float? BiasReg { get; set; }
        public bool? BoldDriver { get; set; }
        public bool? FrequencyRegularization { get; set; }
        public float? LearnRate { get; set; }
        public float? LearnRateDecay { get; set; }
        public string OptimizationTarget { get; set; }
        public uint? NumFactors { get; set; }
        public uint? NumIter { get; set; }
        public float? RegI { get; set; }
        public float? RegU { get; set; }

        public BiasedMatrixFactorizationRecSourceParams()
        {
            MinEpisodesToCountIncomplete = 26;
            UseDropped = true;
            MinUsersToCountAnime = 50;
        }

        public BiasedMatrixFactorizationRecSourceParams(int minEpisodesToCountIncomplete, bool useDropped, int minUsersToCountAnime)
        {
            MinEpisodesToCountIncomplete = minEpisodesToCountIncomplete;
            UseDropped = useDropped;
            MinUsersToCountAnime = minUsersToCountAnime;
        }

        public override string GetRecSourceTypeName()
        {
            return RecSourceTypes.BiasedMatrixFactorization;
        }
    }

    public class SVDPlusPlusRecSourceParams : RecSourceParams
    {
        public int MinEpisodesToCountIncomplete { get; set; }
        public bool UseDropped { get; set; }
        public int MinUsersToCountAnime { get; set; }

        public float? BiasLearnRate { get; set; }
        public float? BiasReg { get; set; }
        public bool? FrequencyRegularization { get; set; }
        public float? LearnRate { get; set; }
        public float? LearnRateDecay { get; set; }
        public uint? NumFactors { get; set; }
        public uint? NumIter { get; set; }
        public float? Regularization { get; set; }

        public SVDPlusPlusRecSourceParams()
        {
            MinEpisodesToCountIncomplete = 26;
            UseDropped = true;
            MinUsersToCountAnime = 50;
        }
    }

    public class ItemKNNRecSourceParams : RecSourceParams
    {
        public int MinEpisodesToCountIncomplete { get; set; }
        public bool UseDropped { get; set; }
        public int MinUsersToCountAnime { get; set; }

        public float? Alpha { get; set; }
        public string Correlation { get; set; }
        public uint? K { get; set; }
        public uint? NumIter { get; set; }
        public float? RegI { get; set; }
        public float? RegU { get; set; }
        public bool? WeightedBinary { get; set; }

        public ItemKNNRecSourceParams()
        {
            MinEpisodesToCountIncomplete = 26;
            UseDropped = true;
            MinUsersToCountAnime = 50;
        }
    }

    public class BPRMFRecSourceParams : RecSourceParams
    {
        public double FractionConsideredRecommended { get; set; }
        public int MinEpisodesToClassifyIncomplete { get; set; }
        public int MinUsersToCountAnime { get; set; }

        public uint? NumFactors { get; set; }
        public float? BiasReg { get; set; }
        public float? RegU { get; set; }
        public float? RegI { get; set; }
        public float? RegJ { get; set; }
        public uint? NumIter { get; set; }
        public float? LearnRate { get; set; }
        public bool? UniformUserSampling { get; set; }
        public bool? WithReplacement { get; set; }
        public bool? UpdateJ { get; set; }

        public BPRMFRecSourceParams()
        {
            FractionConsideredRecommended = 0.35;
            MinEpisodesToClassifyIncomplete = 26;
            MinUsersToCountAnime = 50;
        }

        public BPRMFRecSourceParams(double fractionConsideredRecommended, int minEpisodesToClassifyIncomplete, int minUsersToCountAnime)
        {
            FractionConsideredRecommended = fractionConsideredRecommended;
            MinEpisodesToClassifyIncomplete = minEpisodesToClassifyIncomplete;
            MinUsersToCountAnime = minUsersToCountAnime;
        }

        public override string GetRecSourceTypeName()
        {
            return RecSourceTypes.BPRMF;
        }
    }
}

// Copyright (C) 2012 Greg Najda
//
// This file is part of AnimeRecs.RecService.DTO.
//
// AnimeRecs.RecService.DTO is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// AnimeRecs.RecService.DTO is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with AnimeRecs.RecService.DTO.  If not, see <http://www.gnu.org/licenses/>.