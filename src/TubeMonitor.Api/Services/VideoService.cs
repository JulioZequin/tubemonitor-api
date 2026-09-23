using Microsoft.EntityFrameworkCore;
using TubeMonitor.Api.Data;
using TubeMonitor.Api.DTOs;
using TubeMonitor.Api.Exceptions;
using TubeMonitor.Api.Mappings;
using TubeMonitor.Api.Models;

namespace TubeMonitor.Api.Services;

public class VideoService(AppDbContext context) : IVideoService
{
    public async Task<IReadOnlyList<VideoResponse>> ListarAsync(CancellationToken cancellationToken)
    {
        var videos = await QueryComCanal()
            .OrderByDescending(v => v.PublicadoEm)
            .ToListAsync(cancellationToken);

        return videos.Select(v => v.ParaResponse()).ToList();
    }

    public async Task<IReadOnlyList<VideoResponse>> ListarPorCanalAsync(int canalId, CancellationToken cancellationToken)
    {
        var canalExiste = await context.Canais.AnyAsync(c => c.Id == canalId, cancellationToken);
        if (!canalExiste)
            throw new RecursoNaoEncontradoException($"Canal com id {canalId} não foi encontrado.");

        var videos = await QueryComCanal()
            .Where(v => v.CanalId == canalId)
            .OrderByDescending(v => v.PublicadoEm)
            .ToListAsync(cancellationToken);

        return videos.Select(v => v.ParaResponse()).ToList();
    }

    public async Task<IReadOnlyList<VideoResponse>> ListarEmAltaAsync(int quantidade, CancellationToken cancellationToken)
    {
        var videos = await QueryComCanal().ToListAsync(cancellationToken);

        // O ranking usa "views por dia", calculado em memória porque o SQLite
        // não traduz operações com datas de forma confiável.
        return videos
            .Select(v => v.ParaResponse())
            .OrderByDescending(v => v.ViewsPorDia)
            .Take(quantidade)
            .ToList();
    }

    public async Task<VideoResponse> ObterPorIdAsync(int id, CancellationToken cancellationToken)
    {
        var video = await QueryComCanal().FirstOrDefaultAsync(v => v.Id == id, cancellationToken)
            ?? throw VideoNaoEncontrado(id);

        return video.ParaResponse();
    }

    public async Task<VideoResponse> CriarAsync(VideoRequest request, CancellationToken cancellationToken)
    {
        ValidarRegrasDeNegocio(request);
        var canal = await BuscarCanalInformadoAsync(request.CanalId, cancellationToken);
        await GarantirYoutubeIdDisponivelAsync(request.YoutubeVideoId.Trim(), idIgnorado: null, cancellationToken);

        var video = request.ParaEntidade();
        video.Canal = canal;

        context.Videos.Add(video);
        await context.SaveChangesAsync(cancellationToken);

        return video.ParaResponse();
    }

    public async Task AtualizarAsync(int id, VideoRequest request, CancellationToken cancellationToken)
    {
        var video = await context.Videos.FindAsync([id], cancellationToken)
            ?? throw VideoNaoEncontrado(id);

        ValidarRegrasDeNegocio(request);
        await BuscarCanalInformadoAsync(request.CanalId, cancellationToken);
        await GarantirYoutubeIdDisponivelAsync(request.YoutubeVideoId.Trim(), idIgnorado: id, cancellationToken);

        video.AtualizarCom(request);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoverAsync(int id, CancellationToken cancellationToken)
    {
        var video = await context.Videos.FindAsync([id], cancellationToken)
            ?? throw VideoNaoEncontrado(id);

        context.Videos.Remove(video);
        await context.SaveChangesAsync(cancellationToken);
    }

    private IQueryable<Video> QueryComCanal() =>
        context.Videos.AsNoTracking().Include(v => v.Canal);

    private static void ValidarRegrasDeNegocio(VideoRequest request)
    {
        if (request.PublicadoEm.ToUniversalTime() > DateTime.UtcNow)
            throw new RegraDeNegocioException("A data de publicação não pode estar no futuro.");

        if (request.Curtidas > request.Visualizacoes)
            throw new RegraDeNegocioException("O número de curtidas não pode ser maior que o de visualizações.");
    }

    private async Task<Canal> BuscarCanalInformadoAsync(int canalId, CancellationToken cancellationToken)
    {
        // O canal vem no corpo da requisição: se não existir, o erro é do cliente (400)
        return await context.Canais.FindAsync([canalId], cancellationToken)
            ?? throw new RegraDeNegocioException($"O canal informado (id {canalId}) não existe.");
    }

    private async Task GarantirYoutubeIdDisponivelAsync(string youtubeVideoId, int? idIgnorado, CancellationToken cancellationToken)
    {
        var jaCadastrado = await context.Videos
            .AnyAsync(v => v.YoutubeVideoId == youtubeVideoId && v.Id != idIgnorado, cancellationToken);

        if (jaCadastrado)
            throw new ConflitoException($"O vídeo '{youtubeVideoId}' já está cadastrado.");
    }

    private static RecursoNaoEncontradoException VideoNaoEncontrado(int id) =>
        new($"Vídeo com id {id} não foi encontrado.");
}
