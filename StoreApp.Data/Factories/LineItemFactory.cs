using MySqlConnector;
using StoreApp.Extensions;
using StoreApp.Models;

namespace StoreApp.Data.Factories;

public class LineItemFactory
{
    #region Functions
    public static void Table(MySqlConnection connection)
    {
        using var command = new MySqlCommand(
            SqlCommands.LineItem.Table,
            connection);
        _ = command.ExecuteNonQuery();
    }

    public static LineItem? Get(int id)
    {
        LineItem? lineItem = null;

        using (var connection = DB.Connect())
        using (var command = new MySqlCommand(
            SqlCommands.LineItem.FindById,
            connection))
        {
            command.Parameters.AddWithValue("@id", id);
            using var reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();
                var product = ProductFactory.Get(
                    Convert.ToInt32(reader.GetValue(1))
                );

                if (product is not null)
                    lineItem = new LineItem(
                            Convert.ToInt32(reader.GetValue(0)),
                            product,
                            Convert.ToInt32(reader.GetValue(2))
                        );
            }
        }

        return lineItem;
    }

    public static int? Add(LineItem lineItem)
    {
        using var connection = DB.Connect();
        using var command = new MySqlCommand(SqlCommands.LineItem.Add, connection);

        command.Parameters.AddWithValue("@product_id", lineItem.Product.Id);
        command.Parameters.AddWithValue("@quantity", lineItem.Quantity);

        _ = command.ExecuteNonQuery();

        using var idCommand = new MySqlCommand(
            SqlCommands.LastInsertId,
            connection);

        return Convert.ToInt32(idCommand.ExecuteScalar());
    }

    public static void Delete(LineItem lineItem)
    {
        using var connection = DB.Connect();
        using var command = new MySqlCommand(SqlCommands.LineItem.Delete, connection);

        command.Parameters.AddWithValue("@id", lineItem.Id);
        _ = command.ExecuteNonQuery();
    }

    public static void UpdateQuantity(LineItem lineItem)
    {
        using var connection = DB.Connect();
        using var command = new MySqlCommand(SqlCommands.LineItem.EditQuantity, connection);

        command.Parameters.AddWithValue("@id", lineItem.Id);
        command.Parameters.AddWithValue("@quantity", lineItem.Quantity);

        _ = command.ExecuteNonQuery();
    }

    internal static List<LineItem> ResolveIds(string ids)
    {
        var lineItems = new List<LineItem>();

        foreach (int id in ids.GetIds())
        {
            var lineItem = Get(id);

            if (lineItem is not null)
                lineItems.Add(lineItem);
        }

        return lineItems;
    }
    #endregion
}

