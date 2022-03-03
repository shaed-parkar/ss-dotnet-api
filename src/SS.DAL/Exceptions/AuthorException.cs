namespace SS.Exceptions;

public class AuthorDalException : Exception
{
    protected AuthorDalException(string message) : base(message)
    {
    }

    protected AuthorDalException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}

[Serializable]
public class AuthorNotFoundException : AuthorDalException
{
    public AuthorNotFoundException(Guid authorId) : base(
        $"Author {authorId} does not exist")
    {
        AuthorId = authorId;
    }

    protected AuthorNotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    public Guid AuthorId { get; }
}