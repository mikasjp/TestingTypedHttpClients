using Polly;
using System.Net;
using System.Net.Mime;
using Polly.Extensions.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace TestingTypedHttpClient.HttpClients;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddMyAwesomeHttpClient(this IServiceCollection services)
    {
        services
            // Let's register our HTTP client
            .AddHttpClient<MyAwesomeHttpClient>()
            // Then we need to configure it
            .ConfigureHttpClient(httpClient =>
            {
                httpClient.BaseAddress = new Uri("https://icanhazdadjoke.com", UriKind.Absolute);
                httpClient.DefaultRequestHeaders
                    .Add(nameof(HttpRequestHeader.Accept), MediaTypeNames.Text.Plain);
            })
            // Or maybe we want to add some exception policies to it?
            .AddPolicyHandler(HttpPolicyExtensions
                .HandleTransientHttpError()
                .RetryAsync(retryCount: 3));
        
        // Don't not forget about our client's dependencies!
        services.TryAddTransient<ISomeOtherDependency, SomeOtherDependency>();

        return services;
    }
}