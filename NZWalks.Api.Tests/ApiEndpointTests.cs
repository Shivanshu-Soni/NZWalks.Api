using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using NZWalks.Api.Models.DTO;
using Xunit;

namespace NZWalks.Api.Tests;

public class ApiEndpointTests
{
    private static readonly Guid AucklandId = Guid.Parse("62e43591-0a93-41fc-8d64-6f2e36d52749");
    private static readonly Guid EasyDifficultyId = Guid.Parse("1ad5baba-b6e0-4bba-8c26-1b87ebb681dd");

    [Fact]
    public async Task GetRegions_ReturnsSeededRegions()
    {
        using var factory = new ApiFactory();
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/api/regions");
        var regions = await response.Content.ReadFromJsonAsync<List<RegionDto>>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(regions);
        Assert.Contains(regions, region => region.Id == AucklandId && region.Name == "AuckLand");
    }

    [Fact]
    public async Task RegionLookup_RequiresReaderRole_AndReturnsRequestedRegion()
    {
        using var factory = new ApiFactory();
        using var client = factory.CreateClient();

        var unauthorized = await client.GetAsync($"/api/regions/{AucklandId}");
        Assert.Equal(HttpStatusCode.Unauthorized, unauthorized.StatusCode);

        await AuthenticateAsync(client, "reader@example.com", "Reader");
        var response = await client.GetAsync($"/api/regions/{AucklandId}");
        var region = await response.Content.ReadFromJsonAsync<RegionDto>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(region);
        Assert.Equal(AucklandId, region.Id);
        Assert.Equal("AuckLand", region.Name);
    }

    [Fact]
    public async Task RegionWriter_CanCreateUpdateAndDeleteRegion()
    {
        using var factory = new ApiFactory();
        using var client = factory.CreateClient();
        await AuthenticateAsync(client, "writer@example.com", "Writer");

        var created = await client.PostAsJsonAsync("/api/regions", new
        {
            Code = "TST",
            Name = "Test Region",
            RegionImageUrl = "https://example.test/region.jpg"
        });
        var region = await created.Content.ReadFromJsonAsync<RegionDto>();

        Assert.Equal(HttpStatusCode.Created, created.StatusCode);
        Assert.NotNull(region);
        Assert.NotEqual(Guid.Empty, region.Id);

        var updated = await client.PutAsJsonAsync($"/api/regions/{region.Id}", new
        {
            Code = "TST2",
            Name = "Updated Region",
            RegionImageUrl = "https://example.test/updated.jpg"
        });
        Assert.Equal(HttpStatusCode.OK, updated.StatusCode);

        var deleted = await client.DeleteAsync($"/api/regions/{region.Id}");
        Assert.Equal(HttpStatusCode.OK, deleted.StatusCode);

        var missing = await client.DeleteAsync($"/api/regions/{region.Id}");
        Assert.Equal(HttpStatusCode.NotFound, missing.StatusCode);
    }

    [Fact]
    public async Task Walks_CanBeCreatedListedReadUpdatedAndDeleted()
    {
        using var factory = new ApiFactory();
        using var client = factory.CreateClient();

        var created = await client.PostAsJsonAsync("/api/walks", NewWalk("Coastal Track"));
        var createdWalk = await created.Content.ReadFromJsonAsync<WalksDTO>();

        Assert.Equal(HttpStatusCode.OK, created.StatusCode);
        Assert.NotNull(createdWalk);
        Assert.NotEqual(Guid.Empty, createdWalk.Id);
        Assert.Equal("Coastal Track", createdWalk.Name);

        var listed = await client.GetFromJsonAsync<List<WalksDTO>>("/api/walks");
        Assert.NotNull(listed);
        Assert.Contains(listed, walk => walk.Id == createdWalk.Id);

        var fetched = await client.GetAsync($"/api/walks/{createdWalk.Id}");
        Assert.Equal(HttpStatusCode.OK, fetched.StatusCode);

        var updated = await client.PutAsJsonAsync($"/api/walks/{createdWalk.Id}", NewWalk("Updated Track"));
        Assert.Equal(HttpStatusCode.OK, updated.StatusCode);

        var deleted = await client.DeleteAsync($"/api/walks/{createdWalk.Id}");
        Assert.Equal(HttpStatusCode.OK, deleted.StatusCode);

        var missing = await client.GetAsync($"/api/walks/{createdWalk.Id}");
        Assert.Equal(HttpStatusCode.NotFound, missing.StatusCode);
    }

    [Fact]
    public async Task CreateWalk_RejectsInvalidRequest()
    {
        using var factory = new ApiFactory();
        using var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync("/api/walks", new
        {
            Name = "",
            Description = "",
            LenghtInkm = 100,
            DifficultyId = Guid.Empty,
            regionId = Guid.Empty
        });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task ImageUpload_AcceptsPng_AndRejectsUnsupportedExtensions()
    {
        using var factory = new ApiFactory();
        using var client = factory.CreateClient();

        using var pngRequest = CreateImageUpload("trail.png");
        var pngResponse = await client.PostAsync("/api/images/upload", pngRequest);
        Assert.Equal(HttpStatusCode.OK, pngResponse.StatusCode);

        using var unsupportedRequest = CreateImageUpload("trail.txt");
        var unsupportedResponse = await client.PostAsync("/api/images/upload", unsupportedRequest);
        Assert.Equal(HttpStatusCode.BadRequest, unsupportedResponse.StatusCode);
    }

    private static object NewWalk(string name) => new
    {
        Name = name,
        Description = "A well-marked walk.",
        LenghtInkm = 8.5,
        DifficultyId = EasyDifficultyId,
        regionId = AucklandId
    };

    private static MultipartFormDataContent CreateImageUpload(string fileName)
    {
        var request = new MultipartFormDataContent();
        request.Add(new ByteArrayContent([1, 2, 3]), "File", fileName);
        request.Add(new StringContent("test-image"), "FileName");
        return request;
    }

    private static async Task AuthenticateAsync(HttpClient client, string email, string role)
    {
        var register = await client.PostAsJsonAsync("/api/auth/register", new
        {
            UserName = email,
            Password = "TestPass123",
            Roles = new[] { role }
        });
        Assert.Equal(HttpStatusCode.OK, register.StatusCode);

        var login = await client.PostAsJsonAsync("/api/auth/login", new
        {
            UserName = email,
            Password = "TestPass123"
        });
        Assert.Equal(HttpStatusCode.OK, login.StatusCode);

        using var token = JsonDocument.Parse(await login.Content.ReadAsStringAsync());
        var jwt = token.RootElement.GetProperty("jwtToken").GetString();
        Assert.False(string.IsNullOrWhiteSpace(jwt));
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
    }
}
