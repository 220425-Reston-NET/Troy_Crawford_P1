using MySqlConnector;
using StoreApp.Extensions;
using StoreApp.Models;

namespace StoreApp.Data.Factories;

public class CustomerFactory
{
    #region Functions
    public static void Table(MySqlConnection connection)
    {
        using var command = new MySqlCommand(
            SqlCommands.Customer.Table,
            connection);
        _ = command.ExecuteNonQuery();
    }

    public static List<Customer> GetAll(bool resolveOrders = false)
    {
        var customers = new List<Customer>();

        using (var connection = DB.Connect())
        using (var command = new MySqlCommand(SqlCommands.Customer.GetAll, connection))
        using (var reader = command.ExecuteReader())
        {
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    List<Order> orders = resolveOrders ?
                        OrderFactory.ResolveIds(reader.GetValue(4).ToString() ?? "") :
                        new List<Order>();

                    customers.Add(new Customer(
                        Convert.ToInt32(reader.GetValue(0)),
                        reader.GetValue(1).ToString() ?? "",
                        reader.GetValue(2).ToString().Truncate(30) ?? "",
                        reader.GetValue(3).ToString() ?? "",
                        orders
                    ));
                }
            }
        }

        return customers;
    }

    public static void Add(Customer customer)
    {
        using var connection = DB.Connect();
        using var command = new MySqlCommand(SqlCommands.Customer.Add, connection);

        command.Parameters.AddWithValue("@name", customer.Name);
        command.Parameters.AddWithValue("@address", customer.Address);
        command.Parameters.AddWithValue("@email_phone", customer.EmailPhone);
        command.Parameters.AddWithValue("@order_ids", "");

        command.ExecuteNonQuery();
    }

    public static Customer? Find(string input)
    {
        Customer? customer = null;

        using (var connection = DB.Connect())
        using (var command = new MySqlCommand(SqlCommands.Customer.FindByName, connection))
        {
            command.Parameters.AddWithValue("@name", $"%{input}%");
            using var reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();
                List<Order> orders = OrderFactory.ResolveIds(reader.GetValue(4).ToString() ?? "");

                customer = new Customer(
                    Convert.ToInt32(reader.GetValue(0)),
                    reader.GetValue(1).ToString() ?? "",
                    reader.GetValue(2).ToString() ?? "",
                    reader.GetValue(3).ToString() ?? "",
                    orders
                );
            }
        }

        return customer;
    }

    public static void UpdateIds(Customer customer)
    {
        using var connection = DB.Connect();
        using var command = new MySqlCommand(SqlCommands.Customer.EditIds, connection);

        command.Parameters.AddWithValue("@id", customer.Id);
        command.Parameters.AddWithValue("@order_ids", customer.Orders.Select(order => order.Id).ToDBString());

        _ = command.ExecuteNonQuery();
    }
    #endregion
}

