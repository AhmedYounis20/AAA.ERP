using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace AAA.ERP.Utility;

public class ExportDataToSeed
{
    private readonly string _connectionString;

    public ExportDataToSeed(IConfiguration configuration)
        => _connectionString = configuration.GetConnectionString("DefaultDbConnection");

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
}