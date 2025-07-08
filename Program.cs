// See https://aka.ms/new-console-template for more information
using System.Data.Common;
using System.Data.SQLite; // classes for database access
// System.Data.SQLite is a.NET data provider for SQLite , which allows developers to interact with
// SQLite databases using the ADO.NET programming model. This namespace will provide classes
// and properties needed to access the SQLite database.

// Another namespace you can use is the one provided by the Microsoft called
// Microsoft.Data.Sqlite

// Both libraries offer similar functionalities for working with SQLite databases, including
// connection management, command execution, data retrieval, data manipulation, and
// transaction support.

internal class Program
{
    private static void Main(string[] args)
    {
        Start();
        Console.WriteLine("Hello, World!");
    }


    static void Start()
    {
        Console.WriteLine("     ... it's a simple database have one tabel two colomn product and price ...");
        using (SQLiteConnection sqliteConnection = CreateConnection())
        {
            CreateTable(sqliteConnection);
            int choice;



            do
            {
                Console.WriteLine(" ... Which Opreation You Want Execute ? ...");
                Console.WriteLine(" 1. Create || 2. ReadAll || 3.Update || 4. Delete || 0. Exit");
                Console.Write("* Enter Your Choice: ");

                if (!int.TryParse(Console.ReadLine(), out choice))
                {
                    Console.WriteLine("Invalid input. Please enter a number from 0 to 4.");
                    continue;
                }
                switch (choice)
                {
                    case 1:
                        Console.Write("* Enter Product Name : ");
                        string name = Console.ReadLine();
                        Console.Write("* Enter Product Price : ");
                        if (int.TryParse(Console.ReadLine(), out int price))
                            Create(sqliteConnection, name, price);
                            
                            //Console.Write("* Enter Product Quantity : ");
                            //if (int.TryParse(Console.ReadLine(), out int quantity))
                            //{
                            //    Console.Write("* Enter Product Price : ");
                            //    if (int.TryParse(Console.ReadLine(), out int price))
                            //        CreateProduct(sqliteConnection, id, name!, quantity, price);
                            //}
                        
                       
                        break;

                    case 2:
                        ReadAll(sqliteConnection);
                        break;

                    case 3:
                        UpdateByID(sqliteConnection);
                        break;

                    case 4:
                        DeleteByID(sqliteConnection);
                        break;

                    case 0:
                        Console.WriteLine("Goodbye!");
                        break;

                    default:
                        Console.WriteLine("Please choose a valid option from 0 to 4.");
                        break;
                }
            } while (choice != 0);
        }
    }




    // create connection with database
    static SQLiteConnection CreateConnection()
    {
        SQLiteConnection sqliteConnection = new SQLiteConnection("Data Source=market.db; Version = 3; New = True; Compress = True");
        try
        {
            if (sqliteConnection.State == System.Data.ConnectionState.Open)
                return sqliteConnection;    
            sqliteConnection.Open();
        }
        catch (Exception ex) 
        {
            Console.WriteLine($"Connection Error: {ex.Message}");
        }
        return sqliteConnection;
    }



    // create table or open it, if it exist
    static void CreateTable(SQLiteConnection connection)
    {
        string SQLQueryCreateTabel = @"CREATE TABLE IF NOT EXISTS Product(Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                                                          Name VARCHAR(20) NOT NULL,
                                                                          Quantity Real,
                                                                          Price Real NOT NULL)";
        using (SQLiteCommand command = connection.CreateCommand())
        {
            command.CommandText = SQLQueryCreateTabel;
            command.ExecuteNonQuery();
        }
    }



    // crud operation
    // 1. Create
    static void Create(SQLiteConnection connection, string name, float price)
    {
        try
        {
                using (SQLiteCommand command = connection.CreateCommand())
            {
                //    With using	                    Without using
                //  Automatically cleans up           You must manually call .Dispose()
                //  Safer, less error-prone	          Risk of memory or resource leaks
                //  Preferred in 99% of cases	      Only skip in special scenarios
                command.CommandText = "INSERT INTO Product(Name, Price) VALUES (@name, @price)";
                //command.CommandText = "INSERT INTO Product(Id, Name, Quantity, Price) VALUES (@id, @name, @quantity, @price)";
                //command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@name", name);
                //command.Parameters.AddWithValue("@quantity", quantity);
                command.Parameters.AddWithValue("@price", price);
                command.ExecuteNonQuery();
            }
            Console.WriteLine($"Data Inserted Successfully: the product is {name}: {price}$ per unit");
        }
        catch (SQLiteException ex)
        {
            Console.WriteLine($"Insert Data Error : {ex.Message}");
        }

    }



    // 2. read all table rows
    static void ReadAll(SQLiteConnection connection)
    {
        using (SQLiteCommand command = connection.CreateCommand())
        {
            command.CommandText = "SELECT Id, Name, Price FROM Product";
            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                Console.WriteLine("All Products:");
                Console.WriteLine("{id} - {name} - {price}");
                //Console.WriteLine("{id} - {name} - {quantity} - {price}");
                while(reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string name = reader.GetString(1);
                    //int quantity = reader.GetInt32(2);
                    float price = reader.GetFloat(3);
                    Console.WriteLine($" {id}   - {name}  - {price}");
                }
            }
        }
    }



    // 3. update product
    static void UpdateByName(SQLiteConnection connection)
    {
        Console.Write("Enter the Product Name to Update : ");
        string name = Console.ReadLine();

        Console.Write("Enter the New Price : ");
        if (!float.TryParse(Console.ReadLine(), out float newPrice))
        {
            Console.WriteLine("Invalid price.");
            return;
        }
        try
        {
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "UPDATE Product SET Price = @newPrice WHERE NAME = @name";
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@newPrice", newPrice);
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                    Console.WriteLine($" Product '{name}' updated to new price: {newPrice}");
                else
                    Console.WriteLine($" No product found with name '{name}'.");
            }
        }
        catch (SQLiteException ex)
        {
            Console.WriteLine($"Update Error: {ex.Message}");
        }
    }

    static void UpdateByID(SQLiteConnection connection)
    {
        Console.Write("Enter the Product ID to Update: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("invalid id");
            return;
        }
        Console.Write("Enter the New Price:");
        if (!int.TryParse(Console.ReadLine(), out int newPrice))
        {
            Console.WriteLine("Invalid Price");
            return;
        }

        try
        {
            using(SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "UPDATE Product SET Price = @newPrice WHERE Id = @id";
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@newPrice", newPrice);
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                    Console.WriteLine($" Product with ID {id} updated to new price: {newPrice}");
                else
                    Console.WriteLine($" No product found with ID {id}.");
            }
        }
        catch (SQLiteException e)
        {
            Console.WriteLine($"Update Error: {e.Message}");
        }
    }

    // 4. delete product
    static void DeleteByName(SQLiteConnection connection)
    {
        Console.Write("Enter the Product Name to delete: ");
        string name = Console.ReadLine();

        try
        {
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM Product WHERE NAME = @name";
                command.Parameters.AddWithValue("@name", name);
                int rowsDeleted = command.ExecuteNonQuery();

                if (rowsDeleted > 0)
                    Console.WriteLine($" Product '{name}' deleted successfully.");
                else
                    Console.WriteLine($" No product found with name '{name}'.");
            }
        }
        catch (SQLiteException ex)
        {
            Console.WriteLine($"Delete Error: {ex.Message}");
        }
    }



    static void DeleteByID(SQLiteConnection connection)
    {
        Console.Write("Enter the Product ID to delete: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID.");
            return;
        }

        try
        {
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM AllProducts WHERE Id = @id";
                command.Parameters.AddWithValue("@id", id);
                int rowsDeleted = command.ExecuteNonQuery();

                if (rowsDeleted > 0)
                    Console.WriteLine($" Product with ID {id} deleted successfully.");
                else
                    Console.WriteLine($" No product found with ID {id}.");
            }
        }
        catch (SQLiteException e)
        {
            Console.WriteLine($"Delete Error: {e.Message}");
        }
    }













    // 3. read single product
}