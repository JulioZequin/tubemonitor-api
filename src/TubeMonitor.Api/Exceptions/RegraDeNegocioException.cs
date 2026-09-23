namespace TubeMonitor.Api.Exceptions;

/// <summary>Resultado: 400 Bad Request.</summary>
public class RegraDeNegocioException(string mensagem) : Exception(mensagem);
