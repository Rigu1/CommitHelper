using CommitHelper.Domain.Commit;
using CommitHelper.Domain.Commit.Services;
using CommitHelper.Domain.Exceptions;
using CommitHelper.Domain.MessageGeneration.Services;
using CommitHelper.Domain.Staging.Services;
using CommitHelper.Presentation.Constants;
using CommitHelper.Presentation.UI;

namespace CommitHelper.Presentation;

public class CommitController(
    OutputView outputView,
    InputView inputView,
    GitDiffService diffService,
    AICommitMessageService aiMessageService,
    GitCommitService commitService)
{
    public async Task RunAsync(CancellationToken ct = default)
    {
        try
        {
            var diff = await FetchDiffContentAsync(ct);
            if (diff == null) return;

            var aiMessage = await GenerateAiMessageAsync(diff, ct);
            await ProcessUserDecisionLoopAsync(aiMessage, ct);
        }
        catch (Exception ex)
        {
            HandleError(ex);
        }
    }

    private async Task<string?> FetchDiffContentAsync(CancellationToken ct)
    {
        outputView.WriteLine(UIConstants.Message.AnalyzingDiff);
        var diff = await diffService.GetDiffAsAiPromptAsync(ct);

        if (!string.IsNullOrWhiteSpace(diff)) return diff;

        outputView.WriteLine(UIConstants.Message.NoStagedChanges, ConsoleColor.Yellow);
        return null;
    }

    private async Task<string> GenerateAiMessageAsync(string diff, CancellationToken ct)
    {
        var response = await aiMessageService.GenerateMessageAsync(diff, ct);
        return response.Value;
    }

    private async Task ProcessUserDecisionLoopAsync(string initialMessage, CancellationToken ct)
    {
        var isResolved = false;

        while (!isResolved)
        {
            DisplayPreview(initialMessage);
            var choice = GetUserChoice();
            isResolved = await HandleUserCommandAsync(choice, initialMessage, ct);
        }
    }

    private string GetUserChoice()
    {
        outputView.WriteEmptyLine();
        outputView.WriteLine(UIConstants.Message.SelectAction, ConsoleColor.Cyan);
        return inputView.ReadLine().ToLower().Trim();
    }

    private async Task<bool> HandleUserCommandAsync(string choice, string message, CancellationToken ct)
    {
        return choice switch
        {
            UIConstants.Command.Yes => await ExecuteCommitAndFinalizeAsync(message),
            UIConstants.Command.No => CancelAndFinalize(),
            UIConstants.Command.Modify => ModifyMessage(ref message),
            _ => ShowInvalidAndContinue()
        };
    }

    private async Task<bool> ExecuteCommitAndFinalizeAsync(string message)
    {
        outputView.WriteLine(UIConstants.Message.Committing, ConsoleColor.Gray);
        await commitService.CommitAsync(new CommitMessage(message));
        outputView.WriteLine(UIConstants.Message.CommitSuccess, ConsoleColor.Green);
        return true; // 루프 종료
    }

    private bool CancelAndFinalize()
    {
        outputView.WriteLine(UIConstants.Message.CommitCancelled, ConsoleColor.Gray);
        return true;
    }

    private bool ModifyMessage(ref string message)
    {
        outputView.WriteEmptyLine();
        outputView.WriteLine(UIConstants.Message.InputModifyMessage, ConsoleColor.Yellow);

        var edited = inputView.ReadLine().Trim();
        if (!string.IsNullOrWhiteSpace(edited))
        {
            message = edited;
        }
        else
        {
            outputView.WriteLine(UIConstants.Message.EmptyMessageError, ConsoleColor.Red);
        }

        return false;
    }

    private bool ShowInvalidAndContinue()
    {
        outputView.WriteLine(UIConstants.Message.InvalidInput, ConsoleColor.Red);
        return false;
    }

    private void DisplayPreview(string message)
    {
        outputView.WriteEmptyLine();
        outputView.WriteLine(UIConstants.Message.GeneratedHeader, ConsoleColor.Green);
        outputView.WriteLine(UIConstants.Message.Divider);
        outputView.WriteLine(message);
        outputView.WriteLine(UIConstants.Message.Divider);
    }

    private void HandleError(Exception ex)
    {
        var errorMessage = ex switch
        {
            GitNotFoundException => UIConstants.Error.GitNotFound,
            AiAuthenticationException authEx => $"{UIConstants.Error.AiAuthFailed}{authEx.Message}",
            GitCommitException commitEx => $"{UIConstants.Error.CommitFailed}{commitEx.Message}",
            ArgumentException { ParamName: "value" } argEx =>
                $"{UIConstants.Error.MessageValidationError}{argEx.Message}",
            _ => $"{UIConstants.Error.UnexpectedError}{ex.Message}"
        };

        outputView.WriteLine(errorMessage, ConsoleColor.Red);
    }
}
