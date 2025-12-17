using CommitHelper.Domain.Commit;
using CommitHelper.Domain.Commit.Services;
using CommitHelper.Domain.Exceptions;
using CommitHelper.Domain.MessageGeneration.Services;
using CommitHelper.Domain.Staging.Services;
using CommitHelper.Presentation.View;

namespace CommitHelper.Presentation;

public class CommitController(
    GitDiffService diffService,
    AICommitMessageService aiMessageService,
    OutputView outputView,
    InputView inputView,
    GitCommitService commitService)
{
    private void HandleError(Exception ex)
    {
        switch (ex)
        {
            case GitNotFoundException:
                outputView.WriteLine("\n[ERROR] Git 실행 파일을 찾을 수 없습니다.", ConsoleColor.Red);
                break;
            case AiAuthenticationException authEx:
                outputView.WriteLine($"\n[ERROR] AI 인증 실패: {authEx.Message}", ConsoleColor.Red);
                break;
            case GitCommitException commitEx:
                outputView.WriteLine($"\n[ERROR] 커밋 실행 실패: {commitEx.Message}", ConsoleColor.Red);
                break;
            case ArgumentException argEx when (argEx.ParamName == nameof(CommitMessage)):
                outputView.WriteLine($"\n[ERROR] 메시지 유효성 오류: {argEx.Message}", ConsoleColor.Red);
                break;
            default:
                outputView.WriteLine($"\n[ERROR] 예기치 않은 오류 발생: {ex.Message}", ConsoleColor.Red);
                break;
        }
    }

    public async Task RunAsync(CancellationToken ct = default)
    {
        try
        {
            outputView.WriteLine("🚀 Staged Diff 분석 및 메시지 생성 중...");

            var formattedDiff = await diffService.GetDiffAsAiPromptAsync(ct);

            if (string.IsNullOrWhiteSpace(formattedDiff))
            {
                outputView.WriteLine("[WARN] 커밋할 Staged 변경 사항이 없습니다.", ConsoleColor.Yellow);
                return;
            }

            var generatedMessage = await aiMessageService.GenerateMessageAsync(formattedDiff, ct);
            string currentMessage = generatedMessage.Value;

            while (true)
            {
                DisplayCommitMessage(currentMessage);

                outputView.WriteEmptyLine();
                outputView.WriteLine("👉 동작을 선택하세요 (y: 커밋 실행, n: 종료, m: 메시지 수정):", ConsoleColor.Cyan);

                string choice = inputView.ReadLine().ToLower().Trim();

                if (choice == "y")
                {
                    await ExecuteCommitAsync(currentMessage);
                    outputView.WriteLine("\n🎉 커밋이 성공적으로 완료되었습니다.", ConsoleColor.Green);
                    break;
                }
                else if (choice == "m")
                {
                    outputView.WriteEmptyLine();
                    outputView.WriteLine("✏️ 수정할 메시지를 입력하세요 (입력 후 Enter):", ConsoleColor.Yellow);
                    string edited = inputView.ReadLine().Trim();

                    if (!string.IsNullOrWhiteSpace(edited))
                    {
                        currentMessage = edited;
                    }
                    else
                    {
                        outputView.WriteLine("[ERROR] 메시지는 비어있을 수 없습니다.", ConsoleColor.Red);
                    }
                }
                else if (choice == "n")
                {
                    outputView.WriteLine("👋 커밋을 취소하고 종료합니다.", ConsoleColor.Gray);
                    break;
                }
                else
                {
                    outputView.WriteLine("[ERROR] 잘못된 입력입니다. (y/n/m 중 선택)", ConsoleColor.Red);
                }
            }
        }
        catch (Exception ex)
        {
            HandleError(ex);
        }
    }

    private void DisplayCommitMessage(string message)
    {
        outputView.WriteEmptyLine();
        outputView.WriteLine("✨ 생성된 커밋 메시지:", ConsoleColor.Green);
        outputView.WriteLine("--------------------------------------------------");
        outputView.WriteLine(message);
        outputView.WriteLine("--------------------------------------------------");
    }

    private async Task ExecuteCommitAsync(string message)
    {
        outputView.WriteLine("⚙️ Git 커밋 명령 실행 중...", ConsoleColor.Gray);

        var commitMessageVo = new CommitMessage(message);

        await commitService.CommitAsync(commitMessageVo);
    }
}
