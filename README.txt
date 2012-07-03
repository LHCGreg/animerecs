A new engine in development under /new. The goal is to have pluggable recommendation sources to make it as easy as possible for third parties to write and test recommendation algorithms. The current engine is extremely coupled to the animerecs algorithm. See /new/readme.txt for an overview of the new engine.

To set up an environment for developing or using the new engine, you will need to install the following:

- Microsoft Visual C# 2010 Express
- PostgreSQL (http://www.postgresql.org/) (9.1.x supported, earlier versions will probably work also)

1. Create a PostgreSQL database called "animerecs" using the SQL in /new/AnimeRecs.DAL/CreateDb.sql
2. Connect to the new database and run the SQL in /new/AnimeRecs.DAL/InitDb.sql to create the tables.
3. Open /new/AnimeRecs.RecEngine.sln in Visual C#. Edit App.config in AnimeRecs.FreshenMalDatabase. Edit the connection string to use your Postgres username and password. Set UsersPerRun to 1000 (or however many users you want to have in the database initially).
4. Compile and run AnimeRecs.FreshenMalDatabase. This will populate the database with users from myanimelist.net's "recently online users" page.

You're all set up now! To start the recommendation service, compile and run AnimeRecs.RecService (edit the App.config with your Postgres username and password).

Now compile AnimeRecs.RecService.Client, open a command prompt in the directory the compiled binary is in, and try these commands. Note, if you are on Linux using Mono, use "mono recclient.exe" instead of just "recclient.exe".

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

[Load a rec source that uses MyMediaLite's BiasedMatrixFactorization algorithm. Since we don't specify -name, it uses the name "default".
recclient.exe -c LoadRecSource -type BiasedMatrixFactorization

[Get anime recommendations using the loaded rec source called "default"]
recclient.exe -c GetMalRecs -u LordHighCaptain

[Let's try reloading it with some non-default option. Use recclient.exe -h to see all possible tunable parameters]
recclient.exe -c LoadRecSource -type BiasedMatrixFactorization -f -bold_driver --bias_learn_rate=0.5
recclient.exe -c GetMalRecs -u LordHighCaptain

[You decided you don't want to use the average score rec source. You can unload a rec source and make its memory available for garbage collection]
recclient.exe -c UnloadRecSource -name avg

[If you have run FreshenMalDatabase to update the database you can retrain all loaded rec sources with current data]
recclient.exe -c ReloadTrainingData








Instructions for the old animerecs
----------------------------------
 

To build and run the site for development, you will need to install the following:

- Microsoft Visual Web Developer 2010 Express
- Microsoft Visual C# 2010 Express
- MongoDB (http://www.mongodb.org/downloads)


Once those are installed, you'll need to configure MongoDB.

Create a mongo.conf file somewhere that looks like this:

logpath = C:\Users\Greg\AppData\Roaming\AnimeRecs\mongo.log
logappend = true
dbpath = C:\Users\Greg\AppData\Roaming\AnimeRecs\MongoData
noauth = true
nohttpinterface = true

Change logpath and dbpath to where you want the mongo log file to be and the directory that you want mongo to keep its data in, respectively.

Start the mongo server by running

mongod -f C:\Users\Greg\AppData\Roaming\AnimeRecs\mongo.conf

from mongo's /bin folder (changing the mongo.conf path of course). You can stop the server with ctrl+c.


Now open up the UpdateRecommendorCache solution in Visual C#. Change the log file path in App.config from C:\Users\Greg\AppData\Roaming\AnimeRecs\UpdateRecommendorCache.log to something more suitable for you. Compile it and run it. This will build the recommendor database. The Mongo server must be running.



Now you are able to open the AnimeRecs solution in Visual Web Developer and run it.