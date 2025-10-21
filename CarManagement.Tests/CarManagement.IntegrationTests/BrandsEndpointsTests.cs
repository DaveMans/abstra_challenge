using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;

namespace CarManagement.IntegrationTests;

public class BrandsEndpointsTests : IClassFixture<TestWebApplicationFactory>
{
    private readonly HttpClient _client;

    public BrandsEndpointsTests(TestWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Get_Brands_Should_Return_Paged_List()
    {
        var response = await _client.GetAsync("/api/v1.0/brands?page=1&pageSize=10");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        doc.RootElement.TryGetProperty("items", out var _).Should().BeTrue();
    }

    [Fact]
    public async Task Post_Brand_Then_GetById_Should_Return_Created_Brand()
    {
        var createRequest = new
        {
            name = "TestBrand",
            country = "Nowhere",
            foundedYear = 2020
        };

        var post = await _client.PostAsJsonAsync("/api/v1.0/brands", createRequest);
        post.StatusCode.Should().Be(HttpStatusCode.Created);

        var location = post.Headers.Location;
        location.Should().NotBeNull();

        var getCreated = await _client.GetAsync(location);
        getCreated.StatusCode.Should().Be(HttpStatusCode.OK);

        var created = await getCreated.Content.ReadFromJsonAsync<JsonElement>();
        created.TryGetProperty("name", out var nameProp).Should().BeTrue();
        nameProp.GetString().Should().Be("TestBrand");
    }
}
