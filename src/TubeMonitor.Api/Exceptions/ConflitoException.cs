namespace TubeMonitor.Api.Exceptions;

/// <summary>Resultado: 409 Conflict (ex.: registro duplicado).</summary>
public class ConflitoException(string mensagem) : Exception(mensagem);
