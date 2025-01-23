//using Microsoft.Data.SqlClient;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApplication.Infrastructure.Persistence.DataContext
{
    public sealed class DapperDataContext
    {
        private readonly IConfiguration _configuration;
        private readonly string? _ecommerceDbConnectionString;
        private IDbConnection? _defaultConnection;
        private IDbTransaction? _transaction;
        public DapperDataContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _ecommerceDbConnectionString = configuration.GetConnectionString("EcommerceAppDbConnection");

        }
        public IDbConnection? DefaultConnection
        {
            get
            {
                if (_defaultConnection is null || _defaultConnection.State != ConnectionState.Open)
                    _defaultConnection = new SqlConnection(_ecommerceDbConnectionString);
                return _defaultConnection;
            }
        }
        public IDbTransaction? Transaction
        {
            get
            {
                return _transaction;
            }

            set
            {
                _transaction = value;
            }
        }
    }
}
