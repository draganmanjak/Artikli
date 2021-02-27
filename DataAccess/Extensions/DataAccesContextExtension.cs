using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Extensions
{
   public static class DataAccesContextExtension
    {
        public static void AddDataAccessContextInfrastructure(this IServiceCollection services, IConfiguration _config)
        {
            if (_config.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<DataAccessContext>(options =>
                    options.UseInMemoryDatabase("IdentityDb"));
            }
            else
            {
                services.AddDbContext<DataAccessContext>(options =>
                options.UseSqlServer(
                    _config.GetConnectionString("IdentityConnection"),
                    b => b.MigrationsAssembly(typeof(DataAccessContext).Assembly.FullName)));
            }
        }
    }
}
