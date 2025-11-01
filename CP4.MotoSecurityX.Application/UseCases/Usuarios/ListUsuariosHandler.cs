using System.Linq;
using CP4.MotoSecurityX.Application.Common;
using CP4.MotoSecurityX.Application.DTOs;
using CP4.MotoSecurityX.Domain.Repositories;

namespace CP4.MotoSecurityX.Application.UseCases.Usuarios;

public sealed class ListUsuariosHandler
{
    private readonly IUsuarioRepository _repo;

    public ListUsuariosHandler(IUsuarioRepository repo) => _repo = repo;

    public async Task<PagedResult<UsuarioDto>> HandleAsync(
        int page,
        int pageSize,
        Func<int, int, string> linkFactory,
        CancellationToken ct = default)
    {
        var total     = await _repo.CountAsync(ct);
        var usuarios  = await _repo.ListAsync(page, pageSize, ct);

        var items = usuarios.Select(u => new UsuarioDto
        {
            Id    = u.Id,
            Nome  = u.Nome,
            Email = u.Email
        });

        return PagedResult<UsuarioDto>.Create(items, total, page, pageSize, linkFactory);
    }
}



