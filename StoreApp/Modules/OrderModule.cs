using ConsoleTables;
using StoreApp.Extensions;
using StoreApp.Models;

namespace StoreApp.Modules;

public class OrderModule : IConsoleModule
{

    #region Variables
    StoreFront? store;
    StoreFront? modifiedStore;
    Customer? customer;

    CartModule cart;
    #endregion

    #region Properties
    public Logic.Logic Logic { get; set; }
    #endregion

    #region Constructor
    public OrderModule(Logic.Logic logic)
    {
        Logic = logic;
        cart = new(logic);
    }
    #endregion

    #region Functions
    public void Default()
    {
        ConsoleExt.Header($"Place Order (MySQL: {Logic.DBVersion})");

        switch (State.Action)
        {
            case Actions.OrderCustomer:
                Customer();
                break;
            case Actions.OrderStore:
                Store();
                break;
            case Actions.OrderCart:
                if (store is not null
                    && modifiedStore is not null
                    && customer is not null)
                    cart.Default(store, modifiedStore, customer);
                else
                    State.Action = Actions.InitialPrompt;
                break;
        }
    }


    private void Customer()
    {
        Console.Write("Enter customer name: ");
        string name = ConsoleExt.ReadString();

        var customer = Logic.Customer.Find(name);
        string confMessage;

        if (customer is not null)
        {
            this.customer = customer;
            ConsoleExt.Write($"Customer found! - {customer.Name}", Colors.Success);
            confMessage = "Do you want to continue";

            State.Logger.Information("Retrieved from customers");
        }
        else
        {
            ConsoleExt.Write("Customer not found!", Colors.Error);
            confMessage = "Do you want to retry";
        }

        State.Action = Actions.InitialPrompt;

        if (ConsoleExt.GetConfirmation(confMessage))
            if (customer is not null)
                State.Action = Actions.OrderStore;
            else
                State.Action = Actions.OrderCustomer;
    }

    private void Store()
    {
        Console.Write("Enter store name: ");
        string name = ConsoleExt.ReadString();

        var store = Logic.StoreFront.Find(name);
        string confMessage;

        if (store is not null)
        {
            this.store = store;
            ConsoleExt.Write($"Store front found! - {store.Name}", Colors.Success);
            confMessage = "Do you want to continue";

            State.Logger.Information("Retrieved from store fronts");
        }
        else
        {
            ConsoleExt.Write("Store front not found!", Colors.Error);
            confMessage = "Do you want to retry";
        }

        State.Action = Actions.InitialPrompt;

        if (ConsoleExt.GetConfirmation(confMessage))
            if (store is not null)
            {
                if (Logic.Order.CanOrder(store))
                {
                    State.Action = Actions.OrderCart;
                    modifiedStore = new StoreFront(
                        store.Id,
                        store.Name,
                        store.Address,
                        store.LineItems.Select(item => (LineItem)item.Clone()).ToList(),
                        store.Orders.Select(order => (Order)order.Clone()).ToList());
                }
                else
                    ConsoleExt.Write("Store inventory is empty!", Colors.Error);
            }
            else
                State.Action = Actions.OrderStore;
    }
    #endregion
}

