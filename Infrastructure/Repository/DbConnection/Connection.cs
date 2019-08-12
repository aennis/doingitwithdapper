using System;
using System.Configuration;

namespace Infrastructure.Repository.DbConnection
{
    public class Connection : IConnection
    {
        /// <summary>
        /// TODO: change back to strongly typed configuration binding, needed to change to string for .net core quick fix
        /// </summary>
        /// <param name="con"></param>
        public Connection(string con)
        {
            // must use a guard clause to ensure something is injected
            //if (settings == null)
            //    throw new ArgumentNullException("settings", "Connection expects constructor injection for connectionStringSettings param.");

            // we have a value by now so assign it
            ConnectionString = con;
        }

        public string ConnectionString { get; set; }

    }
}
