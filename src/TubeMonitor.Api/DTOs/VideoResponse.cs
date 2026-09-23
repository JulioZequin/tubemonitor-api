namespace TubeMonitor.Api.DTOs;

public record VideoResponse(
    int Id,
    int CanalId,
    string NomeCanal,
    string YoutubeVideoId,
    string Url,
    string Titulo,
    long Visualizacoes,
    long Curtidas,
    long Comentarios,
    double TaxaEngajamento,
    double ViewsPorDia,
    DateTime PublicadoEm,
    DateTime CadastradoEm,
    DateTime? AtualizadoEm);
