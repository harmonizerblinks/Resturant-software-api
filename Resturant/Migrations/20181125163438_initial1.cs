using Microsoft.EntityFrameworkCore.Migrations;

namespace Resturant.Migrations
{
    public partial class initial1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Teller_TellerId",
                table: "Transaction");

            migrationBuilder.AlterColumn<int>(
                name: "TellerId",
                table: "Transaction",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "Transaction",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Reference",
                table: "Sales",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Vat",
                table: "Order",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_OrderId",
                table: "Transaction",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Order_OrderId",
                table: "Transaction",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Teller_TellerId",
                table: "Transaction",
                column: "TellerId",
                principalTable: "Teller",
                principalColumn: "TellerId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Order_OrderId",
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Teller_TellerId",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_OrderId",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "Reference",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "Vat",
                table: "Order");

            migrationBuilder.AlterColumn<int>(
                name: "TellerId",
                table: "Transaction",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Teller_TellerId",
                table: "Transaction",
                column: "TellerId",
                principalTable: "Teller",
                principalColumn: "TellerId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
