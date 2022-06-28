namespace StoreApp.Models;

public class StoreFront
{
    #region Properties
    public int Id { get; private set; }

    public string Name { get; set; }

    public string Address { get; set; }

    public List<LineItem> LineItems { get; set; }

    public List<Order> Orders { get; set; }
    #endregion

    #region Constructor
    public StoreFront(
        int id,
        string name,
        string address,
        List<LineItem> lineItems,
        List<Order> orders)
    {
        Id = id;
        Name = name;
        Address = address;
        LineItems = lineItems;
        Orders = orders;
    }
    #endregion

    #region Functions
    public static StoreFront Empty()
    {
        return new StoreFront(0, "", "", new List<LineItem>(), new List<Order>());
    }
    #endregion
}
