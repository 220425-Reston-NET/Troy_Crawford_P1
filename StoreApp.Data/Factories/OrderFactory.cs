using MySqlConnector;
using StoreApp.Extensions;
using StoreApp.Models;

namespace StoreApp.Data.Factories;

public class OrderFactory
{
    #region Functions
    public static void Table(MySqlConnection connection)
    {
        using var command = new MySqlCommand(
            SqlCommands.Order.Table,
            connection);
        _ = command.ExecuteNonQuery();
    }

    public static Order? Get(int id)
    {
        Order? order = null;

        using (var connection = DB.Connect())
        using (var command = new MySqlCommand(SqlCommands.Order.FindById, connection))
        {
            command.Parameters.AddWithValue("@id", id);
            using var reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();

                var lineItems = new List<LineItem>();
                foreach (int lineItemId in (reader.GetValue(2).ToString() ?? "")
                        .GetIds())
                {
                    var lineItem = LineItemFactory.Get(lineItemId);

                    if (lineItem is not null)
                        lineItems.Add(lineItem);
                }

                if (lineItems.Count > 0)
                    order = new Order(
                        Convert.ToInt32(reader.GetValue(0)),
                        reader.GetValue(1).ToString() ?? "",
                        lineItems,
                        Convert.ToDateTime(reader.GetValue(3))
                    );
            }
        }

        return order;
    }

    public static int Add(Order order)
    {
        using var connection = DB.Connect();
        using var command = new MySqlCommand(SqlCommands.Order.Add, connection);

        List<int> lineItems = new();
        foreach (var lineItem in order.LineItems)
        {
            var id = LineItemFactory.Add(lineItem);
            if (id is not null)
                lineItems.Add((int)id);
        }

        command.Parameters.AddWithValue("@address", order.StoreFrontAddress);
        command.Parameters.AddWithValue("@line_item_ids", lineItems.ToDBString());
        command.Parameters.AddWithValue("@timestamp", order.Timestamp);

        _ = command.ExecuteNonQuery();

        using var idCommand = new MySqlCommand(SqlCommands.LastInsertId, connection);

        return Convert.ToInt32(idCommand.ExecuteScalar());
    }

    internal static List<Order> ResolveIds(string ids)
    {
        var orders = new List<Order>();

        foreach (int id in ids.GetIds())
        {
            var order = Get(id);

            if (order is not null)
                orders.Add(order);
        }

        return orders;
    }
    #endregion
}

