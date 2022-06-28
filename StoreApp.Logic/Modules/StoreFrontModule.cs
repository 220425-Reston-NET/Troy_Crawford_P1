using StoreApp.Data.Factories;
using StoreApp.Models;

namespace StoreApp.Logic.Modules;

public class StoreFrontModule
{
    #region Functions
    public List<StoreFront> ViewAll() => StoreFrontFactory.GetAll();

    public void Add(StoreFront storeFront) => StoreFrontFactory.Add(storeFront);

    public void UpdateIds(StoreFront storeFront) => StoreFrontFactory.UpdateIds(storeFront);

    public StoreFront? Find(string input) => StoreFrontFactory.Find(input);
    #endregion
}

