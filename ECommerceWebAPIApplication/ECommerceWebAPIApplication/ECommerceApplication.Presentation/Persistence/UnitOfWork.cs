using ECommerceApplication.Domain.Models;
using ECommerceApplication.Infrastructure.Persistence.DataContext;
using ECommerceApplication.Infrastructure.Persistence.Repository;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ECommerceApplication.Infrastructure.Persistence.UnitOfWork;

namespace ECommerceApplication.Infrastructure.Persistence
{

    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbName _dbName;
        private bool _disposed;
        private readonly DapperDataContext _dapperDataContext;

        public ICustomerRepository Customers { get; private set; }

        public UnitOfWork(DapperDataContext dapperDataContext, IOptions<DbName> dbName)
        {
            _dapperDataContext = dapperDataContext;
            _dbName = dbName.Value;
            Init();
        }

        private void Init()
        {
            Customers = new CustomerRepository(_dbName,_dapperDataContext);
        }


        public void BeginTransaction()
        {
            
                    _dapperDataContext.DefaultConnection?.Open();
                    _dapperDataContext.Transaction = _dapperDataContext.DefaultConnection?.BeginTransaction();
                 
        }

        public void Commit()
        {
            _dapperDataContext.Transaction?.Commit();
            _dapperDataContext.Transaction?.Dispose();
            _dapperDataContext.Transaction = null;
        }

        public void CommitAndCloseConnection()
        {
            _dapperDataContext.Transaction?.Commit();
            _dapperDataContext.Transaction?.Dispose();
            _dapperDataContext.Transaction = null;
            _dapperDataContext.DefaultConnection?.Close();
            _dapperDataContext.DefaultConnection?.Dispose();
        }

        public void Rollback()
        {
            _dapperDataContext.Transaction?.Rollback();
            _dapperDataContext.Transaction?.Dispose();
            _dapperDataContext.Transaction = null;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {

                if (disposing)
                {
                    _dapperDataContext.Transaction?.Dispose();
                    _dapperDataContext.DefaultConnection?.Dispose();
                }
                _disposed = true;
            }

        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

}
