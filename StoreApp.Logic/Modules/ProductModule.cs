using StoreApp.Data.Factories;
using StoreApp.Models;

namespace StoreApp.Logic.Modules;

public class ProductModule
{

    #region Functions
    public int? Add(Product product) => ProductFactory.Add(product);
    #endregion
}

