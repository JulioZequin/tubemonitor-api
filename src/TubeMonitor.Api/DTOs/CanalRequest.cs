using System.ComponentModel.DataAnnotations;

namespace TubeMonitor.Api.DTOs;

public record CanalRequest(
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "O nome deve ter entre 2 e 100 caracteres.")]
    string Nome,

    [Required(ErrorMessage = "O handle é obrigatório.")]
    [RegularExpression(@"^@[A-Za-z0-9._-]{3,49}$", ErrorMessage = "O handle deve começar com @ e ter de 3 a 49 caracteres (letras, números, ponto, hífen ou underline).")]
    string Handle,

    [Required(ErrorMessage = "O nicho é obrigatório.")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "O nicho deve ter entre 2 e 50 caracteres.")]
    string Nicho,

    [Range(0, long.MaxValue, ErrorMessage = "O número de inscritos não pode ser negativo.")]
    long Inscritos);
