using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.ChangeTracking;

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
            table.NutrientID
        });
        builder.Entity<Review>().HasKey(table => new
        {
            table.Barcode,
            table.UserID
        });
        builder.Entity<FoodUpdate>().HasKey(table => new
        {
            table.FoodItemId,
            table.UpdateTime
        });
        builder.Entity<FoodUpdate>().HasDiscriminator<string>("update_type")
            .HasValue<FoodRecall>("foodrecall")
            .HasValue<FoodIngredientChange>("ingredientchange");
        builder.Entity<IngredientUpdateList>().HasKey(table => new
        {
            table.FoodIngredientChangeId
        });
        builder
            .Entity<FoodRecall>()
            .Property(foodRecall => foodRecall.Locations)
            .HasConversion(
                strList => JsonSerializer.Serialize(strList, new JsonSerializerOptions(JsonSerializerDefaults.General)),
                jsonStr => JsonSerializer.Deserialize<List<string>>(jsonStr, new JsonSerializerOptions(JsonSerializerDefaults.General)),
                new ValueComparer<List<string>>(
                    (c1, c2) => c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList())
                );
        builder
            .Entity<FoodRecall>()
            .Property(foodRecall => foodRecall.LotNumbers)
            .HasConversion(
                numList => JsonSerializer.Serialize(numList, new JsonSerializerOptions(JsonSerializerDefaults.General)),
                jsonStr => JsonSerializer.Deserialize<List<int>>(jsonStr, new JsonSerializerOptions(JsonSerializerDefaults.General)),
                new ValueComparer<List<int>>(
                    (c1, c2) => c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()));
        builder
            .Entity<FoodRecall>()
            .Property(foodRecall => foodRecall.ExpirationDates)
            .HasConversion(
                dateList => JsonSerializer.Serialize(dateList, new JsonSerializerOptions(JsonSerializerDefaults.General)),
                jsonStr => JsonSerializer.Deserialize<List<DateTime>>(jsonStr, new JsonSerializerOptions(JsonSerializerDefaults.General)),
                new ValueComparer<List<DateTime>>(
                    (c1, c2) => c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()));
    }
    public DbSet<Ingredient> Ingredients { get; set; } = null!;
    public DbSet<FoodItem> FoodItem { get; set; } = null!;
    public DbSet<LabelIngredient> LabelIngredients { get; set; } = null!;
    public DbSet<LabelNutrient> LabelNutrients { get; set; } = null!;
    public DbSet<Nutrient> Nutrient { get; set; } = null!;
    public DbSet<NutritionLabel> NutritionLabel { get; set; } = null!;
    public DbSet<FoodUpdate> FoodUpdates { get; set; } = null!;
    public DbSet<IngredientUpdateList> IngredientLists { get; set; } = null!;
}
