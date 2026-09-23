using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace TubeMonitor.Api.Exceptions;

/// <summary>
/// Converte exceções em respostas HTTP padronizadas (ProblemDetails).
/// Assim os controllers ficam limpos, sem try/catch repetido.
/// </summary>
public sealed class GlobalExceptionHandler(
    IProblemDetailsService problemDetailsService,
    ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var (statusCode, titulo) = exception switch
        {
            RecursoNaoEncontradoException => (StatusCodes.Status404NotFound, "Recurso não encontrado"),
            RegraDeNegocioException => (StatusCodes.Status400BadRequest, "Requisição inválida"),
            ConflitoException => (StatusCodes.Status409Conflict, "Conflito"),
            _ => (StatusCodes.Status500InternalServerError, "Erro interno do servidor")
        };

        if (statusCode == StatusCodes.Status500InternalServerError)
            logger.LogError(exception, "Erro não tratado ao processar {Metodo} {Caminho}", httpContext.Request.Method, httpContext.Request.Path);
        else
            logger.LogWarning("{Titulo}: {Mensagem}", titulo, exception.Message);

        httpContext.Response.StatusCode = statusCode;

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = titulo,
                // Não expõe detalhes internos em erros 500
                Detail = statusCode == StatusCodes.Status500InternalServerError
                    ? "Ocorreu um erro inesperado. Tente novamente mais tarde."
                    : exception.Message,
                Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}"
            }
        });
    }
}
