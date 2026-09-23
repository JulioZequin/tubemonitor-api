using TubeMonitor.Api.DTOs;

namespace TubeMonitor.Api.Services;

public interface ICanalService
{
    Task<IReadOnlyList<CanalResponse>> ListarAsync(string? nicho, CancellationToken cancellationToken);
    Task<CanalResponse> ObterPorIdAsync(int id, CancellationToken cancellationToken);
    Task<CanalResponse> CriarAsync(CanalRequest request, CancellationToken cancellationToken);
    Task AtualizarAsync(int id, CanalRequest request, CancellationToken cancellationToken);
    Task RemoverAsync(int id, CancellationToken cancellationToken);
}
