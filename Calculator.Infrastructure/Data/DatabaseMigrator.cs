using System.Data.SqlClient;

namespace Calculator.Infrastructure.Data
{
    public class DatabaseMigrator
    {
        private readonly string _connectionString;

        public DatabaseMigrator(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async void Migrate()
        {
            var scripts = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.sql");

            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            foreach (var scriptPath in scripts)
            {
                var script = File.ReadAllText(scriptPath);
                var batches = script.Split(new[] { "GO" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var batch in batches)
                {
                    using var command = new SqlCommand(batch, connection);
                    await command.ExecuteNonQueryAsync();
                }

            }
        }
    }
}
