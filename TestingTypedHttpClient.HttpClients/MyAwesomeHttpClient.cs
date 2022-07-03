namespace TestingTypedHttpClient.HttpClients;

public class MyAwesomeHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly ISomeOtherDependency _someOtherDependency;

    public MyAwesomeHttpClient(
        HttpClient httpClient,
        ISomeOtherDependency someOtherDependency)
    {
        _httpClient = httpClient;
        _someOtherDependency = someOtherDependency ?? throw new ArgumentNullException(nameof(someOtherDependency));
    }

    public async Task<string> GetRandomDadJoke()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, new Uri("/", UriKind.Relative));
        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
            throw new Exception("I can't haz dad joke...");

        _someOtherDependency.DoNothig();
        var joke = await response.Content.ReadAsStringAsync();

        return joke;
    }
}