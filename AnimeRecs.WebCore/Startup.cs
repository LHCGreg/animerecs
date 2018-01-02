using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AnimeRecs.WebCore
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
            services.AddMvc();
            services.Configure<Config.HtmlConfig>(Configuration.GetSection("Html"));
            services.Configure<Config.MalApiConfig>(Configuration.GetSection("MalApi"));
            services.Configure<Config.RecommendationsConfig>(Configuration.GetSection("Recommendations"));
            // TODO: Add services
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime appLifetime, IOptions<Config.HtmlConfig> htmlConfigGetter)
        {
            Config.HtmlConfig htmlConfig = htmlConfigGetter.Value;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                // TODO: Set up error handling
                app.UseExceptionHandler("/Home/Error");
            }

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
