using TestingTypedHttpClient.HttpClients;
using Microsoft.Extensions.DependencyInjection;

var serviceProvider = new ServiceCollection()
    .AddMyAwesomeHttpClient()
    .BuildServiceProvider();

var myAwesomeHttpClient = serviceProvider.GetRequiredService<MyAwesomeHttpClient>();
var joke = await myAwesomeHttpClient.GetRandomDadJoke();

Console.WriteLine(joke);