using TubeMonitor.Api.DTOs;

namespace TubeMonitor.Api.Services;

public interface IVideoService
{
    Task<IReadOnlyList<VideoResponse>> ListarAsync(CancellationToken cancellationToken);
    Task<IReadOnlyList<VideoResponse>> ListarPorCanalAsync(int canalId, CancellationToken cancellationToken);
    Task<IReadOnlyList<VideoResponse>> ListarEmAltaAsync(int quantidade, CancellationToken cancellationToken);
    Task<VideoResponse> ObterPorIdAsync(int id, CancellationToken cancellationToken);
    Task<VideoResponse> CriarAsync(VideoRequest request, CancellationToken cancellationToken);
    Task AtualizarAsync(int id, VideoRequest request, CancellationToken cancellationToken);
    Task RemoverAsync(int id, CancellationToken cancellationToken);
}
