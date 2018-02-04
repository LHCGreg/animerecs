using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.RecEngine
{
    public class AnimeRecsTrainingData<TUnderlyingTrainingData, TUserRatings>
        where TUnderlyingTrainingData : IBasicTrainingData<TUserRatings>
        where TUserRatings : IInputForUserWithItemIds
    {
        public TUnderlyingTrainingData TrainingData { get; private set; }
        public IUserInputClassifier<TUserRatings> RecommenderRatingClassifier { get; private set; }

        public AnimeRecsTrainingData(TUnderlyingTrainingData trainingData, IUserInputClassifier<TUserRatings> recommenderRatingClassifier)
        {
            TrainingData = trainingData;
            RecommenderRatingClassifier = recommenderRatingClassifier;
        }
    }
}
