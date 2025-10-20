using Asp.Versioning;
using CarManagement.Application.Brands.Commands;
using CarManagement.Application.Brands.Dtos;
using CarManagement.Application.Brands.Queries;
using CarManagement.Application.Common.Pagination;
using CarManagement.Application.Lines.Dtos;
using CarManagement.Application.Lines.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CarManagement.API.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/brands")]
public class BrandsController : ControllerBase
{
    private readonly ISender _mediator;

    public BrandsController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{brandId:guid}/lines")]
    [ProducesResponseType(typeof(IReadOnlyList<LineDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLinesByBrand([FromRoute] Guid brandId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetLinesByBrandQuery(brandId), ct);
        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<BrandDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? search = null, [FromQuery] string? country = null, [FromQuery] string? sortBy = null, [FromQuery] bool desc = false, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetBrandsQuery(page, pageSize, search, country, sortBy, desc), ct);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(BrandDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetBrandByIdQuery(id), ct);
        if (result is null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(BrandDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateBrandRequest request, CancellationToken ct)
    {
        var created = await _mediator.Send(new CreateBrandCommand(request.Name, request.Country, request.FoundedYear), ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id, version = HttpContext.GetRequestedApiVersion()!.ToString() }, created);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(BrandDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateBrandRequest request, CancellationToken ct)
    {
        var updated = await _mediator.Send(new UpdateBrandCommand(id, request.Name, request.Country, request.FoundedYear, request.IsActive), ct);
        if (updated is null) return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken ct)
    {
        try
        {
            var ok = await _mediator.Send(new DeleteBrandCommand(id), ct);
            if (!ok) return NotFound();
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new ProblemDetails { Title = "Business rule violation", Detail = ex.Message, Status = StatusCodes.Status409Conflict });
        }
    }
}
