using System;
using System.Net.Http;

namespace TestingTypedHttpClient.HttpClients.Tests.Misc;

internal sealed class HttpMessageHandlerMockWrapper
{
    public HttpMessageHandlerMockWrapper(
        Type typedHttpClientType,
        HttpMessageHandler httpMessageHandlerMock)
    {
        TypedHttpClientType = typedHttpClientType;
        HttpMessageHandlerMock = httpMessageHandlerMock;
    }
    
    public Type TypedHttpClientType { get; }
    public HttpMessageHandler HttpMessageHandlerMock { get; }
}
