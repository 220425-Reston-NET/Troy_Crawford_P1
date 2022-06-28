using StoreApp.Data.Factories;
using StoreApp.Models;

namespace StoreApp.Logic.Modules;

public class LineItemModule
{
    #region Functions
    public int? Add(LineItem lineItem) => LineItemFactory.Add(lineItem);

    public void Delete(LineItem lineItem)
    {
        LineItemFactory.Delete(lineItem);
        ProductFactory.Delete(lineItem.Product);
    }

    public void UpdateQuantity(LineItem lineItem) => LineItemFactory.UpdateQuantity(lineItem);
    #endregion
}

