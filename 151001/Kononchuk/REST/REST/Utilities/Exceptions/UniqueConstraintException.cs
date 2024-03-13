namespace REST.Utilities.Exceptions;

public class UniqueConstraintException(string message = "The uniqueness constraint is not met", int code = 409) : Exception(message)
{
    public int Code { get; } = code;
}