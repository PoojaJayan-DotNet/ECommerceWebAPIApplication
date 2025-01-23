using ECommerceApplication.Infrastructure.Persistence.DataContext;
using ECommerceApplication.Infrastructure.Persistence.Repository;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApplication.Infrastructure.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<DapperDataContext>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();

            return services;
        }
    }

}
