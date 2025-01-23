using ECommerceApplication.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApplication.Infrastructure.Persistence
{
    public interface IUnitOfWork : IDisposable
    {
        public ICustomerRepository Customers { get; }
    }
}
