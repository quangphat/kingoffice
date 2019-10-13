using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Repository.Classes
{
    public abstract class BaseRepository
    {
        protected IDbConnection connection;
        public BaseRepository(IConfiguration configuration)
        {
            connection = new SqlConnection(configuration.GetConnectionString("kingoffice"));
        }
    }
}
