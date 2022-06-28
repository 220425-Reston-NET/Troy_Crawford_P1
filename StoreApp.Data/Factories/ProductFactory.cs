using MySqlConnector;
using StoreApp.Models;

namespace StoreApp.Data.Factories;

public class ProductFactory
{
    #region Functions
    public static void Table(MySqlConnection connection)
    {
        using var command = new MySqlCommand(
            SqlCommands.Product.Table,
            connection);
        _ = command.ExecuteNonQuery();
    }

    public static Product? Get(int id)
    {
        Product? product = null;

        using (var connection = DB.Connect())
        using (var command = new MySqlCommand(SqlCommands.Product.FindById, connection))
        {

            command.Parameters.AddWithValue("@id", id);
            using var reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();
                product = new Product(
                    Convert.ToInt32(reader.GetValue(0)),
                    reader.GetValue(1).ToString() ?? "",
                    Convert.ToDouble(reader.GetValue(2)),
                    reader.GetValue(3).ToString(),
                    reader.GetValue(4).ToString()
                );
            }
        }

        return product;
    }

    public static int Add(Product product)
    {
        using var connection = DB.Connect();
        using var command = new MySqlCommand(SqlCommands.Product.Add, connection);

        command.Parameters.AddWithValue("@name", product.Name);
        command.Parameters.AddWithValue("@price", product.Price);
        command.Parameters.AddWithValue("@description", product.Description);
        command.Parameters.AddWithValue("@category", product.Category);

        _ = command.ExecuteNonQuery();

        using var idCommand = new MySqlCommand(SqlCommands.LastInsertId, connection);

        return Convert.ToInt32(idCommand.ExecuteScalar());
    }

    public static void Delete(Product product)
    {
        using var connection = DB.Connect();
        using var command = new MySqlCommand(SqlCommands.Product.Delete, connection);

        command.Parameters.AddWithValue("@id", product.Id);
        _ = command.ExecuteNonQuery();
    }
    #endregion
}

