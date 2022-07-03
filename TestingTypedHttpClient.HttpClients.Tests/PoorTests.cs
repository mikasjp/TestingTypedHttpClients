using Moq;
using System;
using System.Net.Http;
using System.Net.Mime;
using NUnit.Framework;
using FluentAssertions;
using RichardSzalay.MockHttp;
using System.Threading.Tasks;

namespace TestingTypedHttpClient.HttpClients.Tests;

public class PoorTests
{
    [Test]
    public async Task MyAwesomeTypedHttpClient_ShouldSucceed_WhenRequestingForRandomDadJoke()
    {
        const string ValidJokeResponse =
            "My dog used to chase people on a bike a lot. It got so bad I had to take his bike away.";
        // Create and configure HttpMessageHandler mock
        var httpMessageHandlerMock = new MockHttpMessageHandler();
        httpMessageHandlerMock
            .When(HttpMethod.Get, "https://icanhazdadjoke.com/")
            .Respond(MediaTypeNames.Text.Plain, ValidJokeResponse);
        // Create mock HttpClient out of HttpMessageHandler mock
        var httpClientMock = httpMessageHandlerMock.ToHttpClient();
        // Configure it manually
        httpClientMock.BaseAddress = new Uri("https://icanhazdadjoke.com/", UriKind.Absolute);
        // Create MyAwesomeHttpClient instance manually
        var sut = new MyAwesomeHttpClient(
            httpClientMock,
            Mock.Of<ISomeOtherDependency>());

        var result = await sut.GetRandomDadJoke();

        result.Should()
            .Be(ValidJokeResponse);
    }
}