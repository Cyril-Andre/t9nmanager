using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace t9n.DAL.DesignTime
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<t9nDbContext>
    {
        public t9nDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(@Directory.GetCurrentDirectory() + "/../t9n.api/appsettings.json").Build();
            var builder = new DbContextOptionsBuilder<t9nDbContext>();
            var connectionString =  configuration.GetConnectionString("DatabaseConnection");
            builder.UseSqlServer(connectionString);
            return new t9nDbContext(builder.Options);
        }
    }
}
