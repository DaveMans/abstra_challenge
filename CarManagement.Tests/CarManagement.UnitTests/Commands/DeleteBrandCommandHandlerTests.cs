using CarManagement.Application.Brands.Commands;
using CarManagement.Application.Brands.Specifications;

namespace CarManagement.UnitTests.Commands;

public class DeleteBrandCommandHandlerTests
{
    private readonly Mock<IBrandRepository> _brands = new();
    private readonly Mock<ILineRepository> _lines = new();
    private readonly Mock<IUnitOfWork> _uow = new();

    [Fact]
    public async Task Handle_Should_Return_False_When_NotFound()
    {
        var handler = new DeleteBrandCommandHandler(_brands.Object, _lines.Object, _uow.Object);
        _brands.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync((Brand?)null);

        var result = await handler.Handle(new DeleteBrandCommand(Guid.NewGuid()), CancellationToken.None);

        result.Should().BeFalse();
        _uow.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Throw_When_Has_Active_Lines()
    {
        var existing = new Brand { Id = Guid.NewGuid(), Name = "Brand" };
        var handler = new DeleteBrandCommandHandler(_brands.Object, _lines.Object, _uow.Object);
        _brands.Setup(x => x.GetByIdAsync(existing.Id, It.IsAny<CancellationToken>())).ReturnsAsync(existing);
        _lines.Setup(x => x.AnyAsync(It.IsAny<LinesByBrandActiveSpec>(), It.IsAny<CancellationToken>()))
              .ReturnsAsync(true);

        var act = async () => await handler.Handle(new DeleteBrandCommand(existing.Id), CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Cannot delete a brand with active lines.");
        _uow.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Delete_And_Return_True()
    {
        var existing = new Brand { Id = Guid.NewGuid(), Name = "Brand" };
        var handler = new DeleteBrandCommandHandler(_brands.Object, _lines.Object, _uow.Object);
        _brands.Setup(x => x.GetByIdAsync(existing.Id, It.IsAny<CancellationToken>())).ReturnsAsync(existing);
        _lines.Setup(x => x.AnyAsync(It.IsAny<LinesByBrandActiveSpec>(), It.IsAny<CancellationToken>()))
              .ReturnsAsync(false);
        _uow.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var result = await handler.Handle(new DeleteBrandCommand(existing.Id), CancellationToken.None);

        result.Should().BeTrue();
        _brands.Verify(x => x.Remove(existing), Times.Once);
        _uow.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
