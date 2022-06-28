namespace StoreApp.Extensions;

public class ConsoleExt
{
    #region Functions
    #region Write
    public static void Write(string value, Colors fg, bool newLine = true)
    {
        Console.ForegroundColor = (ConsoleColor)fg;
        Console.Write(value + (newLine ? Environment.NewLine : ""));
        Console.ResetColor();
    }

    public static void Header(string header)
    {
        Write(header, Colors.Info);
        Console.WriteLine();
    }
    #endregion

    #region Read
    public static int ReadInt(int start = -1, int end = -1)
    {
        int? integer = null;

        string startError = start > -1 ?
            $"from {(start < end ? start : end)} " :
            "";

        string endError = end > -1 ?
            $"to {end} " :
            "";

        while (integer == null)
        {
            string input = (Console.ReadLine() ?? "").Trim();
            if (int.TryParse(input, out _))
            {
                integer = int.Parse(input);

                if (start > -1 && integer < start && start < end)
                    integer = null;

                if (end > -1 && integer > end)
                    integer = null;
            }

            if (integer == null)
            {
                Write($"Please enter an integer {startError}{endError}: ", Colors.Error, false);
            }
        }

        return (int)integer;
    }

    public static double ReadDouble(double start = -1, double end = -1)
    {
        double? value = null;

        string startError = start > -1 ?
            $"from {start} " :
            "";

        string endError = end > -1 ?
            $"to {end} " :
            "";

        while (value == null)
        {
            string input = (Console.ReadLine() ?? "").Trim();
            if (double.TryParse(input, out _))
            {
                value = double.Parse(input);

                if (start > -1 && value < start)
                    value = null;

                if (end > -1 && value > end)
                    value = null;
            }

            if (value == null)
            {
                Write($"Please enter an number {startError}{endError}: ", Colors.Error, false);
            }
        }

        return (double)value;
    }

    public static string ReadString()
    {
        string? value = null;

        while (value == null)
        {
            value = (Console.ReadLine() ?? "").Trim();

            if (string.IsNullOrWhiteSpace(value))
            {
                value = null;
                Write($"Please enter valid text: ", Colors.Error, false);
            }
        }

        return value;
    }
    #endregion

    #region Custom Inputs
    public static bool GetConfirmation(string text)
    {
        Write($"{text} (y/N)? ", Colors.Info, false);
        string input = Console.ReadLine() ?? "";

        return char.ToLower(string.IsNullOrWhiteSpace(input) ? 'y' : char.ToLower(input[0])) != 'n';
    }

    public static int GetChoice(string[] options)
    {
        foreach (string option in options)
            Console.WriteLine(option);

        Console.WriteLine();
        Console.Write("Enter choice : ");
        return ReadInt(1, options.Length);
    }
    #endregion
    #endregion
}

public enum Colors : int
{
    Success = ConsoleColor.Green,
    Info = ConsoleColor.DarkYellow,
    Error = ConsoleColor.Red
}