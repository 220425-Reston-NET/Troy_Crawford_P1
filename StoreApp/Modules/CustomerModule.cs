using ConsoleTables;
using StoreApp.Extensions;
using StoreApp.Models;

namespace StoreApp.Modules;

public class CustomerModule : IConsoleModule
{
    #region Properties
    public Logic.Logic Logic { get; set; }
    #endregion

    #region Constructor
    public CustomerModule(Logic.Logic logic)
    {
        Logic = logic;
    }
    #endregion

    #region Functions
    public void Default()
    {
        ConsoleExt.Header($"Customers (MySQL: {Logic.DBVersion})");

        int choice = ConsoleExt.GetChoice(new string[]
        {
            "1. View all",
            "2. Add",
            "3. Search",
            "4. Back"
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
                State.Action = Actions.InitialPrompt;
                break;
        }
    }

    private void View()
    {
        var customers = Logic.Customer.ViewAll;

        if (customers.Count > 0)
        {
            var table = new ConsoleTable(
                "Id",
                "Name",
                "Email/Phone",
                "Address");
            customers?.ForEach(customer => table.AddRow(
                customer.Id,
                customer.Name,
                customer.EmailPhone,
                customer.Address));
            table.Write(Format.Alternative);

            State.Logger.Information("Retrieved from customers table");
        }
        else
            ConsoleExt.Write("No customers present!", Colors.Info);
    }

    private void Add()
    {
        Console.Write("Name: ");
        string name = ConsoleExt.ReadString();

        Console.Write("Address: ");
        string address = ConsoleExt.ReadString();

        Console.Write("Email/Phone: ");
        string emailPhone = ConsoleExt.ReadString();

        Logic.Customer.Add(new Customer(
            0,
            name,
            address,
            emailPhone,
            new List<Order>()));

        Console.WriteLine();
        ConsoleExt.Write($"Customer added successfully!", Colors.Success);

        State.Logger.Information("Inserted to customers table");
    }

    private void Search()
    {
        Console.Write("Name (can be case insensitive and partial): ");
        string name = ConsoleExt.ReadString();

        var customer = Logic.Customer.Find(name);

        if (customer is not null)
        {
            Console.WriteLine();
            Console.WriteLine($"Id: #{customer.Id}");
            Console.WriteLine($"Name: {customer.Name}");
            Console.WriteLine($"Email/Phone: {customer.EmailPhone}");
            Console.WriteLine($"Address: {customer.Address}");
            Console.WriteLine();

            int id = 1;
            Console.WriteLine("Orders (Most recent at top):");
            if (customer.Orders.Count > 0)
                Console.WriteLine();
            else
                ConsoleExt.Write("No orders present!", Colors.Info);
            foreach (Order order in customer.Orders
                .OrderByDescending(ord => ord.Timestamp))
            {
                Console.WriteLine($"{id++}. Ordered at {order.Timestamp}");
                Console.WriteLine($"Store location: {order.StoreFrontAddress}");
                Console.WriteLine($"Total price: ${order.TotalPrice}");
                Common.DisplayItems(order.LineItems);
            }

            State.Logger.Information("Retrieved from customers table");
        }
        else
            ConsoleExt.Write("Customer not found!", Colors.Error);

    }
    #endregion
}

