using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
namespace Console_Runner.Food
{
    /*will run on startup, will configure the services to our database context
 */
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        => services.AddDbContext<ContextFoodDB>();

    }

    public class ContextFoodDB : DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server = localhost; user = root; password = myPassword; Database = FoodDB",
                new MySqlServerVersion(new Version(8, 0, 27)));
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<FoodItem>().HasKey(table => new
            {
                table.Barcode
            });
            builder.Entity<LabelIngredient>().HasKey(table => new
            {
                table.Barcode,
                table.IngredientID
            });
        }


    }
}
