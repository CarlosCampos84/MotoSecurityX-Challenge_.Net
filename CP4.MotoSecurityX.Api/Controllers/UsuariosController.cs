using CP4.MotoSecurityX.Application.Common;
using CP4.MotoSecurityX.Application.DTOs;
using CP4.MotoSecurityX.Application.UseCases.Usuarios;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using CP4.MotoSecurityX.Api.SwaggerExamples;
using CP4.MotoSecurityX.Api.Security;

namespace CP4.MotoSecurityX.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/usuarios")]
[ApiKeyAuth]
public class UsuariosController : ControllerBase
{
    private string Link(int page, int size) =>
        Url.ActionLink(nameof(List), values: new { version = "1.0", page, pageSize = size }) ?? string.Empty;

    [HttpGet]
    [SwaggerOperation(Summary = "Lista usuários", Description = "Retorna lista paginada de usuários com HATEOAS")]
    [ProducesResponseType(typeof(PagedResult<UsuarioDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromServices] ListUsuariosHandler handler = null!,
        CancellationToken ct = default)
    {
        var result = await handler.HandleAsync(page, pageSize, Link, ct);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [SwaggerOperation(Summary = "Busca usuário por Id")]
    [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        Guid id,
        [FromServices] GetUsuarioByIdHandler handler,
        CancellationToken ct)
    {
        var dto = await handler.HandleAsync(id, ct);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Cria um usuário", Description = "Retorna 201 com Location para GET /api/usuarios/{id}")]
    [SwaggerRequestExample(typeof(CreateUsuarioDto), typeof(CreateUsuarioDtoExample))]
    [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateUsuarioDto dto,
        [FromServices] CreateUsuarioHandler handler,
        CancellationToken ct)
    {
        var created = await handler.HandleAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { version = "1.0", id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    [SwaggerOperation(Summary = "Atualiza um usuário")]
    [SwaggerRequestExample(typeof(UpdateUsuarioDto), typeof(UpdateUsuarioDtoExample))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateUsuarioDto dto,
        [FromServices] UpdateUsuarioHandler handler,
        CancellationToken ct)
    {
        var ok = await handler.HandleAsync(id, dto, ct);
        return ok ? NoContent() : NotFound();
    }

    [HttpDelete("{id:guid}")]
    [SwaggerOperation(Summary = "Exclui um usuário")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        Guid id,
        [FromServices] DeleteUsuarioHandler handler,
        CancellationToken ct)
    {
        var ok = await handler.HandleAsync(id, ct);
        return ok ? NoContent() : NotFound();
    }
}


