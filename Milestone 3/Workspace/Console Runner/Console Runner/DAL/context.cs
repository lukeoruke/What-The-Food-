using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User;
using Pomelo.EntityFrameworkCore.MySql;
using Microsoft.Extensions.DependencyInjection;
using Logger;
using Console_Runner;

namespace Class1
{
    /*will run on startup, will configure the services to our database context
     */
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        => services.AddDbContext<Context>();
        
    }

    /*
     * Will run when being configured and it will connect to our database
     * Currently only configures the account table
     */
    public class Context : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server = localhost; user = root; password = myPassword; Database = ef",
                new MySqlServerVersion(new Version(8, 0, 27)));
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Logs>().HasKey(table => new
            {
                table.Date,
                table.Time,
                table.toLog
            });
            builder.Entity<user_permissions>().HasKey(table => new
            {
                table.email,
                table.permission
            });
        }
        public DbSet<Account> accounts { get; set; }
        public DbSet<Logs> logs { get; set; }

        public DbSet<user_permissions> permissions{get; set;}

    }
}