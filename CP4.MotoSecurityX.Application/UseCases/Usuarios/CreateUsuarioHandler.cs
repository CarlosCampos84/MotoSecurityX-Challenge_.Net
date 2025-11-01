using CP4.MotoSecurityX.Application.DTOs;
using CP4.MotoSecurityX.Domain.Entities;
using CP4.MotoSecurityX.Domain.Repositories;

namespace CP4.MotoSecurityX.Application.UseCases.Usuarios;

public class CreateUsuarioHandler
{
    private readonly IUsuarioRepository _repo;
    public CreateUsuarioHandler(IUsuarioRepository repo) => _repo = repo;

    public async Task<UsuarioDto> HandleAsync(CreateUsuarioDto dto, CancellationToken ct = default)
    {
        var usuario = new Usuario(dto.Nome, dto.Email);
        await _repo.AddAsync(usuario, ct);

        return new UsuarioDto
        {
            Id    = usuario.Id,
            Nome  = usuario.Nome,
            Email = usuario.Email
        };
    }
}