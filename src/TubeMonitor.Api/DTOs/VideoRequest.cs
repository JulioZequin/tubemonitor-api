using System.ComponentModel.DataAnnotations;

namespace TubeMonitor.Api.DTOs;

public record VideoRequest(
    [Range(1, int.MaxValue, ErrorMessage = "Informe um canalId válido.")]
    int CanalId,

    [Required(ErrorMessage = "O ID do vídeo no YouTube é obrigatório.")]
    [RegularExpression(@"^[A-Za-z0-9_-]{11}$", ErrorMessage = "O ID do vídeo no YouTube deve ter exatamente 11 caracteres.")]
    string YoutubeVideoId,

    [Required(ErrorMessage = "O título é obrigatório.")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "O título deve ter entre 3 e 200 caracteres.")]
    string Titulo,

    [Range(0, long.MaxValue, ErrorMessage = "As visualizações não podem ser negativas.")]
    long Visualizacoes,

    [Range(0, long.MaxValue, ErrorMessage = "As curtidas não podem ser negativas.")]
    long Curtidas,

    [Range(0, long.MaxValue, ErrorMessage = "Os comentários não podem ser negativos.")]
    long Comentarios,

    [Required(ErrorMessage = "A data de publicação é obrigatória.")]
    DateTime PublicadoEm);
