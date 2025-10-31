using CP4.MotoSecurityX.Application.Common;
using CP4.MotoSecurityX.Application.DTOs;
using CP4.MotoSecurityX.Application.UseCases.Motos;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using CP4.MotoSecurityX.Api.SwaggerExamples;

namespace CP4.MotoSecurityX.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/motos")]
public class MotosController : ControllerBase
{
    private string Link(int page, int size) =>
        Url.ActionLink(nameof(List), values: new { page, pageSize = size }) ?? string.Empty;

    // LIST paginada + HATEOAS
    [HttpGet]
    [SwaggerOperation(Summary = "Lista motos", Description = "Retorna lista paginada de motos com HATEOAS")]
    [ProducesResponseType(typeof(PagedResult<MotoDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromServices] ListMotosHandler handler = null!,
        CancellationToken ct = default)
    {
        var result = await handler.HandleAsync(page, pageSize, Link, ct);
        return Ok(result);
    }

    // GET BY ID
    [HttpGet("{id:guid}")]
    [SwaggerOperation(Summary = "Busca moto por Id")]
    [ProducesResponseType(typeof(MotoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        Guid id,
        [FromServices] GetMotoByIdHandler handler,
        CancellationToken ct)
    {
        var dto = await handler.HandleAsync(id, ct);
        return dto is null ? NotFound() : Ok(dto);
    }

    // CREATE
    [HttpPost]
    [SwaggerOperation(Summary = "Cria uma moto", Description = "Retorna 201 com Location para GET /api/motos/{id}")]
    [SwaggerRequestExample(typeof(CreateMotoDto), typeof(CreateMotoDtoExample))]
    [ProducesResponseType(typeof(MotoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateMotoDto dto,
        [FromServices] CreateMotoHandler handler,
        CancellationToken ct)
    {
        var created = await handler.HandleAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // MOVER MOTO PARA PÁTIO
    [HttpPost("{id:guid}/mover")]
    [SwaggerOperation(Summary = "Move moto para um pátio", Description = "Associa a moto a um pátio válido")]
    [SwaggerRequestExample(typeof(MoveMotoDto), typeof(MoveMotoDtoExample))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Move(
        Guid id,
        [FromBody] MoveMotoDto dto,
        [FromServices] MoveMotoToPatioHandler handler,
        CancellationToken ct)
    {
        var ok = await handler.HandleAsync(id, dto.PatioId, ct);
        return ok ? NoContent() : NotFound();
    }

    // UPDATE
    [HttpPut("{id:guid}")]
    [SwaggerOperation(Summary = "Atualiza uma moto")]
    [SwaggerRequestExample(typeof(UpdateMotoDto), typeof(UpdateMotoDtoExample))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateMotoDto dto,
        [FromServices] UpdateMotoHandler handler,
        CancellationToken ct)
    {
        var ok = await handler.HandleAsync(id, dto, ct);
        return ok ? NoContent() : NotFound();
    }

    // DELETE
    [HttpDelete("{id:guid}")]
    [SwaggerOperation(Summary = "Exclui uma moto")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        Guid id,
        [FromServices] DeleteMotoHandler handler,
        CancellationToken ct)
    {
        var ok = await handler.HandleAsync(id, ct);
        return ok ? NoContent() : NotFound();
    }
}

