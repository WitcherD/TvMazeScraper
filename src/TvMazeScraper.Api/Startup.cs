using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using TvMazeScraper.Api.Application.Jobs;
using TvMazeScraper.Domain.Repositories;
using TvMazeScraper.Infrastructure;
using TvMazeScraper.Infrastructure.Repositories;
using TvMazeScraper.TvMazeClient;
using TvMazeScraper.TvMazeClient.Options;

namespace TvMazeScraper.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, TvMazeDbContext context)
        {
            app.UseMvc();

            var hagfireOptions = new BackgroundJobServerOptions { WorkerCount = 1 };
            app.UseHangfireServer(hagfireOptions);
            app.UseHangfireDashboard();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            context.Database.Migrate();
            RecurringJob.AddOrUpdate<SyncTvMazeDbJob>(job => job.ExecuteAsync(), Cron.Hourly);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddMediatR();

            services.AddTransient<SyncTvMazeDbJob>();

            services.AddDbContext<TvMazeDbContext>(i => i.UseSqlServer(_configuration.GetConnectionString("DefaultConnection")));

            services.AddTransient<IShowsRepository, EfCoreShowsRepository>();
            services.AddTransient<IPersonsRepository, EfCorePersonsRepository>();

            services.AddTvMazeClient(_configuration.GetSection("TvMazeService").Get<TvMazeOptions>());
            services.AddHangfire(i => i.UseSqlServerStorage(_configuration.GetConnectionString("DefaultConnection")));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "TVMaze Api", Version = "v1" });
            });
        }
    }
}
