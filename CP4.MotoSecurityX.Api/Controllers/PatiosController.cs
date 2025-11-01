using CP4.MotoSecurityX.Application.Common;
using CP4.MotoSecurityX.Application.DTOs;
using CP4.MotoSecurityX.Application.UseCases.Patios;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using CP4.MotoSecurityX.Api.SwaggerExamples;

namespace CP4.MotoSecurityX.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/patios")]
public class PatiosController : ControllerBase
{
    private string Link(int page, int size) =>
        Url.ActionLink(nameof(List), values: new { version = "1.0", page, pageSize = size }) ?? string.Empty;

    [HttpGet]
    [SwaggerOperation(Summary = "Lista pátios", Description = "Retorna lista paginada de pátios com HATEOAS")]
    [ProducesResponseType(typeof(PagedResult<PatioDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromServices] ListPatiosHandler handler = null!,
        CancellationToken ct = default)
    {
        var result = await handler.HandleAsync(page, pageSize, Link, ct);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [SwaggerOperation(Summary = "Busca pátio por Id")]
    [ProducesResponseType(typeof(PatioDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        Guid id,
        [FromServices] GetPatioByIdHandler handler,
        CancellationToken ct)
    {
        var dto = await handler.HandleAsync(id, ct);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Cria um pátio", Description = "Retorna 201 com Location para GET /api/patios/{id}")]
    [SwaggerRequestExample(typeof(CreatePatioDto), typeof(CreatePatioDtoExample))]
    [ProducesResponseType(typeof(PatioDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreatePatioDto dto,
        [FromServices] CreatePatioHandler handler,
        CancellationToken ct)
    {
        var created = await handler.HandleAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { version = "1.0", id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    [SwaggerOperation(Summary = "Atualiza um pátio")]
    [SwaggerRequestExample(typeof(UpdatePatioDto), typeof(UpdatePatioDtoExample))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdatePatioDto dto,
        [FromServices] UpdatePatioHandler handler,
        CancellationToken ct)
    {
        var ok = await handler.HandleAsync(id, dto, ct);
        return ok ? NoContent() : NotFound();
    }

    [HttpDelete("{id:guid}")]
    [SwaggerOperation(Summary = "Exclui um pátio")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        Guid id,
        [FromServices] DeletePatioHandler handler,
        CancellationToken ct)
    {
        var ok = await handler.HandleAsync(id, ct);
        return ok ? NoContent() : NotFound();
    }
}



