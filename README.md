﻿AnimeRecs is a project that aims to provide accurate and useful anime recommendations. It contains several components. All components ultimately serve the needs of AnimeRecs.Web, an ASP.NET MVC web application for giving anime recommendations based on the user's myanimelist.net account.

All components are licensed under the GPL version 3. See LICENSE.txt for license information.

AnimeRecs.RecEngine: Core interfaces and classes forming a generic top-N recommendation framework and some basic generic recommendation algorithms. Flexibility is favored over efficiency or simplicity. Algorithms here make as few assumptions about their training data and input as possible. For example, it is possible to make recommendation algorithms that use a different form of input from the usual (user, item, rating) triples, produce more detailed output than a simple list of recommendations, or that avoid making recommendations based on certain criteria (for example, do not recommend an anime that is a sequel if the user has not seen the first anime yet). Also included is code for evaluating the effectiveness of recommendation algorithms. Rec sources defined here are domain-agnostic.

AnimeRecs.RecEngine.MyMediaLite: Integration of the MyMediaLite recommendation framework with the AnimeRecs.RecEngine framework, allowing you to use any class inheriting from MyMediaLite.RatingPrediction.RatingPredictor and implementing MyMediaLite.RatingPrediction.IFoldInRatingPredictor with the AnimeRecs.RecEngine framework.

AnimeRecs.RecEngine.MAL: myanimelist.net-specific recommendation algorithms. These algorithms usually proxy to ones in AnimeRecs.RecEngine and perhaps do some filtering. For example, specials are never recommended because the user may not have seen the anime the special relates to and a user probably does not need a recomendation to tell them to watch the specials of anime they like. If specials are not filtered out, they tend to take up a decent amount of space in a recommendation list. Other filtering includes deciding when to tell algorithms to use the score of an in-progress anime and allowing dropped anime to not count towards an anime's popularity.

AnimeRecs.DAL: SQL scripts for initializing a PostgreSQL database of myanimelist.net users, anime, and ratings, and data access code for it.

AnimeRecs.FreshenMalDatabase: Program that gets N users that are not in the PostgreSQL database and inserts their anime list. The number of users in the database is capped. If the cap is 5000 users, there are 5000 users in the database, and you tell the program to get 100 new users, it will put 100 new users in the database and delete the 100 users that have been in the database the longest. This allows you to maintain a database with a fixed number of users that does not get stale.

AnimeRecs.UpdateStreams: Tool for maintaining the streaming_service_anime_map table.

AnimeRecs.MalEvaluationRunner: Program for evaluating MAL recommendation sources.

AnimeRecs.RecService: Service that listens on a socket and responds to requests for anime recommendations and commands to load/unload recommendation sources.

AnimeRecs.RecService.DTO: Definitions of objects passed between rec service clients and the rec service.

AnimeRecs.RecService.ClientLib: Client library for interacting with AnimeRecs.RecService.

AnimeRecs.RecService.Client: Command-line rec service client application for controlling a running rec service and testing recommendation sources.

AnimeRecs.Web: ASP.NET MVC web application that gives recommendations based on a user's myanimelist.net account

# Setting up

**Note that myanimelist.net now blocks programmatic HTTP requests unless your application is whitelisted so you will not be able to populate a user database or test normally. What you can do is get a database from *somewhere* and enable the config option for getting users' anime list from the database instead of from myanimelist.net.**

To set up a development environment you will need to install the following:

- Microsoft Visual C# 2010 Express
- Visual Web Developer 2010 Express
- PostgreSQL (http://www.postgresql.org/) (9.1.x supported, earlier versions will probably work also)

Set up instructions for Linux assume Ubuntu. If you want to use a different distro, good luck, you're on your own. I have found Ubuntu to have good Mono support.

To build for Mono on Linux, you must use xbuild (or MonoDevelop set to use xbuild) on Linux. Cross-compiling is not supported. Building for .NET vs. Mono has slight differences. Some libraries used have separate .NET and Mono builds, some assemblies need to be copy-local on one but not the other, some need to reference a local assembly when the other references one in the GAC. The only difference in run-time behavior is that when running on Windows recservice is existing by pressing any key, while on Linux it is terminated by sending a SIGINT (ctrl+c) or SIGTERM.

Building on Linux with xbuild:
$ xbuild /t:build /p:Configuration=Debug /p:Platform=AnyCPU xyz.csproj

Building a web package with xbuild (note, this does not apply Web.config transforms!):
$ xbuild /t:build "/p:Configuration=Debug;Platform=AnyCPU;OutDir=/home/greg/publish/" AnimeRecs.Web.csproj

## Setting up PostgreSQL in Ubuntu
1. Set your locale to UTF-8. This ensures that Postgres that does not install with a default of ASCII and prevent you from creating a database that uses UTF-8 as its encoding.
$ sudo update-locale LANG=en_US.UTF-8

2. Increase the maximum shared memory size from the ridiculously small default. This step is optional but will allow you to later increase the amount of shared memory Postgres can use.
edit /etc/sysctl.conf, add line
kernel.shmmax=134217728
(for 128 MB)

$ sudo service procps start



3. Install the postgres package

4. Set a password for the "postgres" PostgreSQL user and allow Linux users other than postgres to log in as the "postgres" PostgreSQL user. Using the postgres user is not necessarily ideal in a production environment, but it's easiest to set up for development.

$ su - postgres
$ psql -d template1 -c "ALTER USER postgres WITH PASSWORD 'testpw';"
$ exit

## Initializing the database

1. Create a PostgreSQL database called "animerecs". On Windows, connect using pgAdmin and use AnimeRecs.DAL/DB Init Scripts/CreateDb_windows.sql. On Linux, psql -U postgres -h localhost -f 'AnimeRecs.DAL/DB Init Scripts/CreateDb_linux.sql'
2. Connect to the new database and run the SQL in AnimeRecs.DAL/DB Init Scripts/InitDb.000.sql to create the tables. On Windows you can use pgAdmin and paste the SQL in. On Linux, psql -U postgres -d animerecs -f InitDb.000.sql.
3. Likewise, run all InitDb.###.sql scripts in order.

## Populating the database with ratings

1. Edit App.config in AnimeRecs.FreshenMalDatabase. Edit the connection string to use your Postgres username and password. Set UsersPerRun to 1000 (or however many users you want to have in the database initially).
2. Compile and run AnimeRecs.FreshenMalDatabase. This will populate the database with users from myanimelist.net's "recently online users" page. This will take some time to run.

## Populating the database with stream mappings

Connect to the animerecs database and run AnimeRecs.DAL/DB Init Scripts/StreamMappings.sql. On Windows you can use pgAdmin and paste the SQL in. On Linux, psql -U postgres -d animerecs -f StreamMappings.sql

## Populating the database with prerequisite mappings

Connect to the animerecs database and run AnimeRecs.DAL/DB Init Scripts/Prereqs.sql. On Windows you can use pgAdmin and paste the SQL in. On Linux, psql -U postgres -d animerecs -f Prereqs.sql

## Running the web site

1. Edit App.config in the AnimeRecs.RecService project. Edit the connection string to use your Postgres username and password. Compile AnimeRecs.RecService and run it in a command prompt.
2. Compile AnimeRecs.RecService.Client and load some rec sources, preferably at least one with the name "default". See the recclient tutorial below for how to use AnimeRecs.RecService.Client.
3. Open AnimeRecs.Web.sln. Compile and run the web site locally. Try it out. You can use http://localhost:[port]/?algorithm=some_rec_source to use a non-default rec source.


[Get command-line usage info]
recclient.exe -h

[Load a rec source that uses average score to recommend anime]
recclient.exe -c LoadRecSource -type AverageScore -name avg

[Load a rec source that requires 40 users to have rated an anime before recommending it instead of the default of 50]
recclient.exe -c LoadRecSource -type AverageScore -name avg --min_users_to_count_anime=40

[Oops! That last command failed because there's already a rec source named "avg". Use the -f switch to overwrite an already-loaded rec source]
recclient.exe -c LoadRecSource -type AverageScore -name avg -f --min_users_to_count_anime=40

[Get anime recommendations for the myanimelist.net user "LordHighCaptain" using the loaded rec source called "avg"]
recclient.exe -c GetMalRecs -name avg -u LordHighCaptain

[Load a rec source that uses MyMediaLite's BiasedMatrixFactorization algorithm. Since we don't specify -name, it uses the name "default".]
recclient.exe -c LoadRecSource -type BiasedMatrixFactorization

[Get anime recommendations using the loaded rec source called "default"]
recclient.exe -c GetMalRecs -u LordHighCaptain

[Let's try reloading it with some non-default options. Use recclient.exe -h to see all possible tunable parameters]
recclient.exe -c LoadRecSource -type BiasedMatrixFactorization -f -bold_driver --bias_learn_rate=0.5
recclient.exe -c GetMalRecs -u LordHighCaptain

[You decided you don't want to use the average score rec source. You can unload a rec source and make its memory available for garbage collection]
recclient.exe -c UnloadRecSource -name avg

[If you have run FreshenMalDatabase to update the database you can retrain all loaded rec sources with current data]
recclient.exe -c ReloadTrainingData
