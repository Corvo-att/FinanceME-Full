using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceME.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddBudgetRolloverAlert : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AlertOnThreshold",
                table: "Budgets",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RolloverUnused",
                table: "Budgets",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AlertOnThreshold",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "RolloverUnused",
                table: "Budgets");
        }
    }
}
