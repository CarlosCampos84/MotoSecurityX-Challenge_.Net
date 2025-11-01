using CP4.MotoSecurityX.Application.DTOs;
using CP4.MotoSecurityX.Domain.Entities;
using CP4.MotoSecurityX.Domain.Repositories;
using CP4.MotoSecurityX.Domain.ValueObjects;

namespace CP4.MotoSecurityX.Application.UseCases.Motos;

public sealed class CreateMotoHandler
{
    private readonly IMotoRepository _repo;
    public CreateMotoHandler(IMotoRepository repo) => _repo = repo;

    public async Task<MotoDto> HandleAsync(CreateMotoDto input, CancellationToken ct = default)
    {
        var placa = Placa.Create(input.Placa);
        var moto  = new Moto(placa, input.Modelo);
        await _repo.AddAsync(moto, ct);

        return new MotoDto
        {
            Id            = moto.Id,
            Placa         = moto.Placa.Value,
            Modelo        = moto.Modelo,
            DentroDoPatio = moto.DentroDoPatio,
            PatioId       = moto.PatioId
        };

    }
}



