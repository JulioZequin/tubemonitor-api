using Microsoft.EntityFrameworkCore;
using TubeMonitor.Api.Data;
using TubeMonitor.Api.DTOs;
using TubeMonitor.Api.Exceptions;
using TubeMonitor.Api.Mappings;
using TubeMonitor.Api.Models;

namespace TubeMonitor.Api.Services;

public class CanalService(AppDbContext context) : ICanalService
{
    public async Task<IReadOnlyList<CanalResponse>> ListarAsync(string? nicho, CancellationToken cancellationToken)
    {
        var query = context.Canais.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(nicho))
        {
            var nichoFiltro = nicho.Trim().ToLower();
            query = query.Where(c => c.Nicho.ToLower() == nichoFiltro);
        }

        return await query
            .OrderBy(c => c.Nome)
            .Select(CanalMappings.Projecao)
            .ToListAsync(cancellationToken);
    }

    public async Task<CanalResponse> ObterPorIdAsync(int id, CancellationToken cancellationToken)
    {
        var canal = await context.Canais
            .AsNoTracking()
            .Where(c => c.Id == id)
            .Select(CanalMappings.Projecao)
            .FirstOrDefaultAsync(cancellationToken);

        return canal ?? throw CanalNaoEncontrado(id);
    }

    public async Task<CanalResponse> CriarAsync(CanalRequest request, CancellationToken cancellationToken)
    {
        var canal = request.ParaEntidade();

        await GarantirHandleDisponivelAsync(canal.Handle, idIgnorado: null, cancellationToken);

        context.Canais.Add(canal);
        await context.SaveChangesAsync(cancellationToken);

        return canal.ParaResponse(totalVideos: 0);
    }

    public async Task AtualizarAsync(int id, CanalRequest request, CancellationToken cancellationToken)
    {
        var canal = await BuscarEntidadeAsync(id, cancellationToken);

        await GarantirHandleDisponivelAsync(request.Handle.Trim().ToLowerInvariant(), idIgnorado: id, cancellationToken);

        canal.AtualizarCom(request);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoverAsync(int id, CancellationToken cancellationToken)
    {
        var canal = await BuscarEntidadeAsync(id, cancellationToken);

        // Os vídeos do canal são removidos em cascata (configurado no CanalConfiguration)
        context.Canais.Remove(canal);
        await context.SaveChangesAsync(cancellationToken);
    }

    private async Task<Canal> BuscarEntidadeAsync(int id, CancellationToken cancellationToken)
    {
        return await context.Canais.FindAsync([id], cancellationToken)
            ?? throw CanalNaoEncontrado(id);
    }

    private async Task GarantirHandleDisponivelAsync(string handle, int? idIgnorado, CancellationToken cancellationToken)
    {
        var handleEmUso = await context.Canais
            .AnyAsync(c => c.Handle == handle && c.Id != idIgnorado, cancellationToken);

        if (handleEmUso)
            throw new ConflitoException($"Já existe um canal cadastrado com o handle '{handle}'.");
    }

    private static RecursoNaoEncontradoException CanalNaoEncontrado(int id) =>
        new($"Canal com id {id} não foi encontrado.");
}
