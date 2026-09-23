namespace TubeMonitor.Api.Exceptions;

/// <summary>Resultado: 404 Not Found.</summary>
public class RecursoNaoEncontradoException(string mensagem) : Exception(mensagem);
