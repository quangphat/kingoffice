﻿using Dapper;
using Entity.Infrastructures;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Repository.Classes
{
    public abstract class BaseRepository
    {
        private IDbConnection _connection;
        protected readonly IConfiguration _configuration;

        public BaseRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connection = new SqlConnection(configuration.GetConnectionString("kingoffice"));
        }
        protected IDbConnection GetConnection()
        {
            var con = new SqlConnection(_configuration.GetConnectionString("kingoffice"));
            con.Open();
            return con;
        }
        protected DynamicParameters AddOutputParam(string name, DbType type = DbType.Int32)
        {
            var p = new DynamicParameters();
            p.Add(name, dbType: type, direction: ParameterDirection.Output);
            return p;
        }
    }
}
