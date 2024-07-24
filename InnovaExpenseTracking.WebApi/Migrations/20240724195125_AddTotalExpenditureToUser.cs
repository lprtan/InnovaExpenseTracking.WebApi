using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InnovaExpenseTracking.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class AddTotalExpenditureToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TotalExpenditure",
                table: "Users",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalExpenditure",
                table: "Users");
        }
    }
}
