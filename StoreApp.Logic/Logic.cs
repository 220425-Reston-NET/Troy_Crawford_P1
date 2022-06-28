using StoreApp.Data;
using StoreApp.Data.Factories;
using StoreApp.Logic.Modules;

namespace StoreApp.Logic;

public class Logic
{
    #region Properties
    public bool DBConnected { get; set; } = false;

    public string DBVersion { get; private set; } = "";

    // Modules
    public CustomerModule Customer { get; private set; } = new();

    public OrderModule Order { get; private set; } = new();

    public StoreFrontModule StoreFront { get; private set; } = new();

    public ProductModule Product { get; private set; } = new();

    public LineItemModule LineItem { get; private set; } = new();
    #endregion

    #region Constructor
    public Logic()
    {
        try
        {
            // DB
            DB.Init();
            DBVersion = DB.Version;

            // Create tables
            using (var connection = DB.Connect())
            {
                ProductFactory.Table(connection);
                LineItemFactory.Table(connection);
                StoreFrontFactory.Table(connection);
                OrderFactory.Table(connection);
                CustomerFactory.Table(connection);
            }

            // Connected
            DBConnected = true;
        }
        catch
        {
            DBConnected = false;
        }
    }
    #endregion
}

