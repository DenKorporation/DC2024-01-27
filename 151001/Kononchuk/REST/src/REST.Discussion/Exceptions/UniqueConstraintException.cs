namespace REST.Discussion.Exceptions;

public class UniqueConstraintException(string message = "The uniqueness constraint is not met", int code = 403) : Exception(message)
{
    public int Code { get; } = code;
}