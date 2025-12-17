namespace CommitHelper.Domain.MessageGeneration.Constants;

public static class MessageGenerationConstants
{
    public const string ErrorEmptyAiResponse = "AI 응답 메시지는 비어 있을 수 없습니다.";
    public const string ErrorNoDiffContent = "커밋 메시지 생성에 필요한 Git Diff 내용이 없습니다.";
}
