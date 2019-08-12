using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repository.DbConnection
{
    public interface IConnection
    {
        string ConnectionString { get; }
    }
}
