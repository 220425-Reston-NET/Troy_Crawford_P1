using ConsoleTables;
using StoreApp.Extensions;
using StoreApp.Models;

namespace StoreApp.Modules;

public class StoreFrontModule : IConsoleModule
{
    #region Properties
    public Logic.Logic Logic { get; set; }
    #endregion

    #region Constructor
    public StoreFrontModule(Logic.Logic logic)
    {
        Logic = logic;
    }
    #endregion

    #region Functions
    public void Default()
    {
        ConsoleExt.Header($"Store Fronts (MySQL: {Logic.DBVersion})");

        int choice = ConsoleExt.GetChoice(new string[]
        {
            "1. View all",
            "2. Add",
            "3. Search",
            "4. Inventory",
            "5. Back"
        });

        Console.WriteLine();

        switch (choice)
        {
            case 1:
                View();
                Common.Wait();
                break;
            case 2:
                Add();
                Common.Wait();
                break;
            case 3:
                Search();
                Common.Wait();
                break;
            case 4:
                Inventory();
                break;
            case 5:
                State.Action = Actions.InitialPrompt;
                break;
        }
    }

    private void View()
    {
        var storeFronts = Logic.StoreFront.ViewAll();

        if (storeFronts.Count > 0)
        {
            var table = new ConsoleTable("Id", "Name", "Address");
            storeFronts?.ForEach(customer => table.AddRow(customer.Id, customer.Name, customer.Address));
            table.Write(Format.Alternative);

            State.Logger.Information("Retrieved from store fronts");
        }
        else
            ConsoleExt.Write("No store fronts present!", Colors.Info);
    }

    private void Search()
    {
        Console.Write("Name (can be case insensitive and partial): ");
        string name = ConsoleExt.ReadString();

        var store = Logic.StoreFront.Find(name);

        if (store is not null)
        {
            Console.WriteLine();
            Console.WriteLine($"Id: #{store.Id}");
            Console.WriteLine($"Name: {store.Name}");
            Console.WriteLine($"Address: {store.Address}");
            Console.WriteLine();

            int id = 1;
            Console.WriteLine("Orders (Most recent at top):");
            if (store.Orders.Count > 0)
                Console.WriteLine();
            else
                ConsoleExt.Write("No orders present!", Colors.Info);
            foreach (Order order in store.Orders.OrderByDescending(ord => ord.Timestamp))
            {
                Console.WriteLine($"{id++}. Ordered at {order.Timestamp}");
                Console.WriteLine($"Total price: ${order.TotalPrice}");
                Common.DisplayItems(order.LineItems);
            }

            State.Logger.Information("Retrieved from store fronts");
        }
        else
            ConsoleExt.Write("Store not found!", Colors.Error);

    }

    private void Add()
    {
        Console.Write("Name: ");
        string name = ConsoleExt.ReadString();

        Console.Write("Address: ");
        string address = ConsoleExt.ReadString();

        Logic.StoreFront.Add(new StoreFront(0, name, address, new List<LineItem>(), new List<Order>()));

        Console.WriteLine();
        ConsoleExt.Write($"Store front added successfully!", Colors.Success);

        State.Logger.Information("Inserted to store fronts");
    }

    private void Inventory()
    {
        Console.Write("Store name: ");
        string name = ConsoleExt.ReadString();

        var store = Logic.StoreFront.Find(name);

        if (store is not null)
        {
            State.InventoryStore = store;
            State.Action = Actions.Inventory;
        }
        else
        {
            ConsoleExt.Write("Store not found!", Colors.Error);
            Common.Wait();
        }
    }
    #endregion
}

