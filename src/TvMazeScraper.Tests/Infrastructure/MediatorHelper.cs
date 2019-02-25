using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using TvMazeScraper.Api.Application.Commands;

namespace TvMazeScraper.Tests.Infrastructure
{
    public static class MediatorHelper
    {
        public static IMediator BuildMediator()
        {
            var services = new ServiceCollection();
            services.AddMediatR(configuration => { }, new Assembly[] { typeof(SyncTvMazeDbCommand).Assembly });
            var provider = services.BuildServiceProvider();
            return provider.GetRequiredService<IMediator>();
        }
    }
}
