using StoreApp.Data.Factories;
using StoreApp.Models;

namespace StoreApp.Logic.Modules;

public class OrderModule
{
    public bool CanOrder(StoreFront? store) => store is not null && store.LineItems.Count > 0;

    public int? Create(Order order) => OrderFactory.Add(order);
}
