﻿// <auto-generated />
using Console_Runner.AccountService;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Console_Runner.Migrations
{
    [DbContext(typeof(ContextAccountDB))]
    [Migration("20220328213718_AccountDB")]
    partial class AccountDB
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Console_Runner.AccountService.Account", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("Enabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("FName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("LName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("salt")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("UserID");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("Console_Runner.AccountService.AMR", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

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

                    b.HasKey("UserID");

                    b.ToTable("AMRs");
                });

            modelBuilder.Entity("Console_Runner.AccountService.Authorization", b =>
                {
                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.Property<string>("Permission")
                        .HasColumnType("varchar(255)");

                    b.HasKey("UserID", "Permission");

                    b.ToTable("Authorizations");
                });

            modelBuilder.Entity("Console_Runner.AccountService.FoodFlag", b =>
                {
                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.Property<int>("IngredientID")
                        .HasColumnType("int");

                    b.HasKey("UserID", "IngredientID");

                    b.ToTable("FoodFlags");
                });

            modelBuilder.Entity("Console_Runner.AccountService.History", b =>
                {
                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.Property<string>("Barcode")
                        .HasColumnType("varchar(255)");

                    b.Property<long>("Timestamp")
                        .HasColumnType("bigint");

                    b.HasKey("UserID", "Barcode");

                    b.ToTable("Historys");
                });
#pragma warning restore 612, 618
        }
    }
}
