using CarManagement.Application.Brands.Commands;
using CarManagement.Application.Brands.Mapping;

namespace CarManagement.UnitTests.Commands;

public class CreateBrandCommandHandlerTests
{
    private readonly Mock<IBrandRepository> _brands = new();
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly IMapper _mapper;

    public CreateBrandCommandHandlerTests()
    {
        var cfg = new MapperConfiguration(c => c.AddProfile(new BrandProfile()));
        _mapper = cfg.CreateMapper();
    }

    [Fact]
    public async Task Handle_Should_Create_Brand_And_Return_Dto()
    {
        var handler = new CreateBrandCommandHandler(_brands.Object, _uow.Object, _mapper);
        _uow.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        Brand? saved = null;
        _brands
            .Setup(x => x.AddAsync(It.IsAny<Brand>(), It.IsAny<CancellationToken>()))
            .Callback<Brand, CancellationToken>((b, _) => saved = b)
            .Returns(Task.CompletedTask);

        var cmd = new CreateBrandCommand("  Toyota  ", "  Japan ", 1937);
        var result = await handler.Handle(cmd, CancellationToken.None);

        saved.Should().NotBeNull();
        saved!.Name.Should().Be("Toyota");
        saved.Country.Should().Be("Japan");
        saved.FoundedYear.Should().Be(1937);
        saved.IsActive.Should().BeTrue();

        result.Should().NotBeNull();
        result.Id.Should().Be(saved.Id);
        result.Name.Should().Be("Toyota");
        result.Country.Should().Be("Japan");
        result.FoundedYear.Should().Be(1937);

        _brands.Verify(x => x.AddAsync(It.IsAny<Brand>(), It.IsAny<CancellationToken>()), Times.Once);
        _uow.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
