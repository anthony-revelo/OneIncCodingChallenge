using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using UserManagement.Application.Interfaces;
using UserManagement.Core.Entities;
using UserManagement.Infrastructure.Validation;

namespace UserManagement.Infrastructure.Data
{
    public class DatabaseClient : IDatabaseClient
    {
        private readonly IOptionsMonitor<DatabaseSettings> _databaseSettings;

        public DatabaseClient(IOptionsMonitor<DatabaseSettings> databaseSettings)
        {
            _databaseSettings = databaseSettings;
        }

        public SqlConnection CreateConnection()
        {
            return new SqlConnection(_databaseSettings.CurrentValue.DefaultConnection);
        }
    }
}
