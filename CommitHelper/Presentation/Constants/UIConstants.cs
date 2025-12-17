namespace CommitHelper.Presentation.Constants;

public static class UIConstants
{
    public static class Message
    {
        public const string AnalyzingDiff = "🚀 Staged Diff 분석 및 메시지 생성 중...";
        public const string NoStagedChanges = "[WARN] 커밋할 Staged 변경 사항이 없습니다.";
        public const string SelectAction = "👉 동작을 선택하세요 (y: 커밋 실행, n: 종료, m: 메시지 수정):";
        public const string InputModifyMessage = "✏️ 수정할 메시지를 입력하세요 (입력 후 Enter):";
        public const string EmptyMessageError = "[ERROR] 메시지는 비어있을 수 없습니다.";
        public const string CommitCancelled = "👋 커밋을 취소하고 종료합니다.";
        public const string InvalidInput = "[ERROR] 잘못된 입력입니다. (y/n/m 중 선택)";
        public const string GeneratedHeader = "✨ 생성된 커밋 메시지:";
        public const string CommitSuccess = "\n🎉 커밋이 성공적으로 완료되었습니다.";
        public const string Committing = "⚙️ Git 커밋 명령 실행 중...";
        public const string Divider = "--------------------------------------------------";
    }

    public static class Error
    {
        public const string GitNotFound = "\n[ERROR] Git 실행 파일을 찾을 수 없습니다.";
        public const string AiAuthFailed = "\n[ERROR] AI 인증 실패: ";
        public const string CommitFailed = "\n[ERROR] 커밋 실행 실패: ";
        public const string MessageValidationError = "\n[ERROR] 메시지 유효성 오류: ";
        public const string UnexpectedError = "\n[ERROR] 예기치 않은 오류 발생: ";
    }

    public static class Command
    {
        public const string Yes = "y";
        public const string No = "n";
        public const string Modify = "m";
    }
}
