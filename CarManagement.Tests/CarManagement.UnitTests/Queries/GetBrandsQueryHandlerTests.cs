using CarManagement.Application.Brands.Mapping;

namespace CarManagement.UnitTests.Queries;

public class GetBrandsQueryHandlerTests
{
    private readonly Mock<IBrandRepository> _brands = new();
    private readonly IMapper _mapper;

    public GetBrandsQueryHandlerTests()
    {
        var cfg = new MapperConfiguration(c => c.AddProfile(new BrandProfile()));
        _mapper = cfg.CreateMapper();
    }

    [Fact]
    public async Task Handle_Should_Return_Empty_Page_When_No_Data()
    {
        var handler = new GetBrandsQueryHandler(_brands.Object, _mapper);
        _brands.Setup(x => x.ListAsync(It.IsAny<ISpecification<Brand>>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(Array.Empty<Brand>());
        _brands.Setup(x => x.CountAsync(It.IsAny<ISpecification<Brand>>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(0);

        var result = await handler.Handle(new GetBrandsQuery(Page: 1, PageSize: 10), CancellationToken.None);

        result.Items.Should().BeEmpty();
        result.Page.Should().Be(1);
        result.PageSize.Should().Be(10);
        result.TotalCount.Should().Be(0);
        result.TotalPages.Should().Be(0);
    }

    [Fact]
    public async Task Handle_Should_Map_Items_And_Set_Pagination()
    {
        var handler = new GetBrandsQueryHandler(_brands.Object, _mapper);
        var items = new List<Brand>
        {
            new Brand { Id = Guid.NewGuid(), Name = "Toyota", Country = "Japan", FoundedYear = 1937, IsActive = true },
            new Brand { Id = Guid.NewGuid(), Name = "Tesla", Country = "USA", FoundedYear = 2003, IsActive = true }
        };
        _brands.Setup(x => x.ListAsync(It.IsAny<ISpecification<Brand>>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(items);
        _brands.Setup(x => x.CountAsync(It.IsAny<ISpecification<Brand>>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(42);

        var query = new GetBrandsQuery(Page: 2, PageSize: 2, Search: "t", Country: null, SortBy: "Name", Desc: false);
        var result = await handler.Handle(query, CancellationToken.None);

        result.Items.Should().HaveCount(2);
        result.Items.Select(x => x.Name).Should().Contain(new[] { "Toyota", "Tesla" });
        result.Page.Should().Be(2);
        result.PageSize.Should().Be(2);
        result.TotalCount.Should().Be(42);
        result.TotalPages.Should().Be((int)Math.Ceiling(42 / 2.0));
    }
}
