﻿using Microsoft.EntityFrameworkCore;
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
using Console_Runner.AMRModel;

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
 
            builder.Entity<Vitamins>().HasKey(table => new
            {
                table.barcode,
                table.vitaminName
            }) ;
            builder.Entity<LabelIdentifyer>().HasKey(table => new
            {
                table.barcode,
                table.ingredientID
            });
            builder.Entity<NutritionLabel>().HasKey(table => new
            {
                table.barcode
            });
            builder.Entity<FoodFlag>().HasKey(table => new
            {
                table.accountEmail,
                table.ingredientID
            });
            builder.Entity<Ingredient>().HasKey(table => new
            {
                table.ingredientID
            });
            builder.Entity<FoodItem>().HasKey(table => new
            {
                table.barcode
            });
            builder.Entity<AMR>().HasKey(table => new
            {
                table.AccountEmail
            });

            // one-to-one relationship between account and amr, where an account can exist without an amr but not vice versa
            builder.Entity<Account>()
                .HasOne(ac => ac.AMR)
                .WithOne(am => am.Account)
                .HasForeignKey<AMR>(a => a.AccountEmail);
        }
        // '= null!' - the null-forgiving operator is used to declare that the variable is NOT null, to silence the compiler warning.
        //     Since the DbContext base constructor "ensures that all DbSet properties will get initialized"
        //     (https://docs.microsoft.com/en-us/ef/core/miscellaneous/nullable-reference-types),
        //     this should be safe.
        public DbSet<Ingredient> Ingredients { get; set; } = null!;

        public DbSet<FoodFlag> FoodFlags { get; set; } = null!;

        public DbSet<Vitamins> Vitamins { get; set; } = null!;

        public DbSet<FoodItem> FoodItems { get; set; } = null!;

        public DbSet<LabelIdentifyer> IngredientIdentifier { get; set; } = null!;

        public DbSet<NutritionLabel> NutritionLabels { get; set; } = null!;

        public DbSet<Account> Accounts { get; set; } = null!;

        public DbSet<History> History { get; set; } = null!;

        public DbSet<Logs> Logs { get; set; } = null!;

        public DbSet<user_permissions> Permissions {get; set; } = null!;

        public DbSet<AMR> AMRs { get; set; } = null!;
    }
}