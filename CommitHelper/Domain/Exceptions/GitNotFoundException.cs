namespace CommitHelper.Domain.Exceptions;

public class GitNotFoundException() : Exception("Git이 설치되어 있지 않거나 경로를 찾을 수 없습니다.");
