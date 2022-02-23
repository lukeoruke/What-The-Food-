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
using Console_Runner.Food;
using Food_Class_Library;

//TODO: ADD FK TO CLASSES! (MQ)
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

            //Sets a composite key of email+permission for the permissions table.
            builder.Entity<user_permissions>().HasKey(table => new
            {
                table.email,
                table.permission
            });
            builder.Entity<History>().HasKey(table => new
            {
                table.email,
                table.foodItems
            });
            builder.Entity<Label_FoodItem>().HasKey(table => new
            {
                table.labelID,
                table.barcode
            });
            builder.Entity<Vitamins>().HasKey(table => new
            {
                table.labelID,
                table.vitaminName
            }) ;
            builder.Entity<Ingredient>().HasKey(table => new
            {
                table.labelID,
                table.ingredientName
            });
            builder.Entity<NutritionLabel>().HasKey(table => new
            {
                table.labelID
            });
        }
        public DbSet<Vitamins> vitamins { get; set;}

        public DbSet<FoodItem> foodItems { get; set; }

        public DbSet<Label_FoodItem> label_foodItem { get; set; }

        public DbSet<Ingredient> ingredient { get; set; }

        public DbSet<NutritionLabel> nutritionLabels { get; set; }

        public DbSet<Account> accounts { get; set; }

        public DbSet<History> history { get; set; }

        public DbSet<Logs> logs { get; set; }

        public DbSet<user_permissions> permissions{get; set;}

    }
}