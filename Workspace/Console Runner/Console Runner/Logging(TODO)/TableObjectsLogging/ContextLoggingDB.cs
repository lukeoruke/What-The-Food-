using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Console_Runner.Logging
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        => services.AddDbContext<ContextLoggingDB>();
    }

    public class ContextLoggingDB : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server = localhost; user = root; password = myPassword; Database = LogDB",
                new MySqlServerVersion(new Version(8, 0, 27)));
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Log>().HasKey(table => new
            {
                table.LogId
            });
        }

        public DbSet<Log> Logs { get; set; } = null!;
        public DbSet<UserIdentifier> UIDs { get; set; } = null!;
    }
}
