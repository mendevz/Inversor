namespace Inversor.Core.Domain.Exceptions;
public abstract class InversorException(string message) : Exception(message);
public class ValidationException(string message) : InversorException(message);
public class NotFoundException(string message) : InversorException(message);