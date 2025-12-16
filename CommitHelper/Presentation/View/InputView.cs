namespace CommitHelper.Presentation.View;

public class InputView
{
    public string ReadLine()
    {
        return Console.ReadLine() ?? string.Empty;
    }
}
