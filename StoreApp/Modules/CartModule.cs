using ConsoleTables;
using StoreApp.Extensions;
using StoreApp.Models;

namespace StoreApp.Modules;

public class CartModule : IConsoleModule
{
    #region Variables
    StoreFront? store;
    StoreFront? modified;
    Customer? customer;
    readonly List<LineItem> cart = new();
    #endregion

    #region Properties
    public Logic.Logic Logic { get; set; }
    #endregion

    #region Constructor
    public CartModule(Logic.Logic logic)
    {
        Logic = logic;
    }
    #endregion

    #region Functions

    public void Default(
        StoreFront store,
        StoreFront modified,
        Customer customer)
    {
        this.store = store;
        this.modified = modified;
        this.customer = customer;
        Default();
    }

    public void Default()
    {
        if (store is null ||
            modified is null ||
            customer is null) return;

        Console.WriteLine($"Store: {store.Name}");
        Console.WriteLine($"Customer: {customer.Name}");
        Console.WriteLine();

        #region Update Store
        for (int itemIndex = 0; itemIndex < store.LineItems.Count; itemIndex++)
        {
            var cartItem = cart.FirstOrDefault(item => item.Id == store.LineItems[itemIndex].Id);
            var cartCount = cartItem is not null ? cartItem.Quantity : 0;

            modified.LineItems[itemIndex].Quantity = store.LineItems[itemIndex].Quantity - cartCount;
        }
        #endregion

        ConsoleExt.Write("Inventory: ", Colors.Info);
        Common.DisplayItems(modified.LineItems);

        ConsoleExt.Write("Cart: ", Colors.Info);
        Common.DisplayItems(cart);

        double total = cart
            .Select(item => item.Quantity * item.Product.Price)
            .Aggregate(0.0, (sum, item) => sum + item);

        ConsoleExt.Write($"Total: ${total}", Colors.Info);
        Console.WriteLine();

        int choice = ConsoleExt.GetChoice(new string[]
        {
            "1. Add to cart",
            "2. Remove from cart",
            "3. Edit quantity",
            "4. Proceed to buy",
            "5. Back"
        });

        Console.WriteLine();

        switch (choice)
        {
            case 1:
                Add(store, cart);
                break;
            case 2:
                if (cart.Count > 0)
                    Remove(cart);
                else
                {
                    ConsoleExt.Write("No items in cart!", Colors.Error);
                    Common.Wait();
                }
                break;
            case 3:
                if (cart.Count > 0)
                    EditQuantity(store, cart);
                else
                {
                    ConsoleExt.Write("No items in cart!", Colors.Error);
                    Common.Wait();
                }
                break;
            case 4:
                if (cart.Count > 0)
                    Buy(store, customer, cart);
                else
                    ConsoleExt.Write("No items in cart!", Colors.Error);
                Common.Wait();
                break;
            case 5:
                State.Action = Actions.InitialPrompt;
                break;
        }
    }

    public void Add(StoreFront modified, List<LineItem> cart)
    {
        Console.Write($"Enter id {Common.NumberIndicator(1, modified.LineItems.Count)}: ");
        int index = ConsoleExt.ReadInt(1, modified.LineItems.Count) - 1;

        Console.Write($"Quantity {Common.NumberIndicator(1, modified.LineItems[index].Quantity)}: ");
        int quantity = ConsoleExt.ReadInt(1, modified.LineItems[index].Quantity);

        int itemIndex = cart.FindIndex(item => item.Id == modified.LineItems[index].Id);

        if (itemIndex == -1)
            cart.Add(new LineItem(
                modified.LineItems[index].Id,
                modified.LineItems[index].Product,
                quantity));
        else
            cart[itemIndex].Quantity += quantity;

    }

    public void Remove(List<LineItem> cart)
    {
        ConsoleExt.Write("Remove from cart", Colors.Info);
        Console.WriteLine();

        Console.Write($"Enter cart id {Common.NumberIndicator(1, cart.Count)}: ");
        int index = ConsoleExt.ReadInt(1, cart.Count) - 1;

        cart.RemoveAt(index);
    }

    public void EditQuantity(StoreFront store, List<LineItem> cart)
    {
        Console.Write($"Enter cart id {Common.NumberIndicator(1, cart.Count)}: ");
        int index = ConsoleExt.ReadInt(1, cart.Count) - 1;

        int itemIndex = store.LineItems.FindIndex(item => item.Id == cart[index].Id);

        Console.Write($"Quantity {Common.NumberIndicator(1, store.LineItems[itemIndex].Quantity)}: ");
        int quantity = ConsoleExt.ReadInt(1, store.LineItems[itemIndex].Quantity);

        cart[index].Quantity = quantity;
    }

    public void Buy(
        StoreFront store,
        Customer customer,
        List<LineItem> cart)
    {
        if (!ConsoleExt.GetConfirmation("Do you want to proceed to buy")) return;

        var order = new Order(0, store.Address, cart, DateTime.Now);
        var orderId = Logic.Order.Create(order);

        if (orderId is not null)
        {
            order.SetId((int)orderId);

            store.Orders.Add(order);
            customer.Orders.Add(order);

            for (int itemIndex = 0; itemIndex < store.LineItems.Count; itemIndex++)
            {
                var cartItem = cart.FirstOrDefault(item => item.Id == store.LineItems[itemIndex].Id);

                if (cartItem is not null)
                {
                    store.LineItems[itemIndex].Quantity =
                        store.LineItems[itemIndex].Quantity -
                        cartItem.Quantity;
                    Logic.LineItem.UpdateQuantity(store.LineItems[itemIndex]);
                }
            }

            Logic.StoreFront.UpdateIds(store);
            Logic.Customer.UpdateIds(customer);

            ConsoleExt.Write("Order created successfully", Colors.Success);

            State.Logger.Information("Created order successfully");
        }

        State.Action = Actions.InitialPrompt;
    }
    #endregion
}

