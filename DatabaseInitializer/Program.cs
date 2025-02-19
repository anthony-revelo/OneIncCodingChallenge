using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

class Program
{
    static void Main(string[] args)
    {
        // Build the configuration to get the connection string from appsettings.json
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();

            // Create database if it does not exist
            var createDbCommand = new SqlCommand(@"
                IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'UserManagementDb')
                BEGIN
                    CREATE DATABASE UserManagementDb;
                END;
            ", connection);

            createDbCommand.ExecuteNonQuery();

            // Close and reopen connection to switch to the new database context
            connection.Close();
            connection.ConnectionString = connectionString.Replace("Database=master;", "Database=UserManagementDb;");
            connection.Open();

            // Create Users table if it does not exist
            var createTableCommand = new SqlCommand(@"
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')
                BEGIN
                    CREATE TABLE Users (
                        Id INT PRIMARY KEY IDENTITY(1,1),
                        FirstName NVARCHAR(128) NOT NULL,
                        LastName NVARCHAR(128),
                        Email NVARCHAR(256) NOT NULL UNIQUE,
                        DateOfBirth DATETIME NOT NULL,
                        PhoneNumber NVARCHAR(10) NOT NULL
                    );
                END;
            ", connection);

            createTableCommand.ExecuteNonQuery();
            Console.WriteLine("Database and table created successfully.");
        }
    }
}
