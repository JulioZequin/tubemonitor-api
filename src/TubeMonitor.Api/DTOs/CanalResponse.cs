namespace TubeMonitor.Api.DTOs;

public record CanalResponse(
    int Id,
    string Nome,
    string Handle,
    string Nicho,
    long Inscritos,
    int TotalVideos,
    DateTime CadastradoEm,
    DateTime? AtualizadoEm);
