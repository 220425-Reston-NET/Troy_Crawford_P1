using System.Linq;

namespace StoreApp.Models;

public class Order : ICloneable
{
    #region Properties
    public int Id { get; private set; }

    public string StoreFrontAddress { get; set; }

    public List<LineItem> LineItems { get; set; }

    public double TotalPrice
    {
        get
        {
            return LineItems
                .Select(item => item.Quantity * item.Product.Price)
                .Aggregate(0.0, (sum, item) => sum + item);
        }
    }

    public DateTime Timestamp { get; set; }
    #endregion

    #region Constructor
    public Order(
        int id,
        string storeFrontAddress,
        List<LineItem> lineItems,
        DateTime timestamp)
    {
        Id = id;
        StoreFrontAddress = storeFrontAddress;
        LineItems = lineItems;
        Timestamp = timestamp;
    }
    #endregion

    #region Functions
    public void SetId(int id)
    {
        Id = id;
    }

    public object Clone()
    {
        return new Order(
            Id,
            StoreFrontAddress,
            LineItems.Select(item => (LineItem)item.Clone()).ToList(),
            Timestamp);
    }
    #endregion
}
