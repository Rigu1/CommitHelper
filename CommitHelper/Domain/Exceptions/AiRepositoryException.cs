namespace CommitHelper.Domain.Exceptions;

public class AiRepositoryException : Exception
{
    public AiRepositoryException(string message, Exception innerException) : base(message, innerException) { }
    public AiRepositoryException(string message) : base(message) { }
}
