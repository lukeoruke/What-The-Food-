﻿// <auto-generated />
using Console_Runner.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Console_Runner.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20220307181623_CreateCustomerDB")]
    partial class CreateCustomerDB
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Console_Runner.AMRModel.AMR", b =>
                {
                    b.Property<string>("AccountEmail")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("Activity")
                        .HasColumnType("int");

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<float>("CustomAMR")
                        .HasColumnType("float");

                    b.Property<float>("Height")
                        .HasColumnType("float");

                    b.Property<bool>("IsCustomAMR")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsMale")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("Weight")
                        .HasColumnType("int");

                    b.HasKey("AccountEmail");

                    b.ToTable("AMRs");
                });

            modelBuilder.Entity("Console_Runner.Food.FoodFlag", b =>
                {
                    b.Property<string>("accountEmail")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ingredientID")
                        .HasColumnType("varchar(255)");

                    b.HasKey("accountEmail", "ingredientID");

                    b.ToTable("FoodFlags");
                });

            modelBuilder.Entity("Console_Runner.Food.FoodItem", b =>
                {
                    b.Property<string>("barcode")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("companyName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("productName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("barcode");

                    b.ToTable("FoodItems");
                });

            modelBuilder.Entity("Console_Runner.Food.Ingredient", b =>
                {
                    b.Property<string>("ingredientID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ingredientDescription")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("ingredientName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("ingredientShortName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("ingredientID");

                    b.ToTable("Ingredients");
                });

            modelBuilder.Entity("Console_Runner.Food.NutritionLabel", b =>
                {
                    b.Property<string>("barcode")
                        .HasColumnType("varchar(255)");

                    b.Property<double>("A")
                        .HasColumnType("double");

                    b.Property<int>("AddedSugar")
                        .HasColumnType("int");

                    b.Property<double>("B12")
                        .HasColumnType("double");

                    b.Property<double>("B6")
                        .HasColumnType("double");

                    b.Property<double>("Biotin")
                        .HasColumnType("double");

                    b.Property<double>("C")
                        .HasColumnType("double");

                    b.Property<double>("Calcium")
                        .HasColumnType("double");

                    b.Property<int>("Calories")
                        .HasColumnType("int");

                    b.Property<double>("Chloride")
                        .HasColumnType("double");

                    b.Property<int>("Cholesterol")
                        .HasColumnType("int");

                    b.Property<double>("Choline")
                        .HasColumnType("double");

                    b.Property<double>("Chromium")
                        .HasColumnType("double");

                    b.Property<double>("Copper")
                        .HasColumnType("double");

                    b.Property<double>("D")
                        .HasColumnType("double");

                    b.Property<int>("DietaryFiber")
                        .HasColumnType("int");

                    b.Property<double>("E")
                        .HasColumnType("double");

                    b.Property<double>("Folate")
                        .HasColumnType("double");

                    b.Property<double>("Iodine")
                        .HasColumnType("double");

                    b.Property<double>("Iron")
                        .HasColumnType("double");

                    b.Property<double>("K")
                        .HasColumnType("double");

                    b.Property<double>("Magnesium")
                        .HasColumnType("double");

                    b.Property<double>("Manganese")
                        .HasColumnType("double");

                    b.Property<double>("Molybdenum")
                        .HasColumnType("double");

                    b.Property<double>("Niacin")
                        .HasColumnType("double");

                    b.Property<double>("PantothenicAcid")
                        .HasColumnType("double");

                    b.Property<double>("Phosphorus")
                        .HasColumnType("double");

                    b.Property<double>("Potassium")
                        .HasColumnType("double");

                    b.Property<int>("Protein")
                        .HasColumnType("int");

                    b.Property<double>("Riboflavin")
                        .HasColumnType("double");

                    b.Property<int>("SaturatedFat")
                        .HasColumnType("int");

                    b.Property<double>("Selenium")
                        .HasColumnType("double");

                    b.Property<double>("ServingSize")
                        .HasColumnType("double");

                    b.Property<int>("Servings")
                        .HasColumnType("int");

                    b.Property<int>("Sodium")
                        .HasColumnType("int");

                    b.Property<double>("Thiamin")
                        .HasColumnType("double");

                    b.Property<int>("TotalCarbohydrate")
                        .HasColumnType("int");

                    b.Property<int>("TotalFat")
                        .HasColumnType("int");

                    b.Property<int>("TotalSugars")
                        .HasColumnType("int");

                    b.Property<int>("TransFat")
                        .HasColumnType("int");

                    b.Property<double>("Zinc")
                        .HasColumnType("double");

                    b.HasKey("barcode");

                    b.ToTable("NutritionLabels");
                });

            modelBuilder.Entity("Console_Runner.Food.Vitamins", b =>
                {
                    b.Property<string>("barcode")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("vitaminName")
                        .HasColumnType("varchar(255)");

                    b.HasKey("barcode", "vitaminName");

                    b.ToTable("Vitamins");
                });

            modelBuilder.Entity("Console_Runner.History", b =>
                {
                    b.Property<string>("email")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("foodItems")
                        .HasColumnType("varchar(255)");

                    b.HasKey("email", "foodItems");

                    b.ToTable("History");
                });

            modelBuilder.Entity("Console_Runner.Logging.Logs", b =>
                {
                    b.Property<string>("Date")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Time")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Message")
                        .HasColumnType("varchar(255)");

                    b.HasKey("Date", "Time", "Message");

                    b.ToTable("Logs");
                });

            modelBuilder.Entity("Console_Runner.User_Management.Account", b =>
                {
                    b.Property<string>("Email")
                        .HasColumnType("varchar(255)");

                    b.Property<bool>("Enabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Fname")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Lname")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Email");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("Console_Runner.User_Management.Permission", b =>
                {
                    b.Property<string>("Email")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Resource")
                        .HasColumnType("varchar(255)");

                    b.HasKey("Email", "Resource");

                    b.ToTable("Permissions");
                });

            modelBuilder.Entity("Food_Class_Library.LabelIdentifyer", b =>
                {
                    b.Property<string>("barcode")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ingredientID")
                        .HasColumnType("varchar(255)");

                    b.HasKey("barcode", "ingredientID");

                    b.ToTable("IngredientIdentifier");
                });

            modelBuilder.Entity("Console_Runner.AMRModel.AMR", b =>
                {
                    b.HasOne("Console_Runner.User_Management.Account", "Account")
                        .WithOne("AMR")
                        .HasForeignKey("Console_Runner.AMRModel.AMR", "AccountEmail")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("Console_Runner.User_Management.Account", b =>
                {
                    b.Navigation("AMR");
                });
#pragma warning restore 612, 618
        }
    }
}