using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Learning.Migrations
{
    /// <inheritdoc />
    public partial class Vu_FixSalary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Salaries_payments_PaymentId",
                table: "Salaries");

            migrationBuilder.DropIndex(
                name: "IX_Salaries_PaymentId",
                table: "Salaries");

            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "Salaries");

            migrationBuilder.AddColumn<long>(
                name: "AuthorId",
                table: "Salaries",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Salaries_AuthorId",
                table: "Salaries",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Salaries_users_AuthorId",
                table: "Salaries",
                column: "AuthorId",
                principalTable: "users",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Salaries_users_AuthorId",
                table: "Salaries");

            migrationBuilder.DropIndex(
                name: "IX_Salaries_AuthorId",
                table: "Salaries");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Salaries");

            migrationBuilder.AddColumn<long>(
                name: "PaymentId",
                table: "Salaries",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Salaries_PaymentId",
                table: "Salaries",
                column: "PaymentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Salaries_payments_PaymentId",
                table: "Salaries",
                column: "PaymentId",
                principalTable: "payments",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
