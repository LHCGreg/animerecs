To build and run the site for development, you will need to install the following:

- Microsoft Visual Web Developer 2010 Express
- Microsoft Visual C# 2010 Express
- MongoDB (http://www.mongodb.org/downloads)
- Google Dart editor (http://www.dartlang.org/docs/getting-started/editor/index-win.html)


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



Next, compile the dart code to javascript by opening AnimeRecs.dart in the Dart editor. Change one character in the code, then change it back, then save to force a compile.



Now open up the UpdateRecommendorCache solution in Visual C#. Change the log file path in App.config from C:\Users\Greg\AppData\Roaming\AnimeRecs\UpdateRecommendorCache.log to something more suitable for you. Compile it and run it. This will build the recommendor database. The Mongo server must be running.



Now you are able to open the AnimeRecs solution in Visual Web Developer and run it.