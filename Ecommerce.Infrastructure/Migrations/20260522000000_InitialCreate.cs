using System;
using Ecommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.Migrations;

[DbContext(typeof(EcommerceDbContext))]
[Migration("20260522000000_InitialCreate")]
public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Categories",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_Categories", x => x.Id));

        migrationBuilder.CreateTable(
            name: "Users",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                FirstName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                LastName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                Email = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                PasswordHash = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                Role = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_Users", x => x.Id));

        migrationBuilder.CreateTable(
            name: "Products",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                Name = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                Description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                Stock = table.Column<int>(type: "INTEGER", nullable: false),
                CategoryId = table.Column<Guid>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Products", x => x.Id);
                table.ForeignKey("FK_Products_Categories_CategoryId", x => x.CategoryId, "Categories", "Id", onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex("IX_Users_Email", "Users", "Email", unique: true);
        migrationBuilder.CreateIndex("IX_Products_CategoryId", "Products", "CategoryId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable("Products");
        migrationBuilder.DropTable("Users");
        migrationBuilder.DropTable("Categories");
    }
}
