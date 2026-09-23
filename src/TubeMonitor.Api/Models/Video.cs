namespace TubeMonitor.Api.Models;

public class Video
{
    public int Id { get; set; }
    public string YoutubeVideoId { get; set; } = string.Empty;
    public string Titulo { get; set; } = string.Empty;
    public long Visualizacoes { get; set; }
    public long Curtidas { get; set; }
    public long Comentarios { get; set; }
    public DateTime PublicadoEm { get; set; }
    public DateTime CadastradoEm { get; set; }
    public DateTime? AtualizadoEm { get; set; }

    public int CanalId { get; set; }
    public Canal Canal { get; set; } = null!;
}
