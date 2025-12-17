namespace CommitHelper.Presentation.UI;

public class InputView
{
    public string ReadLine()
    {
        return Console.ReadLine() ?? string.Empty;
    }
}
