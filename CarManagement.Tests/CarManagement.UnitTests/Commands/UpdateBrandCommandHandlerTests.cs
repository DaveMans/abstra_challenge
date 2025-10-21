using CarManagement.Application.Brands.Commands;
using CarManagement.Application.Brands.Mapping;

namespace CarManagement.UnitTests.Commands;

public class UpdateBrandCommandHandlerTests
{
    private readonly Mock<IBrandRepository> _brands = new();
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly IMapper _mapper;

    public UpdateBrandCommandHandlerTests()
    {
        var cfg = new MapperConfiguration(c => c.AddProfile(new BrandProfile()));
        _mapper = cfg.CreateMapper();
    }

    [Fact]
    public async Task Handle_Should_Return_Null_When_NotFound()
    {
        var handler = new UpdateBrandCommandHandler(_brands.Object, _uow.Object, _mapper);
        _brands.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync((Brand?)null);

        var result = await handler.Handle(new UpdateBrandCommand(Guid.NewGuid(), "Name", "Country", 2000, true), CancellationToken.None);

        result.Should().BeNull();
        _uow.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Update_And_Return_Dto()
    {
        var existing = new Brand { Id = Guid.NewGuid(), Name = "Old", Country = "OldC", FoundedYear = 1900, IsActive = true };
        var handler = new UpdateBrandCommandHandler(_brands.Object, _uow.Object, _mapper);
        _brands.Setup(x => x.GetByIdAsync(existing.Id, It.IsAny<CancellationToken>())).ReturnsAsync(existing);
        _uow.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var result = await handler.Handle(new UpdateBrandCommand(existing.Id, "  New  ", "  USA ", 2020, false), CancellationToken.None);

        existing.Name.Should().Be("New");
        existing.Country.Should().Be("USA");
        existing.FoundedYear.Should().Be(2020);
        existing.IsActive.Should().BeFalse();

        result.Should().NotBeNull();
        result!.Id.Should().Be(existing.Id);
        result.Name.Should().Be("New");
        result.Country.Should().Be("USA");
        result.FoundedYear.Should().Be(2020);

        _brands.Verify(x => x.Update(existing), Times.Once);
        _uow.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
