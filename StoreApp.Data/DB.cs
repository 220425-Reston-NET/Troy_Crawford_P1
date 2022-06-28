

using MySqlConnector;

namespace StoreApp.Data;

public static class DB
{
    #region Properties
    public static string Version { get; private set; } = "";
    #endregion

    #region Constants
    // Connection Params
    private static readonly string
        SERVER = "192.168.64.2",
        USER = "admin",
        PASSWORD = "admin",
        DB_NAME = "store_app";
    #endregion

    #region Functions
    public static void Init()
    {
        using var connection = Connect();
        Version = connection.ServerVersion;
    }

    public static MySqlConnection Connect()
    {
        var builder = new MySqlConnectionStringBuilder()
        {
            Server = SERVER,
            UserID = USER,
            Password = PASSWORD,
            Database = DB_NAME,
            Pooling = false,
            ConvertZeroDateTime = true
        };

        var connection = new MySqlConnection(builder.ToString());
        connection.Open();

        return connection;
    }
    #endregion
}

