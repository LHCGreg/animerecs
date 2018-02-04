using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace AnimeRecs.Web
{
    enum ExitCode
    {
        Success = 0,
        Error = 1
    }

    public class Program
    {
        public static int Main(string[] args)
        {
            try
            {
                BuildWebHost(args).Run();
            }
            catch (Exception ex)
            {
                if (Logging.Log != null)
                {
                    Logging.Log.FatalFormat("Fatal error: {0}", ex, ex.Message);
                }
                else
                {
                    Console.Error.WriteLine("Fatal error: {0}", ex);
                }

                return (int)ExitCode.Error;
            }

            Logging.Log.Info("Shutdown complete.");
            return (int)ExitCode.Success;
        }

        private static CommandLineArgs ReadCommandLine(string[] args)
        {
            CommandLineArgs commandLine = new CommandLineArgs(args);
            if (commandLine.ShowHelp)
            {
                commandLine.DisplayHelp(Console.Out);
                Environment.Exit((int)ExitCode.Success);
            }

            return commandLine;
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            IWebHostBuilder hostBuilder = new WebHostBuilder();

            CommandLineArgs commandLine = ReadCommandLine(args);
            //Config config = Config.LoadFromFile(commandLine.ConfigFile);

            // Load the config file to read some early startup settings
            // Namely, the logging config file path and everything in the Hosting section.
            // ASP.NET Core will read the same file again because the rest of the app's settings are there.
            IConfigurationBuilder bootstrapConfigBuilder = new ConfigurationBuilder()
                .AddXmlFile(commandLine.ConfigFile);
            IConfigurationRoot bootstrapRawConfig = bootstrapConfigBuilder.Build();
            Config bootstrapConfig = bootstrapRawConfig.Get<Config>();

            if (!string.IsNullOrWhiteSpace(bootstrapConfig.LoggingConfigPath))
            {
                Logging.SetUpLogging(bootstrapConfig.LoggingConfigPath);
                hostBuilder.UseNLog();
            }
            else
            {
                Console.Error.WriteLine("No logging configuration file set. Logging to console.");
                Logging.SetUpConsoleLogging();
                hostBuilder.ConfigureLogging((hostingContext, logging) =>
                {
                    logging.SetMinimumLevel(LogLevel.Information);
                    logging.AddConsole();
                });
            }

            string appDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            hostBuilder
                .UseKestrel(options =>
                {
                    if (bootstrapConfig.Hosting.UnixSocketPath != null)
                    {
                        options.ListenUnixSocket(bootstrapConfig.Hosting.UnixSocketPath);
                    }
                    else
                    {
                        options.Listen(IPAddress.Loopback, bootstrapConfig.Hosting.Port);
                    }
                })
                 //.UseContentRoot(appDir)
                 .UseContentRoot(Environment.CurrentDirectory)
                 .ConfigureAppConfiguration((context, configBuilder) =>
                 {
                     configBuilder.AddXmlFile(commandLine.ConfigFile, optional: false, reloadOnChange: true);
                 })
                 .UseStartup<Startup>();

            return hostBuilder.Build();
        }
    }
}
