using Dapper;
using Entity.Infrastructures;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Repository.Classes
{
    public abstract class BaseRepository
    {
        protected IDbConnection connection;
        protected CurrentProcess _process;
        private IConfiguration configuration;

        public BaseRepository(IConfiguration configuration, CurrentProcess process)
        {
            _process = process;
            connection = new SqlConnection(configuration.GetConnectionString("kingoffice"));
        }

        protected BaseRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        protected DynamicParameters AddOutputParam(string name, DbType type = DbType.Int32)
        {
            var p = new DynamicParameters();
            p.Add(name, dbType: type, direction: ParameterDirection.Output);
            return p;
        }
    }
}
