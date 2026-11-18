using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrepMeUp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderSubscriptionExpiresAtIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Orders_SubscriptionId",
                table: "Orders");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_SubscriptionId_ExpiresAt",
                table: "Orders",
                columns: new[] { "SubscriptionId", "ExpiresAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Orders_SubscriptionId_ExpiresAt",
                table: "Orders");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_SubscriptionId",
                table: "Orders",
                column: "SubscriptionId");
        }
    }
}
