using CarManagement.Application.Lines.Mapping;

namespace CarManagement.UnitTests.Queries;

public class GetLinesByBrandQueryHandlerTests
{
    private readonly Mock<ILineRepository> _lines = new();
    private readonly IMapper _mapper;

    public GetLinesByBrandQueryHandlerTests()
    {
        var cfg = new MapperConfiguration(c => c.AddProfile(new LineProfile()));
        _mapper = cfg.CreateMapper();
    }

    [Fact]
    public async Task Handle_Should_Return_Empty_When_No_Lines()
    {
        var handler = new GetLinesByBrandQueryHandler(_lines.Object, _mapper);
        _lines.Setup(x => x.ListAsync(It.IsAny<ISpecification<Line>>(), It.IsAny<CancellationToken>()))
              .ReturnsAsync(Array.Empty<Line>());

        var result = await handler.Handle(new GetLinesByBrandQuery(Guid.NewGuid()), CancellationToken.None);

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_Should_Map_Lines_With_ModelYears()
    {
        var brandId = Guid.NewGuid();
        var line = new Line
        {
            Id = Guid.NewGuid(),
            BrandId = brandId,
            Name = "Corolla",
            Category = CarCategory.Sedan,
            IsActive = true,
            ModelYears =
            {
                new ModelYear { Id = Guid.NewGuid(), LineId = Guid.NewGuid(), Year = 2020, BasePrice = 20000m, Features = "{}", IsActive = true },
                new ModelYear { Id = Guid.NewGuid(), LineId = Guid.NewGuid(), Year = 2021, BasePrice = 21000m, Features = "{}", IsActive = true }
            }
        };
        var handler = new GetLinesByBrandQueryHandler(_lines.Object, _mapper);
        _lines.Setup(x => x.ListAsync(It.IsAny<ISpecification<Line>>(), It.IsAny<CancellationToken>()))
              .ReturnsAsync(new[] { line });

        var result = await handler.Handle(new GetLinesByBrandQuery(brandId), CancellationToken.None);

        result.Should().HaveCount(1);
        var dto = result[0];
        dto.Id.Should().Be(line.Id);
        dto.BrandId.Should().Be(brandId);
        dto.Name.Should().Be("Corolla");
        dto.Category.Should().Be(CarCategory.Sedan);
        dto.IsActive.Should().BeTrue();
        dto.ModelYears.Should().HaveCount(2);
        dto.ModelYears.Select(x => x.Year).Should().Contain(new[] { 2020, 2021 });
    }
}
