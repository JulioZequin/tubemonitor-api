using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using TubeMonitor.Api.DTOs;
using TubeMonitor.Api.Services;

namespace TubeMonitor.Api.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/canais")]
[Produces("application/json")]
public class CanaisController(ICanalService canalService, IVideoService videoService) : ControllerBase
{
    /// <summary>Lista os canais monitorados, com filtro opcional por nicho.</summary>
    [HttpGet]
    [ProducesResponseType<IEnumerable<CanalResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CanalResponse>>> Listar(
        [FromQuery] string? nicho, CancellationToken cancellationToken)
    {
        var canais = await canalService.ListarAsync(nicho, cancellationToken);
        return Ok(canais);
    }

    /// <summary>Busca um canal pelo id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType<CanalResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CanalResponse>> ObterPorId(int id, CancellationToken cancellationToken)
    {
        var canal = await canalService.ObterPorIdAsync(id, cancellationToken);
        return Ok(canal);
    }

    /// <summary>Lista os vídeos de um canal.</summary>
    [HttpGet("{id:int}/videos")]
    [ProducesResponseType<IEnumerable<VideoResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<VideoResponse>>> ListarVideos(int id, CancellationToken cancellationToken)
    {
        var videos = await videoService.ListarPorCanalAsync(id, cancellationToken);
        return Ok(videos);
    }

    /// <summary>Cadastra um novo canal para monitoramento.</summary>
    [HttpPost]
    [ProducesResponseType<CanalResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<CanalResponse>> Criar(
        [FromBody] CanalRequest request, CancellationToken cancellationToken)
    {
        var canal = await canalService.CriarAsync(request, cancellationToken);
        return CreatedAtAction(nameof(ObterPorId), new { id = canal.Id, version = "1" }, canal);
    }

    /// <summary>Atualiza os dados de um canal.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Atualizar(
        int id, [FromBody] CanalRequest request, CancellationToken cancellationToken)
    {
        await canalService.AtualizarAsync(id, request, cancellationToken);
        return NoContent();
    }

    /// <summary>Remove um canal e todos os seus vídeos.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Remover(int id, CancellationToken cancellationToken)
    {
        await canalService.RemoverAsync(id, cancellationToken);
        return NoContent();
    }
}
