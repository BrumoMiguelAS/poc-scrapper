namespace Scrapper.Presentation.Web
{
    using Hangfire;
    using Hangfire.Mongo;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Scrapper.Applciation.Services.Media;
    using Scrapper.Application.Services.Media;
    using Scrapper.Application.Services.Scrapping.Sources;
    using Scrapper.Data.Gateway.Media;
    using Scrapper.Data.Gateway.Scrapping;
    using Scrapper.Data.Repository.Interfaces;
    using Scrapper.Data.Repository.Mongo;
    using Scrapper.Domain.Core.Interfaces.Media;
    using Scrapper.Domain.Core.Interfaces.Scrapping.Sources;
    using Scrapper.Domain.Services.Media;
    using Scrapper.Domain.Services.Scrapping.Sources;

    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("Configs/appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"Configs/appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var mongoSettings = Configuration.GetSection("MongoConnection");
            var mongoConnection = mongoSettings.GetValue<string>("ConnectionString");
            var mongoDatabase = mongoSettings.GetValue<string>("Database");

            services.AddMvc();
            services.AddHangfire(x => x.UseMongoStorage(mongoConnection, mongoDatabase));

            this.RegisterApplicationServices(services);
            this.RegisterDomainServices(services);
            this.RegisterRepositories(services);
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
            app.UseHangfireServer();
            app.UseHangfireDashboard();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void RegisterApplicationServices(IServiceCollection services)
        {
            services.AddTransient<IMediaService, MediaService>();
            services.AddTransient<IScrappingSourceService, ScrappingSourceService>();
        }

        private void RegisterDomainServices(IServiceCollection services)
        {
            var mediaStorageSettings = Configuration.GetSection("MediaStorage");

            services.AddTransient<IMediaDataService, MediaDataService>();
            services.AddTransient<IScrappingSourceDataService, ScrappingSourceDataService>();
            services.AddTransient<IScrappingSourceProcessor, ScrappingSourceProcessor>();
            services.AddTransient<IImageDownloader>(x => new ImageDownloader(mediaStorageSettings.GetValue<string>("Path")));
            services.AddTransient<IImageAnalyser>(x => new GoogleImageAnalyser(mediaStorageSettings.GetValue<string>("Path")));
        }

        private void RegisterRepositories(IServiceCollection services)
        {
            var mongoSettings = Configuration.GetSection("MongoConnection");
            var mongoConnection = mongoSettings.GetValue<string>("ConnectionString");
            var mongoDatabase = mongoSettings.GetValue<string>("Database");

            services.AddTransient<IMediaRepository>(repo => new MediaRepository(mongoConnection, mongoDatabase));
            services.AddTransient<IScrappingSourceRepository>(repo => new ScrappingSourceRepository(mongoConnection, mongoDatabase));
        }
    }
}
