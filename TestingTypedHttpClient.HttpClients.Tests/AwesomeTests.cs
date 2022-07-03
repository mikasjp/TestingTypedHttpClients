using Moq;
using System;
using System.Net;
using NUnit.Framework;
using System.Net.Http;
using System.Net.Mime;
using FluentAssertions;
using RichardSzalay.MockHttp;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TestingTypedHttpClient.HttpClients.Tests.Misc;

namespace TestingTypedHttpClient.HttpClients.Tests;

public class Tests
{
    [Test]
    public async Task MyAwesomeTypedHttpClient_ShouldSucceed_WhenRequestingForRandomDadJoke()
    {
        const string ValidJokeResponse =
            "My dog used to chase people on a bike a lot. It got so bad I had to take his bike away.";
        // Create and configure our HttpMessageHandler mock
        var httpMessageHandlerMock = new MockHttpMessageHandler();
        httpMessageHandlerMock
            .When(HttpMethod.Get, "https://icanhazdadjoke.com/")
            .Respond(MediaTypeNames.Text.Plain, ValidJokeResponse);
        // Create our test DI container
        var serviceProvider = new ServiceCollection()
            // Mock ISomeOtherDependency
            .AddTransient(_ => Mock.Of<ISomeOtherDependency>())
            // Add and configure our typed HTTP client
            .AddMyAwesomeHttpClient()
            // Overwrite what you need
            .OverridePrimaryHttpMessageHandler<MyAwesomeHttpClient>(httpMessageHandlerMock)
            .BuildServiceProvider();
        var sut = serviceProvider.GetRequiredService<MyAwesomeHttpClient>();

        var result = await sut.GetRandomDadJoke();

        result.Should()
            .Be(ValidJokeResponse);
    }

    [TestCase(HttpStatusCode.BadRequest, 1)]
    [TestCase(HttpStatusCode.InternalServerError, 4)]
    public async Task MyAwesomeTypedHttpClient_ShouldFail_WhenReceiveErrorResponseCode(
        HttpStatusCode httpStatusCode, int failedRequestCountBeforeException)
    {
        var httpMessageHandlerMock = new MockHttpMessageHandler();
        var testHttpRequestDefinition = httpMessageHandlerMock
            .When(HttpMethod.Get, "https://icanhazdadjoke.com/")
            .Respond(httpStatusCode);
        var serviceProvider = new ServiceCollection()
            .AddTransient(_ => Mock.Of<ISomeOtherDependency>())
            .AddMyAwesomeHttpClient()
            .OverridePrimaryHttpMessageHandler<MyAwesomeHttpClient>(httpMessageHandlerMock)
            .BuildServiceProvider();
        var sut = serviceProvider.GetRequiredService<MyAwesomeHttpClient>();

        var action = async () => await sut.GetRandomDadJoke();

        await action.Should()
            .ThrowAsync<Exception>();
        httpMessageHandlerMock.GetMatchCount(testHttpRequestDefinition)
            .Should().Be(failedRequestCountBeforeException);
    }
}