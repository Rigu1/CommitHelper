namespace CommitHelper.Domain.Exceptions;

public class AiAuthenticationException(string message, Exception innerException = null) : AiRepositoryException(message, innerException);
