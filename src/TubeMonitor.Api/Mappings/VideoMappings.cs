using TubeMonitor.Api.DTOs;
using TubeMonitor.Api.Models;

namespace TubeMonitor.Api.Mappings;

public static class VideoMappings
{
    public static Video ParaEntidade(this VideoRequest request) => new()
    {
        CanalId = request.CanalId,
        YoutubeVideoId = request.YoutubeVideoId.Trim(),
        Titulo = request.Titulo.Trim(),
        Visualizacoes = request.Visualizacoes,
        Curtidas = request.Curtidas,
        Comentarios = request.Comentarios,
        PublicadoEm = request.PublicadoEm.ToUniversalTime(),
        CadastradoEm = DateTime.UtcNow
    };

    public static void AtualizarCom(this Video video, VideoRequest request)
    {
        video.CanalId = request.CanalId;
        video.YoutubeVideoId = request.YoutubeVideoId.Trim();
        video.Titulo = request.Titulo.Trim();
        video.Visualizacoes = request.Visualizacoes;
        video.Curtidas = request.Curtidas;
        video.Comentarios = request.Comentarios;
        video.PublicadoEm = request.PublicadoEm.ToUniversalTime();
        video.AtualizadoEm = DateTime.UtcNow;
    }

    /// <summary>Requer que a navegação Canal esteja carregada.</summary>
    public static VideoResponse ParaResponse(this Video video) => new(
        video.Id,
        video.CanalId,
        video.Canal.Nome,
        video.YoutubeVideoId,
        $"https://www.youtube.com/watch?v={video.YoutubeVideoId}",
        video.Titulo,
        video.Visualizacoes,
        video.Curtidas,
        video.Comentarios,
        CalcularTaxaEngajamento(video),
        CalcularViewsPorDia(video),
        video.PublicadoEm,
        video.CadastradoEm,
        video.AtualizadoEm);

    /// <summary>(curtidas + comentários) / visualizações, em %.</summary>
    private static double CalcularTaxaEngajamento(Video video)
    {
        if (video.Visualizacoes == 0) return 0;

        var interacoes = video.Curtidas + video.Comentarios;
        return Math.Round(interacoes * 100.0 / video.Visualizacoes, 2);
    }

    /// <summary>Média de visualizações por dia desde a publicação (mínimo de 1 dia).</summary>
    private static double CalcularViewsPorDia(Video video)
    {
        var diasPublicado = Math.Max(1, (DateTime.UtcNow - video.PublicadoEm).TotalDays);
        return Math.Round(video.Visualizacoes / diasPublicado, 1);
    }
}
