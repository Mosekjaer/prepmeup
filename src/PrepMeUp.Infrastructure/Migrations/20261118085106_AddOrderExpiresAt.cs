using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrepMeUp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderExpiresAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ExpiresAt",
                table: "Orders",
                type: "datetimeoffset",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpiresAt",
                table: "Orders");
        }
    }
}
