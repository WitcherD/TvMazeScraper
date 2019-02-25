using System;
using System.Net;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using TvMazeScraper.TvMazeClient.Options;

namespace TvMazeScraper.TvMazeClient
{
    public static class TvMazeCollectionExtensions
    {
        public static void AddTvMazeClient(this IServiceCollection services, TvMazeOptions options)
        {
            services.AddHttpClient<ITvMazeService, TvMazeService>()
                .ConfigureHttpClient(i => { i.BaseAddress = new Uri(options.BaseAddress); })
                .AddPolicyHandler(GetRetryPolicy(Int32.MaxValue, options.RetryTimeout));
        }

        public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(int retryCount, TimeSpan timeout) => HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(i => i.StatusCode == (HttpStatusCode)429)
            .WaitAndRetryAsync(retryCount, retryAttempt => timeout);
    }
}
