namespace CommitHelper.Domain.Exceptions;

public class AiNetworkException(string message, Exception innerException = null)
    : AiRepositoryException(message, innerException);
