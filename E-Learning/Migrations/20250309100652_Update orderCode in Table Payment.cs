using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Learning.Migrations
{
    /// <inheritdoc />
    public partial class UpdateorderCodeinTablePayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_payments_users_UserId",
                table: "payments");

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "payments",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "order_code",
                table: "payments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_payments_users_UserId",
                table: "payments",
                column: "UserId",
                principalTable: "users",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_payments_users_UserId",
                table: "payments");

            migrationBuilder.DropColumn(
                name: "order_code",
                table: "payments");

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "payments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_payments_users_UserId",
                table: "payments",
                column: "UserId",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
