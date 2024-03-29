using System.Data.Common;
using DotNet.Testcontainers.Builders;
using EntityFramework.Exceptions.PostgreSQL;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Respawn;
using REST.Data;
using Testcontainers.PostgreSql;

namespace REST.IntegrationTests.Fixtures;

public class RestWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private const string BasePath = "api/v1.0/";
    
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:latest")
        .WithDatabase("distcomp")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .WithPortBinding(5432, true)
        .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5432))
        .WithCleanUp(true)
        .Build();
    public HttpClient HttpClient { get; private set; } = null!;

    private DbConnection _dbConnection = null!;
    private Respawner _respawner = null!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var descriptor = services.SingleOrDefault(d =>
                d.ServiceType == typeof(DbContextOptions<AppDbContext>));

            if (descriptor != null) services.Remove(descriptor);

            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(_dbContainer.GetConnectionString()).UseExceptionProcessor());
        });
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        
        HttpClient = CreateClient();
        if (HttpClient.BaseAddress != null) HttpClient.BaseAddress = new Uri(HttpClient.BaseAddress.AbsoluteUri + BasePath);

        
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(_dbContainer.GetConnectionString())
            .Options;

        await using AppDbContext context = new AppDbContext(options);
        await context.Database.MigrateAsync();

        _dbConnection = new NpgsqlConnection(_dbContainer.GetConnectionString());
        await InitializeRespawnerAsync();
    }

    private async Task InitializeRespawnerAsync()
    {
        await _dbConnection.OpenAsync();
        _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            SchemasToInclude = ["public"]
        });
    }
    
    public async Task ResetDatabase()
    {
        await _respawner.ResetAsync(_dbConnection);
    }

    public new async Task DisposeAsync() => await _dbContainer.DisposeAsync();
}