namespace CarManagement.UnitTests.Queries;

public class GetBrandByIdQueryHandlerTests
{
    private readonly Mock<IBrandRepository> _brands = new();
    private readonly IMapper _mapper;

    public GetBrandByIdQueryHandlerTests()
    {
        var cfg = new MapperConfiguration(c => c.AddProfile(new BrandProfile()));
        _mapper = cfg.CreateMapper();
    }

    [Fact]
    public async Task Handle_Should_Return_Null_When_NotFound()
    {
        var handler = new GetBrandByIdQueryHandler(_brands.Object, _mapper);
        _brands.Setup(x => x.ListAsync(It.IsAny<ISpecification<Brand>>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(Array.Empty<Brand>());

        var result = await handler.Handle(new GetBrandByIdQuery(Guid.NewGuid()), CancellationToken.None);

        result.Should().BeNull();
    }

    [Fact]
    public async Task Handle_Should_Return_Dto_When_Found()
    {
        var brand = new Brand { Id = Guid.NewGuid(), Name = "Tesla", Country = "USA", FoundedYear = 2003, IsActive = true };
        var handler = new GetBrandByIdQueryHandler(_brands.Object, _mapper);
        _brands.Setup(x => x.ListAsync(It.IsAny<ISpecification<Brand>>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new[] { brand });

        var result = await handler.Handle(new GetBrandByIdQuery(brand.Id), CancellationToken.None);

        result.Should().NotBeNull();
        result!.Id.Should().Be(brand.Id);
        result.Name.Should().Be("Tesla");
        result.Country.Should().Be("USA");
        result.FoundedYear.Should().Be(2003);
        result.IsActive.Should().BeTrue();
    }
}
