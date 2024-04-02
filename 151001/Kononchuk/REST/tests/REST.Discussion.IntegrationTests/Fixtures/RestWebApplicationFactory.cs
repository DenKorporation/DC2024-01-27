using Microsoft.AspNetCore.Mvc.Testing;

namespace REST.Discussion.IntegrationTests.Fixtures;

public class RestWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private const string BasePath = "api/v1.0/";
    
    public HttpClient HttpClient { get; private set; } = null!;

    public Task InitializeAsync()
    {
        HttpClient = CreateClient();
        if (HttpClient.BaseAddress != null) HttpClient.BaseAddress = new Uri(HttpClient.BaseAddress.AbsoluteUri + BasePath);
        return Task.CompletedTask;
    }

    public new Task DisposeAsync() => Task.CompletedTask;
}