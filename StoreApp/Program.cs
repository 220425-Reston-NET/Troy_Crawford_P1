using StoreApp;
using StoreApp.Extensions;
using StoreApp.Logic;
using StoreApp.Modules;

class Program
{
    #region Variables
    static readonly Logic logic = new();
    #endregion

    public static void Main(string[] args)
    {
        var done = false;

        var customers = new CustomerModule(logic);
        var stores = new StoreFrontModule(logic);
        var inventory = new InventoryModule(logic);
        var orders = new OrderModule(logic);

        State.Logger.Information("Started");

        if (logic.DBConnected)
        {
            State.Logger.Information("Connected to DB");

            while (!done)
            {
                switch (State.Action)
                {
                    case Actions.InitialPrompt: InitialPrompt(); break;
                    case Actions.Customers: customers.Default(); break;
                    case Actions.StoreFronts: stores.Default(); break;
                    case Actions.Inventory: inventory.Default(State.InventoryStore); break;
                    case Actions.OrderStore:
                    case Actions.OrderCustomer:
                    case Actions.OrderCart:
                        orders.Default(); break;
                    case Actions.Exit:
                        done = true;
                        Common.Wait(true); break;
                }

                Common.ClearConsole();
            }
        }
        else
        {
            ConsoleExt.Write("Error in connecting with database...", Colors.Error);
            Common.Wait(true);
        }
    }

    #region Initial Prompt
    private static void InitialPrompt()
    {
        ConsoleExt.Header($"StoreApp (MySQL: {logic.DBVersion})");

        int choice = ConsoleExt.GetChoice(new string[]
        {
            "1. Customers",
            "2. Store fronts",
            "3. Place order",
            "4. Exit"
        });

        switch (choice)
        {
            case 1:
                State.Action = Actions.Customers;
                break;
            case 2:
                State.Action = Actions.StoreFronts;
                break;
            case 3:
                State.Action = Actions.OrderCustomer;
                break;
            case 4:
                State.Action = Actions.Exit;
                break;
        }
    }
    #endregion
}