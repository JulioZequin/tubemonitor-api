namespace TubeMonitor.Api.Models;

public class Canal
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Handle { get; set; } = string.Empty;
    public string Nicho { get; set; } = string.Empty;
    public long Inscritos { get; set; }
    public DateTime CadastradoEm { get; set; }
    public DateTime? AtualizadoEm { get; set; }

    public ICollection<Video> Videos { get; set; } = new List<Video>();
}
