namespace StoreApp.Models;

public class LineItem : ICloneable
{
    #region Properties
    public int Id { get; private set; }

    public Product Product { get; set; }

    public int Quantity { get; set; }
    #endregion

    #region Constructor
    public LineItem(
        int id,
        Product product,
        int quantity)
    {
        Id = id;
        Product = product;
        Quantity = quantity;
    }
    #endregion

    #region Functions
    public void SetId(int id)
    {
        Id = id;
    }

    public object Clone()
    {
        return new LineItem(
            Id,
            new Product(
                Product.Id,
                Product.Name,
                Product.Price,
                Product.Description,
                Product.Category),
            Quantity);
    }
    #endregion
}
