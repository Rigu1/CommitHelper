namespace CommitHelper.Presentation.UI;

public class OutputView
{
    public void WriteLine(string message)
    {
        Console.WriteLine(message);
    }

    public void WriteLine(string message, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    public void WriteEmptyLine()
    {
        Console.WriteLine();
    }
}
