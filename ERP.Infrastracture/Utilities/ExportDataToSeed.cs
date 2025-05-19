using System.Reflection;
using Domain.Account.DBConfiguration.DbContext;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Domain.Account.Utility;

public class ExportDataToSeed
{
    private readonly string _connectionString;
    private readonly IApplicationDbContext _context;

    public ExportDataToSeed(IConfiguration configuration, IApplicationDbContext context)
    {
        _connectionString = configuration.GetConnectionString("DefaultDbConnection");
        _context = context;
    }

    public async Task ExportAllTablesToJsonAsync(string outputDirectory = "account")
    {
        Directory.CreateDirectory(@$"seeding/{outputDirectory}");
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var tables = await GetTableNamesAsync(connection);

            foreach (var table in tables)
            {
                var tableData = await GetTableDataAsync(connection, table);
                var json = JsonConvert.SerializeObject(tableData, Formatting.Indented);
                await System.IO.File.WriteAllTextAsync(
                    Path.Combine("seeding", Path.Combine(outputDirectory, $"{table}.json")), json);
            }
        }
    }

    private async Task<List<string>> GetTableNamesAsync(SqlConnection connection)
    {
        var tableNames = new List<string>();

        var query =
            "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_CATALOG = @database";
        using (var command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@database", connection.Database);
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    tableNames.Add(reader.GetString(0));
                }
            }
        }

        return tableNames.Where(e => e != "__EFMigrationsHistory").ToList();
    }

    private async Task<List<Dictionary<string, object>>> GetTableDataAsync(SqlConnection connection, string tableName)
    {
        var tableData = new List<Dictionary<string, object>>();


        var query = $"SELECT * FROM {tableName}";
        using (var command = new SqlCommand(query, connection))
        using (var reader = await command.ExecuteReaderAsync())
        {
            var columnNames = new List<string>();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                columnNames.Add(reader.GetName(i));
            }

            while (await reader.ReadAsync())
            {
                var row = new Dictionary<string, object>();
                foreach (var columnName in columnNames)
                {
                    row[columnName] = reader[columnName];
                }

                tableData.Add(row);
            }
        }

        return tableData;
    }


    public async void InsertIntoTable(string tableName, List<object> entity)
    {
        var files = Directory.GetFiles("seeding/account", "*.json").Select(e => e)
            .ToList();

            var json = await File.ReadAllTextAsync(files.FirstOrDefault());
            List<Dictionary<string, object>> dictionaryList = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(json);

            // Get the Type object of the class using the class name
            Type type = Type.GetType("AccountGuide");

            if (type != null)
            {
                // Create an empty list to hold objects of MyClass
                List<object> objectList = new List<object>();

                // Loop through each dictionary in the list
                foreach (var dictionary in dictionaryList)
                {
                    // Create an instance of the class dynamically
                    object instance = Activator.CreateInstance(type);

                    // Populate the properties of the instance using reflection
                    foreach (var kvp in dictionary)
                    {
                        PropertyInfo property = type.GetProperty(kvp.Key);
                        if (property != null && property.CanWrite)
                        {
                            // Convert the value to the correct type if necessary
                            object value = Convert.ChangeType(kvp.Value, property.PropertyType);
                            property.SetValue(instance, value);
                        }
                    }

                    // Add the populated instance to the list
                    objectList.Add(instance);
                }

                // Now objectList contains a list of MyClass objects
                foreach (var obj in objectList)
                {
                    object myObj = Convert.ChangeType(obj, type);
                }
            }
            else
            {
                Console.WriteLine("Class not found!");
            }
            var tableData = JsonConvert.DeserializeObject<Object[]>(json);
            var t = tableData.GetType();
       
            // Get the DbSet property dynamically based on the table name
            var dbSetProperty = _context.GetType().GetProperty(Path.GetFileNameWithoutExtension(files.FirstOrDefault()));
            if (dbSetProperty == null)
            {
                throw new ArgumentException($"Table {tableName} does not exist in the context.");
            }

            // Get the DbSet instance
            var dbSet = dbSetProperty.GetValue(_context);

            // Use reflection to call the Add method on the DbSet
            var addMethod = dbSet.GetType().GetMethod("Add");
            if (addMethod == null)
            {
                throw new InvalidOperationException("Add method not found on DbSet.");
            }
            addMethod.Invoke(dbSet, tableData);

            await _context.SaveChangesAsync();
        
    }
}