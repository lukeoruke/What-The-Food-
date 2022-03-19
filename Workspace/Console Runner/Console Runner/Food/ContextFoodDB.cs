using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
namespace Console_Runner.FoodService;

/*will run on startup, will configure the services to our database context
*/
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    => services.AddDbContext<ContextFoodDB>();

}

public class ContextFoodDB : DbContext
{
    /// <summary>
    /// Creates database to store all information regarding Food
    /// </summary>
    /// <param name="optionsBuilder"></param>
    /// Specfic configuration made for the FoodData base
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql("server = localhost; user = root; password = myPassword; Database = FoodDB",
            new MySqlServerVersion(new Version(8, 0, 27)));
    }
    /// <summary>
    /// Assigns primary key within each table of Food Database
    /// </summary>
    /// <param name="builder"></param>
    /// 
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
        builder.Entity<Ingredient>().HasKey(table => new
        {
            table.IngredientID,
        });
        builder.Entity<NutritionLabel>().HasKey(table => new
        {
            table.Barcode
        });
        builder.Entity<LabelNutrient>().HasKey(table => new
        {
            table.Barcode,
            table.NutrientID
        });
        builder.Entity<Nutrient>().HasKey(table => new
        {
            //table.NID
        });

    }
    public DbSet<Ingredient> Ingredients { get; set; } = null!;

    public DbSet<FoodItem> FoodItem { get; set; } = null!;
    public DbSet<LabelIngredient> LabelIngredients { get; set; } = null!;
    public DbSet<LabelNutrient> LabelNutrients { get; set; } = null!;
    public DbSet<Nutrient> Nutrient { get; set; } = null!;
    public DbSet<NutritionLabel> NutritionLabel { get; set; } = null!;





}
