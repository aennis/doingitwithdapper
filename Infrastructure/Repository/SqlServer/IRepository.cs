using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Infrastructure.Repository.SqlServer
{
    public interface ISQLRepository
    {
        Task<T> WithConnection<T>(Func<IDbConnection, Task<T>> getData);
        SqlConnection GetConnection(bool multipleActiveResultSets = false);
    }
}
