using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;


namespace Console_Runner.AccountService
{
    /*will run on startup, will configure the services to our database context*/

    public class Startup 
    {
        public void ConfigureServices(IServiceCollection services)
        => services.AddDbContext<ContextAccountDB>();
    }
    internal class ContextAccountDB : DbContext
    {
        /// <summary>
        /// Creates database to store all information regarding Accont services
        /// </summary>
        /// <param name="optionsBuilder"></param>
        /// Specfic configuration made for the Account Database
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server = localhost; user = root; password = myPassword; Database = AccountDB",
                new MySqlServerVersion(new Version(8, 0, 27)));
        }
        /// <summary>
        /// Assigns primary key within each table of Food Database
        /// </summary>
        /// <param name="builder"></param>
        /// 
        protected override void OnModelCreating(ModelBuilder builder) 
        {
            builder.Entity<Account>().HasKey(table => new
            {
                table.UserID
            });

            //TODO: Apply authorization

            builder.Entity<Authorization>().HasKey(table => new
            {
                table.UserID,
                table.Permission
            });

            builder.Entity<FoodFlag>().HasKey(table => new
            {
                table.UserID,
                table.IngredientID
            });

            builder.Entity<History>().HasKey(table => new
            {
                table.UserID,
                table.Barcode
            });;

            builder.Entity<AMR>().HasKey(table => new
            {
                table.UserID
            });
            builder.Entity<ActiveSessionTracker>().HasKey(table => new
            {

                table.jwt
            });
        }
        public DbSet<Account> Accounts { get; set; } = null!;

        public DbSet<FoodFlag> FoodFlags { get; set; } = null!;
        public DbSet<History> Historys { get; set; } = null!;
        public DbSet<AMR> AMRs { get; set; } = null!;
        public DbSet<ActiveSessionTracker> ActiveSessionTracker { get; set; } = null!;
        public DbSet<Authorization> Authorizations { get; set; } = null!;

    }
}
