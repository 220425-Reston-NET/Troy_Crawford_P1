using Serilog;
using Serilog.Core;
using StoreApp.Models;

namespace StoreApp.Extensions;

public static class State
{
    public static Actions Action { get; set; } = Actions.InitialPrompt;

    public static StoreFront InventoryStore { get; set; } = StoreFront.Empty();

    public static Logger Logger { get; } = new LoggerConfiguration()
        .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)
        .CreateLogger();
}

public enum Actions
{
    // Initial
    InitialPrompt,

    // Customer
    Customers,

    // Stores
    StoreFronts,
    Inventory,

    // Order
    OrderCustomer,
    OrderStore,
    OrderCart,

    // Misc
    Exit
}
