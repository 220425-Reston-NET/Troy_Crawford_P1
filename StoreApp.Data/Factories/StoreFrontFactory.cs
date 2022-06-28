using MySqlConnector;
using StoreApp.Extensions;
using StoreApp.Models;

namespace StoreApp.Data.Factories;

public class StoreFrontFactory
{
    #region Functions
    public static void Table(MySqlConnection connection)
    {
        using var command = new MySqlCommand(
            SqlCommands.StoreFront.Table,
            connection);
        _ = command.ExecuteNonQuery();
    }

    public static StoreFront? Find(string input)
    {
        StoreFront? storeFront = null;

        using (var connection = DB.Connect())
        using (var command = new MySqlCommand(SqlCommands.StoreFront.FindByName, connection))
        {
            command.Parameters.AddWithValue("@name", $"%{input}%");
            using var reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();
                int id = Convert.ToInt32(reader.GetValue(0));
                string name = reader.GetValue(1).ToString() ?? "";
                string address = reader.GetValue(2).ToString().Truncate(30) ?? "";
                string liIds = reader.GetValue(3).ToString() ?? "";
                string orderIds = reader.GetValue(4).ToString() ?? "";
                reader.Close();

                reader.Dispose();
                command.Dispose();

                List<LineItem> lineItems = LineItemFactory.ResolveIds(liIds).ToList();
                List<Order> orders = OrderFactory.ResolveIds(orderIds).ToList();

                storeFront = new StoreFront(
                    id,
                    name,
                    address,
                    lineItems,
                    orders
                );
            }
        }

        return storeFront;
    }

    public static void Add(StoreFront storeFront)
    {
        using var connection = DB.Connect();
        using var command = new MySqlCommand(SqlCommands.StoreFront.Add, connection);

        command.Parameters.AddWithValue("@name", storeFront.Name);
        command.Parameters.AddWithValue("@address", storeFront.Address);
        command.Parameters.AddWithValue("@line_item_ids", "");
        command.Parameters.AddWithValue("@order_ids", "");

        command.ExecuteNonQuery();
    }

    public static void UpdateIds(StoreFront storeFront)
    {
        using var connection = DB.Connect();
        using var command = new MySqlCommand(SqlCommands.StoreFront.EditIds, connection);

        command.Parameters.AddWithValue("@id", storeFront.Id);
        command.Parameters.AddWithValue("@line_item_ids", storeFront.LineItems.Select(item => item.Id).ToDBString());
        command.Parameters.AddWithValue("@order_ids", storeFront.Orders.Select(order => order.Id).ToDBString());

        _ = command.ExecuteNonQuery();
    }

    public static List<StoreFront> GetAll(bool resolveFully = false)
    {
        var storeFronts = new List<StoreFront>();

        using (var connection = DB.Connect())
        using (var command = new MySqlCommand(SqlCommands.StoreFront.GetAll, connection))
        using (var reader = command.ExecuteReader())
        {
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    List<LineItem> lineItems = resolveFully ?
                        LineItemFactory.ResolveIds(reader.GetValue(3).ToString() ?? "") :
                        new List<LineItem>();

                    List<Order> orders = resolveFully ?
                        OrderFactory.ResolveIds(reader.GetValue(4).ToString() ?? "") :
                        new List<Order>();

                    storeFronts.Add(new StoreFront(
                        Convert.ToInt32(reader.GetValue(0)),
                        reader.GetValue(1).ToString() ?? "",
                        reader.GetValue(2).ToString().Truncate(30) ?? "",
                        lineItems,
                        orders
                    ));
                }
            }
        }

        return storeFronts;
    }
    #endregion
}
