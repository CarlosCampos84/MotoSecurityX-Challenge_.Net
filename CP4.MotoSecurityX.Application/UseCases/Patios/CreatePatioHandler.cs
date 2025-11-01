using CP4.MotoSecurityX.Application.DTOs;
using CP4.MotoSecurityX.Domain.Entities;
using CP4.MotoSecurityX.Domain.Repositories;

namespace CP4.MotoSecurityX.Application.UseCases.Patios;

public sealed class CreatePatioHandler
{
    private readonly IPatioRepository _repo;
    public CreatePatioHandler(IPatioRepository repo) => _repo = repo;

    public async Task<PatioDto> HandleAsync(CreatePatioDto dto, CancellationToken ct = default)
    {
        var patio = new Patio(dto.Nome, dto.Endereco);
        await _repo.AddAsync(patio, ct);

        return new PatioDto
        {
            Id       = patio.Id,
            Nome     = patio.Nome,
            Endereco = patio.Endereco
        };
    }
}