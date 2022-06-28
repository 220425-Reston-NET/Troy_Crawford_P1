namespace StoreApp.Data;

public static class SqlCommands
{
    public static string LastInsertId { get; } = "SELECT last_insert_id()";

    internal static class Customer
    {
        public static string Table { get; } = "CREATE TABLE IF NOT EXISTS `customers` (`id` INT NOT NULL AUTO_INCREMENT, `name` VARCHAR(255) NOT NULL, `address` TEXT NOT NULL, `email_phone` VARCHAR(255) NOT NULL, `order_ids` TEXT NOT NULL, PRIMARY KEY(`id`))";
        public static string GetAll { get; } = "SELECT * FROM `customers`";
        public static string Add { get; } = "INSERT INTO customers(name, address, email_phone, order_ids) VALUES (@name, @address, @email_phone, @order_ids)";
        public static string FindByName { get; } = "SELECT * FROM customers WHERE LOWER(name) LIKE LOWER(@name)";
        public static string EditIds { get; } = "UPDATE customers SET order_ids=@order_ids WHERE `id` = @id";
    }

    internal static class LineItem
    {
        public static string Table { get; } = "CREATE TABLE IF NOT EXISTS `line_items` (`id` INT NOT NULL AUTO_INCREMENT, `product_id` INT NOT NULL, `quantity` INT NOT NULL, PRIMARY KEY(`id`), FOREIGN KEY (`product_id`) REFERENCES products(`id`))";
        public static string FindById { get; } = "SELECT * FROM `line_items` WHERE `id` = @id";
        public static string Add { get; } = "INSERT INTO line_items(product_id, quantity) VALUES (@product_id, @quantity)";
        public static string Delete { get; } = "DELETE FROM `line_items` WHERE `id` = @id";
        public static string EditQuantity { get; } = "UPDATE line_items SET quantity=@quantity WHERE `id` = @id";
    }

    internal static class Order
    {
        public static string Table { get; } = "CREATE TABLE IF NOT EXISTS `orders` (`id` INT NOT NULL AUTO_INCREMENT, `address` TEXT NOT NULL, `line_item_ids` TEXT NOT NULL, `timestamp` TIMESTAMP NOT NULL ON UPDATE CURRENT_TIMESTAMP, PRIMARY KEY(`id`))";
        public static string Add { get; } = "INSERT INTO orders(address, line_item_ids, timestamp) VALUES (@address, @line_item_ids, @timestamp)";
        public static string FindById { get; } = "SELECT * FROM `orders` WHERE `id` = @id";
    }

    internal static class Product
    {
        public static string Table { get; } = "CREATE TABLE IF NOT EXISTS `products` (`id` INT NOT NULL AUTO_INCREMENT, `name` VARCHAR(255) NOT NULL, `price` DOUBLE NOT NULL, `description` TEXT, `category` VARCHAR(255), PRIMARY KEY(`id`))";
        public static string Add { get; } = "INSERT INTO products(name, price, description, category) VALUES (@name, @price, @description, @category)";
        public static string FindById { get; } = "SELECT * FROM `products` WHERE `id` = @id";
        public static string Delete { get; } = "DELETE FROM `products` WHERE `id` = @id";
    }

    internal static class StoreFront
    {
        public static string Table { get; } = "CREATE TABLE IF NOT EXISTS `store_fronts` (`id` INT NOT NULL AUTO_INCREMENT, `name` VARCHAR(255) NOT NULL, `address` TEXT NOT NULL, `line_item_ids` TEXT NOT NULL, `order_ids` TEXT NOT NULL, PRIMARY KEY(`id`))";
        public static string GetAll { get; } = "SELECT * FROM `store_fronts`";
        public static string Add { get; } = "INSERT INTO store_fronts(name, address, line_item_ids, order_ids) VALUES (@name, @address, @line_item_ids, @order_ids)";
        public static string FindByName { get; } = "SELECT * FROM store_fronts WHERE LOWER(name) LIKE LOWER(@name)";
        public static string EditIds { get; } = "UPDATE store_fronts SET line_item_ids=@line_item_ids, order_ids=@order_ids WHERE `id` = @id";
    }
}

