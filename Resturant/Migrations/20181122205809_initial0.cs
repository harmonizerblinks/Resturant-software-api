using Microsoft.EntityFrameworkCore.Migrations;

namespace Resturant.Migrations
{
    public partial class initial0 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "StockLog",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "Stock",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Sales",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Source",
                table: "Order",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "Source",
                table: "Order");

            migrationBuilder.AlterColumn<string>(
                name: "Quantity",
                table: "StockLog",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "Quantity",
                table: "Stock",
                nullable: false,
                oldClrType: typeof(int));
        }
    }
}
