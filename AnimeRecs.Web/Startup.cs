using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AnimeRecs.DAL;
using MalApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;

namespace AnimeRecs.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IHostingEnvironment HostingEnvironment { get; }

        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                // Use property names as is on classes that get serialized, instead of camelcasing.
                .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());
            services.AddOptions();
            services.Configure<Config.HtmlConfig>(Configuration.GetSection("Html"));
            services.Configure<Config.MalApiConfig>(Configuration.GetSection("MalApi"));
            services.Configure<Config.RecommendationsConfig>(Configuration.GetSection("Recommendations"));
            services.Configure<Config.ConnectionStringsConfig>(Configuration.GetSection("ConnectionStrings"));

            services.AddTransient<IAnimeRecsClientFactory, ConfigBasedRecClientFactory>();
            services.AddTransient<IAnimeRecsDbConnectionFactory, ConfigBasedAnimeRecsDbConnectionFactory>();

            IMyAnimeListApi api;
            Config.MalApiConfig apiConfig = Configuration.GetSection("MalApi").Get<Config.MalApiConfig>();
            if (apiConfig.Type == Config.MalApiType.Normal)
            {
                api = new MyAnimeListApi()
                {
                    UserAgent = apiConfig.UserAgentString,
                    TimeoutInMs = apiConfig.TimeoutMilliseconds,
                };
            }
            else if (apiConfig.Type == Config.MalApiType.DB)
            {
                api = new PgMyAnimeListApi(Configuration.GetConnectionString("AnimeRecs"));
            }
            else
            {
                throw new Exception($"Don't know how to construct MAL API type {apiConfig.Type}.");
            }
            CachingMyAnimeListApi cachingApi = new CachingMyAnimeListApi(api, TimeSpan.FromSeconds(apiConfig.AnimeListCacheExpirationSeconds), ownApi: true);
            SingletonMyAnimeListApiFactory factory = new SingletonMyAnimeListApiFactory(cachingApi);
            services.AddSingleton<IMyAnimeListApiFactory>(factory);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime appLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
            }

            // This error handler is for both API and user-facing URLs
            app.UseExceptionHandler("/error/500");
            app.UseStatusCodePages();

            //app.UseExceptionHandler(builder =>
            //{
            //    builder.Run(async context =>
            //    {
            //        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            //    });
            //});

            //appLifetime.ApplicationStarted.Register
            //appLifetime.ApplicationStopping.Register
            //appLifetime.ApplicationStopped.Register

            app.UseForwardedHeaders();

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
