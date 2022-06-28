using StoreApp.Extensions;
using StoreApp.Models;

namespace StoreApp.Modules;

public class InventoryModule : IConsoleModule
{
    #region Variables
    StoreFront? store;
    #endregion

    #region Properties
    public Logic.Logic Logic { get; set; }
    #endregion

    #region Constructor
    public InventoryModule(Logic.Logic logic)
    {
        Logic = logic;
    }
    #endregion

    #region Functions
    public void Default(StoreFront store)
    {
        this.store = store;
        Default();
    }

    public void Default()
    {
        if (store is null) return;

        ConsoleExt.Header($"{store.Name} - Inventory (MySQL: {Logic.DBVersion})");
        Common.DisplayItems(store.LineItems);

        int choice = ConsoleExt.GetChoice(new string[]
        {
            "1. Add product",
            "2. Remove product",
            "3. Product details",
            "4. Replenish product quantity",
            "5. Back"
        });

        Console.WriteLine();

        switch (choice)
        {
            case 1:
                Add(store);
                Common.Wait();
                break;
            case 2:
                if (store.LineItems.Count > 0)
                    Remove(store);
                else
                    ConsoleExt.Write("No products in store!", Colors.Error);
                Common.Wait();
                break;
            case 3:
                if (store.LineItems.Count > 0)
                    Details(store);
                else
                    ConsoleExt.Write("No products in store!", Colors.Error);
                Common.Wait();
                break;
            case 4:
                if (store.LineItems.Count > 0)
                    Replenish(store);
                else
                    ConsoleExt.Write("No products in store!", Colors.Error);
                Common.Wait();
                break;
            case 5:
                State.InventoryStore = StoreFront.Empty();
                State.Action = Actions.StoreFronts;
                break;
        }
    }

    private void Add(StoreFront store)
    {
        Console.Write("Name: ");
        string name = ConsoleExt.ReadString();

        Console.Write("Price (in $): ");
        double price = ConsoleExt.ReadDouble(0);

        Console.Write("Description: ");
        string? desc = Console.ReadLine();

        Console.Write("Category: ");
        string? category = Console.ReadLine();

        Console.Write("Quantity: ");
        int quantity = ConsoleExt.ReadInt(0);

        var product = new Product(0, name, price, desc, category);

        var productId = Logic.Product.Add(product);

        if (productId is not null)
        {
            product.SetId((int)productId);
            var lineItem = new LineItem(0, product, quantity);
            var lineItemId = Logic.LineItem.Add(lineItem);

            if (lineItemId is not null)
            {
                lineItem.SetId((int)lineItemId);
                store.LineItems.Add(lineItem);

                Logic.StoreFront.UpdateIds(store);
            }
        }

        Console.WriteLine();
        ConsoleExt.Write($"Product added successfully!", Colors.Success);

        State.Logger.Information("Inserted line item & product");
    }

    private void Remove(StoreFront store)
    {
        Console.Write($"Enter id {Common.NumberIndicator(1, store.LineItems.Count)}: ");
        int index = ConsoleExt.ReadInt(1, store.LineItems.Count) - 1;

        if (ConsoleExt.GetConfirmation(
            $"Do you want to delete product, \'{store.LineItems[index].Product.Name}\'"
        ))
        {
            Logic.LineItem.Delete(store.LineItems[index]);

            store.LineItems.RemoveAt(index);
            Logic.StoreFront.UpdateIds(store);

            Console.WriteLine();
            ConsoleExt.Write($"Product removed successfully!", Colors.Success);

            State.Logger.Information("Removed line item & product");
        }
    }

    private void Details(StoreFront store)
    {
        Console.Write($"Enter id {Common.NumberIndicator(1, store.LineItems.Count)}: ");
        int index = ConsoleExt.ReadInt(1, store.LineItems.Count) - 1;

        Console.WriteLine();

        Console.WriteLine($"Name: {store.LineItems[index].Product.Name}");
        Console.WriteLine($"Price: ${string.Format("{0:0.##}", store.LineItems[index].Product.Price)}");
        Console.WriteLine($"Description: {store.LineItems[index].Product.Description}");
        Console.WriteLine($"Category: {store.LineItems[index].Product.Category}");
        Console.WriteLine($"Quantity left: {store.LineItems[index].Quantity}");

        Console.WriteLine();


        State.Logger.Information("Accessed line item & product tables");
    }

    private void Replenish(StoreFront store)
    {
        Console.Write($"Enter id {Common.NumberIndicator(1, store.LineItems.Count)}: ");
        int index = ConsoleExt.ReadInt(1, store.LineItems.Count) - 1;

        Console.Write("Enter new quantity: ");
        int quantity = ConsoleExt.ReadInt(0);

        store.LineItems[index].Quantity = quantity;
        Logic.LineItem.UpdateQuantity(store.LineItems[index]);

        Console.WriteLine();
        ConsoleExt.Write($"Quantity updated successfully!", Colors.Success);

        State.Logger.Information("Updated quantity in line item");
    }
    #endregion
}

