using Microsoft.Data.SqlClient;

namespace UserManagement.Application.Interfaces
{
    public interface IDatabaseClient
    {
        SqlConnection CreateConnection();
    }
}
