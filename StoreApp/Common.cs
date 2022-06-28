using ConsoleTables;
using StoreApp.Extensions;
using StoreApp.Models;

namespace StoreApp;

public static class Common
{
    internal static void ClearConsole()
    {
        Console.Write("\x1b[3J");
        Console.Clear();
    }

    internal static string NumberIndicator(int start = -1, int end = -1)
    {
        if (start == -1 && end == -1) return "";
        return $"[{start}{(start != end ? "-" + end.ToString() : "")}]";
    }

    internal static void Wait(bool exit = false)
    {
        Console.Write($"Press enter to {(exit ? "exit" : "go back")}...");
        Console.Read();
    }

    internal static void DisplayItems(List<LineItem> lineItems)
    {
        if (lineItems.Count > 0)
        {
            int id = 1;
            var table = new ConsoleTable("Id", "Product", "Price", "Quantity");
            lineItems.ForEach(item => table.AddRow(id++, item.Product.Name, item.Product.Price, item.Quantity));
            table.Write(Format.Alternative);
        }
        else
        {
            ConsoleExt.Write("No items present!", Colors.Info);
            Console.WriteLine();
        }
    }
}

