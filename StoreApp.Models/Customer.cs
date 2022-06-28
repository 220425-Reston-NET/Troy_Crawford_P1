namespace StoreApp.Models;

public class Customer
{
    #region Properties
    public int Id { get; private set; }

    public string Name { get; set; }

    public string Address { get; set; }

    public string EmailPhone { get; set; }

    public List<Order> Orders { get; set; }
    #endregion

    #region Constructor
    public Customer(
        int id,
        string name,
        string address,
        string emailPhone,
        List<Order> orders)
    {
        Id = id;
        Name = name;
        Address = address;
        EmailPhone = emailPhone;
        Orders = orders;
    }
    #endregion
    //testing this out///
}