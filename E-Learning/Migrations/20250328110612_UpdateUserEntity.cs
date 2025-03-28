using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Learning.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "bio",
                table: "users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "certificate",
                table: "users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "cv_url",
                table: "users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "expertise",
                table: "users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "facebook_link",
                table: "users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "otp",
                table: "users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "otp_expiry_date",
                table: "users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "points",
                table: "users",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<double>(
                name: "years_of_experience",
                table: "users",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "zip_code",
                table: "users",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "bio",
                table: "users");

            migrationBuilder.DropColumn(
                name: "certificate",
                table: "users");

            migrationBuilder.DropColumn(
                name: "cv_url",
                table: "users");

            migrationBuilder.DropColumn(
                name: "expertise",
                table: "users");

            migrationBuilder.DropColumn(
                name: "facebook_link",
                table: "users");

            migrationBuilder.DropColumn(
                name: "otp",
                table: "users");

            migrationBuilder.DropColumn(
                name: "otp_expiry_date",
                table: "users");

            migrationBuilder.DropColumn(
                name: "points",
                table: "users");

            migrationBuilder.DropColumn(
                name: "years_of_experience",
                table: "users");

            migrationBuilder.DropColumn(
                name: "zip_code",
                table: "users");
        }
    }
}
