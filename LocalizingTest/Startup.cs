using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Localization.Routing;

namespace LocalizingTest
{
    public class Startup
    {
        public static string CULTURE_COOKIE_KEY = "culture-test";

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLocalization(locSetup => locSetup.ResourcesPath = "Resources");

            var supportedCultures = new CultureInfo[]
            {
                //new CultureInfo("de"),
                new CultureInfo("de-DE"),
                new CultureInfo("en-GB"),
                //new CultureInfo("en"),
                //new CultureInfo("en-US"),
            };

            services.Configure<RequestLocalizationOptions>(s => 
            {
                s.SupportedCultures = supportedCultures;
                s.SupportedUICultures  = supportedCultures;
                s.DefaultRequestCulture = new RequestCulture(culture: "en-GB", uiCulture: "en-GB");
                s.RequestCultureProviders.Clear();
                s.RequestCultureProviders.Insert(0, new QueryStringRequestCultureProvider());
                //s.RequestCultureProviders.Insert(0, new RouteDataRequestCultureProvider());
                //s.RequestCultureProviders.Insert(2, new CookieRequestCultureProvider() { CookieName = CULTURE_COOKIE_KEY, Options = options });
                s.RequestCultureProviders.Insert(1, new AcceptLanguageHeaderRequestCultureProvider());
            });


            services
                .AddMvc()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix, s => s.ResourcesPath = "Resources")
                .AddDataAnnotationsLocalization();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(locOptions.Value);

            app.UseMvc();//routes =>
            // {
            //     //routes.MapRoute(
            //         // name: "no-culture-default",
            //         // template: "de/{controller=Home}/{action=Index}/{id?}");

            //     routes.MapRoute(
            //         name: "default",
            //         template: "{culture}/{controller=Home}/{action=Index}/{id?}");
            // });
        }
    }
}
