AnimeRecs is a project that aims to provide accurate and useful anime recommendations. It contains several components, some of which may be useful outside this project.

AnimeRecs.MalApi is licensed under the Apache 2.0 license. All other components are licensed under the GPL version 3. See LICENSE.txt for license information.

AnimeRecs.MalApi: Provides a .NET binding to the API of myanimelist.net and methods for scraping information where no official API is available. This library is primarily concerned with the needs of the AnimeRecs project but patches adding more functionality are welcome.

AnimeRecs.RecEngine: Core interfaces and classes forming a generic top-N recommendation framework and some basic generic recommendation algorithms. Flexibility is favored over efficiency or simplicity. Algorithms here make as few assumptions about their training data and input as possible. For example, it is possible to make recommendation algorithms that use a different form of input from the usual (user, item, rating) triples, produce more detailed output than a simple list of recommendations, or that avoid making recommendations based on certain criteria (for example, do not recommend an anime that is a sequel if the user has not seen the first anime yet). Also included is code for evaluating the effectiveness of recommendation algorithms.

AnimeRecs.RecEngine.MyMediaLite: Integration of the MyMediaLite recommendation framework with the AnimeRecs.RecEngine framework, allowing you to use any class inheriting from MyMediaLite.RatingPrediction.RatingPredictor and implementing MyMediaLite.RatingPrediction.IFoldInRatingPredictor with the AnimeRecs.RecEngine framework.

AnimeRecs.RecEngine.MAL: myanimelist.net-specific recommendation algorithms. These algorithms usually proxy to ones in AnimeRecs.RecEngine and perhaps do some filtering. For example, specials are never recommended because the user may not have seen the anime the special relates to and a user probably needs to recommendation to tell them to watch the specials of anime they like. If specials are not filtered out, they tend to take up a decent amount of space in a recommendation list. Other filtering includes deciding when to tell algorithms to use the score of an in-progress anime and allowing dropped anime to not count towards an anime's popularity.

AnimeRecs.DAL: SQL scripts for initializing a PostgreSQL database of myanimelist.net users, anime, and ratings, and data access code for it.

AnimeRecs.FreshenMalDatabase: Program that gets N users that are not in the PostgreSQL database and inserts their anime list. The number of users in the database can capped. If the cap is 5000 users, there are 5000 users in the database, and you tell the program to get 100 new users, it will put 100 new users in the database and delete the 100 users that have been in the database the longest. This allows you to maintain a database with a fixed number of users that does not get stale.

AnimeRecs.GetMalRecs: Program for training a recommendation source with the PostgreSQL database, getting a given MAL user's anime list, and getting recommendations for that user. Useful for testing out a recommendation source and seeing what recommendations it returns.

AnimeRecs.MalEvaluationRunner: Program for evaluating MAL recommendation sources.