using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Microsoft.Data.SqlClient;

namespace delivery_management_systeem.DALS
{
    public abstract class BaseDAL
    {
        private readonly string _connectionString = @"Server=tcp:sql.bsite.net\MSSQL2016;Database=coldfire0412_MatrixInc;User ID=coldfire0412_MatrixInc;Password=4LZC#jz5wCk^3kY;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;";


        protected IDbConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}

