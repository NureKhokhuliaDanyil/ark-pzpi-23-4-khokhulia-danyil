using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Washing.Migrations
{
    /// <inheritdoc />
    public partial class ComprehensiveBusinessLogic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DoorLocked",
                table: "WashingSessions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PasswordResetToken",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Cost",
                table: "MaintenanceLogs",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ResolvedAt",
                table: "MaintenanceLogs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LoadPricingConditions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoadThreshold = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Multiplier = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoadPricingConditions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PromoCodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromoCodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TimePricingConditions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TimeStart = table.Column<TimeSpan>(type: "time", nullable: false),
                    TimeEnd = table.Column<TimeSpan>(type: "time", nullable: false),
                    Multiplier = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimePricingConditions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PromoCodes_Code",
                table: "PromoCodes",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoadPricingConditions");

            migrationBuilder.DropTable(
                name: "PromoCodes");

            migrationBuilder.DropTable(
                name: "TimePricingConditions");

            migrationBuilder.DropColumn(
                name: "DoorLocked",
                table: "WashingSessions");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PasswordResetToken",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Cost",
                table: "MaintenanceLogs");

            migrationBuilder.DropColumn(
                name: "ResolvedAt",
                table: "MaintenanceLogs");
        }
    }
}
