using StoreApp.Data.Factories;
using StoreApp.Models;

namespace StoreApp.Logic.Modules;

public class CustomerModule
{
    #region Functions
    public List<Customer> ViewAll => CustomerFactory.GetAll();

    public void Add(Customer customer) => CustomerFactory.Add(customer);

    public Customer? Find(string input) => CustomerFactory.Find(input);

    public void UpdateIds(Customer customer) => CustomerFactory.UpdateIds(customer);
    #endregion
}
