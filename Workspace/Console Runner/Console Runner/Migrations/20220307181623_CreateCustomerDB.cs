using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Console_Runner.Migrations
{
    public partial class CreateCustomerDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Email = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Fname = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Lname = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Enabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Password = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Email);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FoodFlags",
                columns: table => new
                {
                    accountEmail = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ingredientID = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodFlags", x => new { x.accountEmail, x.ingredientID });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FoodItems",
                columns: table => new
                {
                    barcode = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    productName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    companyName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodItems", x => x.barcode);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "History",
                columns: table => new
                {
                    email = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    foodItems = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_History", x => new { x.email, x.foodItems });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "IngredientIdentifier",
                columns: table => new
                {
                    barcode = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ingredientID = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IngredientIdentifier", x => new { x.barcode, x.ingredientID });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Ingredients",
                columns: table => new
                {
                    ingredientID = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ingredientName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ingredientDescription = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ingredientShortName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredients", x => x.ingredientID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    Date = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Time = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Message = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => new { x.Date, x.Time, x.Message });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "NutritionLabels",
                columns: table => new
                {
                    barcode = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Calories = table.Column<int>(type: "int", nullable: false),
                    Servings = table.Column<int>(type: "int", nullable: false),
                    ServingSize = table.Column<double>(type: "double", nullable: false),
                    TotalFat = table.Column<int>(type: "int", nullable: false),
                    SaturatedFat = table.Column<int>(type: "int", nullable: false),
                    TransFat = table.Column<int>(type: "int", nullable: false),
                    Cholesterol = table.Column<int>(type: "int", nullable: false),
                    Sodium = table.Column<int>(type: "int", nullable: false),
                    TotalCarbohydrate = table.Column<int>(type: "int", nullable: false),
                    DietaryFiber = table.Column<int>(type: "int", nullable: false),
                    TotalSugars = table.Column<int>(type: "int", nullable: false),
                    AddedSugar = table.Column<int>(type: "int", nullable: false),
                    Protein = table.Column<int>(type: "int", nullable: false),
                    Biotin = table.Column<double>(type: "double", nullable: false),
                    Choline = table.Column<double>(type: "double", nullable: false),
                    Folate = table.Column<double>(type: "double", nullable: false),
                    Niacin = table.Column<double>(type: "double", nullable: false),
                    PantothenicAcid = table.Column<double>(type: "double", nullable: false),
                    Riboflavin = table.Column<double>(type: "double", nullable: false),
                    Thiamin = table.Column<double>(type: "double", nullable: false),
                    A = table.Column<double>(type: "double", nullable: false),
                    B6 = table.Column<double>(type: "double", nullable: false),
                    B12 = table.Column<double>(type: "double", nullable: false),
                    C = table.Column<double>(type: "double", nullable: false),
                    D = table.Column<double>(type: "double", nullable: false),
                    E = table.Column<double>(type: "double", nullable: false),
                    K = table.Column<double>(type: "double", nullable: false),
                    Calcium = table.Column<double>(type: "double", nullable: false),
                    Chloride = table.Column<double>(type: "double", nullable: false),
                    Chromium = table.Column<double>(type: "double", nullable: false),
                    Copper = table.Column<double>(type: "double", nullable: false),
                    Iodine = table.Column<double>(type: "double", nullable: false),
                    Iron = table.Column<double>(type: "double", nullable: false),
                    Magnesium = table.Column<double>(type: "double", nullable: false),
                    Manganese = table.Column<double>(type: "double", nullable: false),
                    Molybdenum = table.Column<double>(type: "double", nullable: false),
                    Phosphorus = table.Column<double>(type: "double", nullable: false),
                    Potassium = table.Column<double>(type: "double", nullable: false),
                    Selenium = table.Column<double>(type: "double", nullable: false),
                    Zinc = table.Column<double>(type: "double", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NutritionLabels", x => x.barcode);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Email = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Resource = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => new { x.Email, x.Resource });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Vitamins",
                columns: table => new
                {
                    barcode = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    vitaminName = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vitamins", x => new { x.barcode, x.vitaminName });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AMRs",
                columns: table => new
                {
                    AccountEmail = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsMale = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Weight = table.Column<int>(type: "int", nullable: false),
                    Height = table.Column<float>(type: "float", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Activity = table.Column<int>(type: "int", nullable: false),
                    IsCustomAMR = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CustomAMR = table.Column<float>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AMRs", x => x.AccountEmail);
                    table.ForeignKey(
                        name: "FK_AMRs_Accounts_AccountEmail",
                        column: x => x.AccountEmail,
                        principalTable: "Accounts",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AMRs");

            migrationBuilder.DropTable(
                name: "FoodFlags");

            migrationBuilder.DropTable(
                name: "FoodItems");

            migrationBuilder.DropTable(
                name: "History");

            migrationBuilder.DropTable(
                name: "IngredientIdentifier");

            migrationBuilder.DropTable(
                name: "Ingredients");

            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DropTable(
                name: "NutritionLabels");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "Vitamins");

            migrationBuilder.DropTable(
                name: "Accounts");
        }
    }
}
