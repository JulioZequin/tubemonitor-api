using System.Linq.Expressions;
using TubeMonitor.Api.DTOs;
using TubeMonitor.Api.Models;

namespace TubeMonitor.Api.Mappings;

public static class CanalMappings
{
    /// <summary>Projeção traduzida para SQL pelo EF Core (inclui a contagem de vídeos).</summary>
    public static readonly Expression<Func<Canal, CanalResponse>> Projecao = canal => new CanalResponse(
        canal.Id,
        canal.Nome,
        canal.Handle,
        canal.Nicho,
        canal.Inscritos,
        canal.Videos.Count,
        canal.CadastradoEm,
        canal.AtualizadoEm);

    public static Canal ParaEntidade(this CanalRequest request) => new()
    {
        Nome = request.Nome.Trim(),
        Handle = request.Handle.Trim().ToLowerInvariant(),
        Nicho = request.Nicho.Trim(),
        Inscritos = request.Inscritos,
        CadastradoEm = DateTime.UtcNow
    };

    public static void AtualizarCom(this Canal canal, CanalRequest request)
    {
        canal.Nome = request.Nome.Trim();
        canal.Handle = request.Handle.Trim().ToLowerInvariant();
        canal.Nicho = request.Nicho.Trim();
        canal.Inscritos = request.Inscritos;
        canal.AtualizadoEm = DateTime.UtcNow;
    }

    public static CanalResponse ParaResponse(this Canal canal, int totalVideos) => new(
        canal.Id,
        canal.Nome,
        canal.Handle,
        canal.Nicho,
        canal.Inscritos,
        totalVideos,
        canal.CadastradoEm,
        canal.AtualizadoEm);
}
