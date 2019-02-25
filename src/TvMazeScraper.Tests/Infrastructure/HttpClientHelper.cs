using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;

namespace TvMazeScraper.Tests.Infrastructure
{
    public class HttpClientHelper
    {
        public static HttpClient GetMockHttpClient(HttpStatusCode httpStatusCode, string responseStringContent)
        {
            var handlerMock = GetMockHttpMessageHandler(httpStatusCode, responseStringContent);
            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://test.com/"),
            };

            return httpClient;
        }

        public static Mock<HttpMessageHandler> GetMockHttpMessageHandler(HttpStatusCode httpStatusCode, string responseStringContent)
        {
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = httpStatusCode,
                    Content = new StringContent(responseStringContent)
                })
                .Verifiable();
            return handlerMock;
        }
    }
}
