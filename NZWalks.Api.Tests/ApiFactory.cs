using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using NZWalks.Api.Data;
using NZWalks.Api.Models;
using NZWalks.Api.Repository;

namespace NZWalks.Api.Tests;

public sealed class ApiFactory : WebApplicationFactory<Program>
{
    private readonly string databaseName = $"NZWalksTests_{Guid.NewGuid():N}";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.UseContentRoot(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "NZWalks.Api")));
        builder.ConfigureServices(services =>
        {
            services.RemoveAll<DbContextOptions<NZWalksDbContext>>();
            services.RemoveAll<IDbContextOptionsConfiguration<NZWalksDbContext>>();
            services.RemoveAll<NZWalksDbContext>();
            services.AddDbContext<NZWalksDbContext>(options => options.UseInMemoryDatabase(databaseName));

            services.RemoveAll<DbContextOptions<NZWalksAuthDbContext>>();
            services.RemoveAll<IDbContextOptionsConfiguration<NZWalksAuthDbContext>>();
            services.RemoveAll<NZWalksAuthDbContext>();
            services.AddDbContext<NZWalksAuthDbContext>(options => options.UseInMemoryDatabase($"{databaseName}_Auth"));

            services.RemoveAll<IImageRepository>();
            services.AddSingleton<IImageRepository, StubImageRepository>();
        });
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        var host = base.CreateHost(builder);
        using var scope = host.Services.CreateScope();
        scope.ServiceProvider.GetRequiredService<NZWalksDbContext>().Database.EnsureCreated();
        scope.ServiceProvider.GetRequiredService<NZWalksAuthDbContext>().Database.EnsureCreated();
        return host;
    }

    private sealed class StubImageRepository : IImageRepository
    {
        public Task<Image> Upload(Image image) => Task.FromResult(image);
    }
}
