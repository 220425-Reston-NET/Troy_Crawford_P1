namespace StoreApp.Models;

public class Product
{
    #region Properties
    public int Id { get; private set; }

    public string Name { get; set; }

    public double Price { get; set; }

    public string? Description { get; set; }

    public string? Category { get; set; }
    #endregion

    #region Constructor
    public Product(
        int id,
        string name,
        double price,
        string? desc,
        string? category)
    {
        Id = id;
        Name = name;
        Price = price;
        Description = desc;
        Category = category;
    }

    public Product(
        string name,
        double price)
    {
        Name = name;
        Price = price;
    }
    #endregion

    #region Functions
    public void SetId(int id)
    {
        Id = id;
    }
    #endregion
}
