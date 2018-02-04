AnimeRecs is a project that aims to provide accurate and useful anime recommendations. It contains several components. All components ultimately serve the needs of AnimeRecs.Web, an ASP.NET MVC web application for giving anime recommendations based on the user's myanimelist.net account.

All components are licensed under the GPL version 3. See LICENSE.txt for license information.

AnimeRecs.RecEngine: Core interfaces and classes forming a generic top-N recommendation framework and some basic generic recommendation algorithms. Flexibility is favored over efficiency or simplicity. Algorithms here make as few assumptions about their training data and input as possible. For example, it is possible to make recommendation algorithms that use a different form of input from the usual (user, item, rating) triples, produce more detailed output than a simple list of recommendations, or that avoid making recommendations based on certain criteria (for example, do not recommend an anime that is a sequel if the user has not seen the first anime yet). Also included is code for evaluating the effectiveness of recommendation algorithms. Rec sources defined here are domain-agnostic.

AnimeRecs.RecEngine.MyMediaLite: Integration of the MyMediaLite recommendation framework with the AnimeRecs.RecEngine framework, allowing you to use any class inheriting from MyMediaLite.RatingPrediction.RatingPredictor and implementing MyMediaLite.RatingPrediction.IFoldInRatingPredictor with the AnimeRecs.RecEngine framework. This project currently only builds when targeting the full .NET framework, not netstandard, due to MyMediaLite and its dependencies, C5 and MathNet.Numerics, not having netstandard packages yet. Any functionality in other projects that depends on this project will only be built when targetting the full framework.

AnimeRecs.RecEngine.MAL: myanimelist.net-specific recommendation algorithms. These algorithms usually proxy to ones in AnimeRecs.RecEngine and perhaps do some filtering. For example, specials are never recommended because the user may not have seen the anime the special relates to and a user probably does not need a recomendation to tell them to watch the specials of anime they like. If specials are not filtered out, they tend to take up a decent amount of space in a recommendation list. Other filtering includes deciding when to tell algorithms to use the score of an in-progress anime and allowing dropped anime to not count towards an anime's popularity.

AnimeRecs.DAL: SQL scripts for initializing a PostgreSQL database of myanimelist.net users, anime, and ratings, and data access code for it.

AnimeRecs.FreshenMalDatabase: Program that gets N users that are not in the PostgreSQL database and inserts their anime list. The number of users in the database is capped. If the cap is 5000 users, there are 5000 users in the database, and you tell the program to get 100 new users, it will put 100 new users in the database and delete the 100 users that have been in the database the longest. This allows you to maintain a database with a fixed number of users that does not get stale.

AnimeRecs.UpdateStreams: Tool for maintaining the streaming_service_anime_map table.

AnimeRecs.MalEvaluationRunner: Program for evaluating MAL recommendation sources.

AnimeRecs.RecService: Service that listens on a socket and responds to requests for anime recommendations and commands to load/unload recommendation sources.

AnimeRecs.RecService.DTO: Definitions of objects passed between rec service clients and the rec service.

AnimeRecs.RecService.ClientLib: Client library for interacting with AnimeRecs.RecService.

AnimeRecs.RecService.Client: Command-line rec service client application for controlling a running rec service and testing recommendation sources.

AnimeRecs.Web: ASP.NET web application that gives recommendations based on a user's myanimelist.net account

# Setting up

To set up a development environment you will need to install the following:

- Visual Studio 2017 Community Edition
- PostgreSQL (http://www.postgresql.org/) (9.6.x supported, other versions will probably work)

## Setting up PostgreSQL in Debian
1. Set your locale to UTF-8. This ensures that Postgres that does not install with a default of ASCII and prevent you from creating a database that uses UTF-8 as its encoding.
$ echo "en_US.UTF-8 UTF-8" | sudo tee /etc/locale.gen > /dev/null
$ sudo locale-gen
$ sudo update-locale LANG=en_US.UTF-8

2. Increase the maximum shared memory size from the ridiculously small default. This step is optional but will allow you to later increase the amount of shared memory Postgres can use.
edit /etc/sysctl.conf, add line
kernel.shmmax=134217728
(for 128 MB)

$ sudo service procps start



3. Install the postgres package

4. Set a password for the "postgres" PostgreSQL user and allow Linux users other than postgres to log in as the "postgres" PostgreSQL user. Using the postgres user is not ideal in a production environment, but it's easiest to set up for development.

$ su - postgres
$ psql -d template1 -c "ALTER USER postgres WITH PASSWORD 'testpw';"
$ exit

## Initializing the database

1. Install [pgdbsc](https://github.com/LHCGreg/dbsc/releases). On Windows, download the installer for the latest version from the releases page. On Linux, follow the instructions on [the readme](https://github.com/LHCGreg/dbsc/blob/master/README.md) to add the dbsc package repository and install the package.
2. Open a command prompt to AnimeRecs.DAL\DB Init Scripts. On Windows, run `pgdbsc checkout -u postgres -p testpw -dbCreateTemplate windows_create_template.sql`. On Linux, run `pgdbsc checkout -u postgres -p testpw -dbCreateTemplate linux_create_template.sql`

## Populating the database with ratings

1. Copy config.example.xml in AnimeRecs.FreshenMalDatabase to config.xml. Edit the connection string to use your Postgres username and password. Set UsersPerRun and MaxUsersInDatabase appropriately.
2. Compile and run AnimeRecs.FreshenMalDatabase. This will populate the database with users from myanimelist.net's "recently online users" page. This will take some time to run.

## Populating the database with stream mappings

Connect to the animerecs database and run AnimeRecs.DAL/DB Init Scripts/StreamMappings.sql. psql -h localhost -U postgres -d animerecs -f StreamMappings.sql

## Running the web site

1. Copy NLog.example.config in AnimeRecs.AnimeRecs.RecService to NLog.config. Copy config.example.xml to config.xml. Edit the connection string to use your Postgres username and password. Compile AnimeRecs.RecService and run it in a command prompt.
2. Compile and run the web site locally. Try it out. You can use http://localhost:8888/?algorithm=some_rec_source to use a non-default rec source.

## recclient tutorial
[Get command-line usage info]
dotnet recclient.dll -h

[Load a rec source that uses average score to recommend anime]
dotnet recclient.dll -c LoadRecSource -type AverageScore -name avg

[Load a rec source that requires 40 users to have rated an anime before recommending it instead of the default of 50]
dotnet recclient.dll -c LoadRecSource -type AverageScore -name avg --min_users_to_count_anime=40

[Oops! That last command failed because there's already a rec source named "avg". Use the -f switch to overwrite an already-loaded rec source]
dotnet recclient.dll -c LoadRecSource -type AverageScore -name avg -f --min_users_to_count_anime=40

[Get anime recommendations for the myanimelist.net user "LordHighCaptain" using the loaded rec source called "avg"]
dotnet recclient.dll -c GetMalRecs -name avg -u LordHighCaptain

[Load a rec source that uses MyMediaLite's BiasedMatrixFactorization algorithm. Since we don't specify -name, it uses the name "default".]
dotnet recclient.dll -c LoadRecSource -type BiasedMatrixFactorization

[Get anime recommendations using the loaded rec source called "default"]
dotnet recclient.dll -c GetMalRecs -u LordHighCaptain

[Let's try reloading it with some non-default options. Use dotnet recclient.dll -h to see all possible tunable parameters]
dotnet recclient.dll -c LoadRecSource -type BiasedMatrixFactorization -f -bold_driver --bias_learn_rate=0.5
dotnet recclient.dll -c GetMalRecs -u LordHighCaptain

[You decided you don't want to use the average score rec source. You can unload a rec source and make its memory available for garbage collection]
dotnet recclient.dll -c UnloadRecSource -name avg

[If you have run FreshenMalDatabase to update the database you can retrain all loaded rec sources with current data]
dotnet recclient.dll -c ReloadTrainingData
