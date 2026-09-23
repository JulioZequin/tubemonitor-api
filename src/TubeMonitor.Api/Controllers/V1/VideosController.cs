using System.ComponentModel.DataAnnotations;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using TubeMonitor.Api.DTOs;
using TubeMonitor.Api.Services;

namespace TubeMonitor.Api.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/videos")]
[Produces("application/json")]
public class VideosController(IVideoService videoService) : ControllerBase
{
    /// <summary>Lista todos os vídeos monitorados (mais recentes primeiro).</summary>
    [HttpGet]
    [ProducesResponseType<IEnumerable<VideoResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<VideoResponse>>> Listar(CancellationToken cancellationToken)
    {
        var videos = await videoService.ListarAsync(cancellationToken);
        return Ok(videos);
    }

    /// <summary>Ranking dos vídeos em alta, ordenado por média de views por dia.</summary>
    [HttpGet("em-alta")]
    [ProducesResponseType<IEnumerable<VideoResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<VideoResponse>>> ListarEmAlta(
        [FromQuery, Range(1, 50, ErrorMessage = "O parâmetro 'top' deve estar entre 1 e 50.")] int top = 10,
        CancellationToken cancellationToken = default)
    {
        var videos = await videoService.ListarEmAltaAsync(top, cancellationToken);
        return Ok(videos);
    }

    /// <summary>Busca um vídeo pelo id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType<VideoResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<VideoResponse>> ObterPorId(int id, CancellationToken cancellationToken)
    {
        var video = await videoService.ObterPorIdAsync(id, cancellationToken);
        return Ok(video);
    }

    /// <summary>Cadastra um vídeo vinculado a um canal.</summary>
    [HttpPost]
    [ProducesResponseType<VideoResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<VideoResponse>> Criar(
        [FromBody] VideoRequest request, CancellationToken cancellationToken)
    {
        var video = await videoService.CriarAsync(request, cancellationToken);
        return CreatedAtAction(nameof(ObterPorId), new { id = video.Id, version = "1" }, video);
    }

    /// <summary>Atualiza os dados e métricas de um vídeo.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Atualizar(
        int id, [FromBody] VideoRequest request, CancellationToken cancellationToken)
    {
        await videoService.AtualizarAsync(id, request, cancellationToken);
        return NoContent();
    }

    /// <summary>Remove um vídeo.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Remover(int id, CancellationToken cancellationToken)
    {
        await videoService.RemoverAsync(id, cancellationToken);
        return NoContent();
    }
}
