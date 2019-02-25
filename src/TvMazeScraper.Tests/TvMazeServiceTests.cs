using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using TvMazeScraper.Tests.Infrastructure;
using TvMazeScraper.TvMazeClient;

namespace TvMazeScraper.Tests
{
    public class TvMazeServiceTests
    {
        [Test]
        public async Task GetShowsAsync_OkResponse_Should_Return_Rows()
        {
            // Arrange
            var httpClient = HttpClientHelper.GetMockHttpClient(HttpStatusCode.OK, File.ReadAllText("seed/shows.json"));
            var tvMazeService = new TvMazeService(httpClient);

            // Act
            var shows = await tvMazeService.GetShowsAsync(1);

            // Assert
            Assert.IsNotEmpty(shows);
        }

        [Test]
        public async Task GetShowsAsync_PageNotFound_Should_Return_EmptyList()
        {
            // Arrange
            var httpClient = HttpClientHelper.GetMockHttpClient(HttpStatusCode.NotFound, String.Empty);
            var tvMazeService = new TvMazeService(httpClient);

            // Act
            var shows = await tvMazeService.GetShowsAsync(1);

            // Assert
            Assert.IsEmpty(shows);
        }

        [Test]
        public void GetShowsAsync_InternalServerError_Should_Throw_HttpRequestException()
        {
            // Arrange
            var httpClient = HttpClientHelper.GetMockHttpClient(HttpStatusCode.InternalServerError, String.Empty);
            var tvMazeService = new TvMazeService(httpClient);

            // Act Assert
            Assert.ThrowsAsync<HttpRequestException>(async () => await tvMazeService.GetShowsAsync(1));
        }

        [Test]
        public async Task GetShowCastsAsync_NullBirthday_Should_Deserialize()
        {
            // Arrange
            var httpClient = HttpClientHelper.GetMockHttpClient(HttpStatusCode.OK, File.ReadAllText("seed/cast.json"));
            var tvMazeService = new TvMazeService(httpClient);

            // Act
            var casts = await tvMazeService.GetShowCastsAsync(1);

            // Assert
            Assert.IsNotEmpty(casts);
        }

        [Test]
        public async Task GetShowCastsAsync_ShowNotFound_Should_Return_EmptyList()
        {
            // Arrange
            var httpClient = HttpClientHelper.GetMockHttpClient(HttpStatusCode.NotFound, String.Empty);
            var tvMazeService = new TvMazeService(httpClient);

            // Act
            var shows = await tvMazeService.GetShowCastsAsync(1);

            // Assert
            Assert.IsEmpty(shows);
        }

        [Test]
        public async Task GetRetryPolicy_RateLimited_HTTP429_Should_Retry()
        {
            // Arrange
            var handlerMock = HttpClientHelper.GetMockHttpMessageHandler((HttpStatusCode)429, String.Empty);
            var httpClient = new HttpClient(handlerMock.Object) { BaseAddress = new Uri("http://test.com/") };
            var timeout = TimeSpan.FromSeconds(5);
            var stopwatch = new Stopwatch();

            // Act
            stopwatch.Start();
            await TvMazeCollectionExtensions.GetRetryPolicy(1, timeout).ExecuteAndCaptureAsync(() => httpClient.GetAsync("/"));
            stopwatch.Stop();

            // Assert
            Assert.AreEqual(2, handlerMock.Invocations.Count);
            Assert.IsTrue(stopwatch.ElapsedTicks >= timeout.Ticks);
        }
    }
}
