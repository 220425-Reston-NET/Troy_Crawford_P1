using StoreApp.Extensions;
using StoreApp.Models;

namespace StoreApp.Tests;

public class Tests
{
    #region General
    [Test]
    public void OrderSum()
    {
        Order order = new(0, "Address", new(), DateTime.Now);
        order.LineItems = new List<LineItem>()
        {
            new LineItem(0, new Product("Test 1", 200), 5),
            new LineItem(0, new Product("Test 2", 500), 5),
            new LineItem(0, new Product("Test 3", 800), 5),
        };

        Assert.That(order.TotalPrice, Is.EqualTo(7500.0));
    }

    [Test]
    public void TruncateString()
    {
        Assert.That("Pine Street".Truncate(3), Is.EqualTo("Pin..."));
    }

    [Test]
    public void StringToIds()
    {
        var ids = "1,2,3".GetIds();

        Assert.Multiple(() =>
        {
            Assert.That(ids[0], Is.EqualTo(1));
            Assert.That(ids[1], Is.EqualTo(2));
        });
    }

    [Test]
    public void IdsToString()
    {
        Assert.That((new int[] { 1, 2, 3 }).ToDBString(), Is.EqualTo("1,2,3"));
    }

    [Test]
    public void OrderValidation()
    {
        var logic = new Logic.Logic();
        var store = new StoreFront(0, "Name", "Address", new(), new());

        Assert.That(logic.Order.CanOrder(store), Is.False);
    }
    #endregion

    #region DB
    [Test]
    public void AddProduct()
    {
        var logic = new Logic.Logic();
        var prod = new Product("Apple", 2.49);

        var id = logic.Product.Add(prod);
        Assert.That(id is not null, Is.True);
    }

    [Test]
    public void AddLineItem()
    {
        var logic = new Logic.Logic();

        var prod = new Product("Apple", 2.49);
        var id = logic.Product.Add(prod);

        if (id is null) Assert.Fail();
        prod.SetId(id ?? 0);

        var item = new LineItem(0, prod, 5);
        id = logic.LineItem.Add(item);

        Assert.That(id is not null, Is.True);
    }

    [Test]
    public void AddCustomer()
    {
        var logic = new Logic.Logic();
        var customer = new Customer(0, "Test", "Address", "123456789", new());
        logic.Customer.Add(customer);

        Assert.Pass();
    }

    [Test]
    public void SearchCustomer()
    {
        var logic = new Logic.Logic();
        var customer = new Customer(0, "Test", "Address", "123456789", new());
        logic.Customer.Add(customer);

        Assert.That(logic.Customer.Find("Test") is not null, Is.True);
    }

    [Test]
    public void AddStore()
    {
        var logic = new Logic.Logic();
        var store = new StoreFront(0, "Store", "Address", new(), new());
        logic.StoreFront.Add(store);

        Assert.Pass();
    }
    #endregion
}
