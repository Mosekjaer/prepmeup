using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PrepMeUp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPackageTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Items",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql("UPDATE [Items] SET [Type] = 1 WHERE [Name] IN ('torch', 'radio')");

            migrationBuilder.CreateTable(
                name: "PackageTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackageTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PackageContents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    PackageTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackageContents", x => x.Id);
                    table.CheckConstraint("CK_PackageContents_Quantity", "[Quantity] >= 1");
                    table.ForeignKey(
                        name: "FK_PackageContents_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PackageContents_PackageTypes_PackageTypeId",
                        column: x => x.PackageTypeId,
                        principalTable: "PackageTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "Name", "Type" },
                values: new object[,]
                {
                    { new Guid("7034103d-0cce-40d5-a06a-345b47276ded"), "Water", 0 },
                    { new Guid("8e33e585-870c-4f60-bd6e-995bc86ef2c6"), "Crispbread", 0 },
                    { new Guid("cd127a73-703c-4717-88bf-7726dd09944c"), "Canned food", 0 }
                });

            migrationBuilder.InsertData(
                table: "PackageTypes",
                columns: new[] { "Id", "Name", "Status" },
                values: new object[,]
                {
                    { new Guid("52f89479-6e11-4698-8602-e7cbe63004ec"), "Normal - Child", 0 },
                    { new Guid("dbd55106-cbca-4197-bf2b-e1b21b36e7bd"), "Normal - Adult", 0 }
                });

            migrationBuilder.InsertData(
                table: "PackageContents",
                columns: new[] { "Id", "ItemId", "PackageTypeId", "Quantity" },
                values: new object[,]
                {
                    { new Guid("257ad596-d194-4b4a-a713-465cc4da0155"), new Guid("cd127a73-703c-4717-88bf-7726dd09944c"), new Guid("dbd55106-cbca-4197-bf2b-e1b21b36e7bd"), 4 },
                    { new Guid("701ffdee-edbc-4b6c-a8f9-88d11ef486e9"), new Guid("7034103d-0cce-40d5-a06a-345b47276ded"), new Guid("52f89479-6e11-4698-8602-e7cbe63004ec"), 4 },
                    { new Guid("940be6bb-ef0f-4cbf-92c4-b0faa27177af"), new Guid("8e33e585-870c-4f60-bd6e-995bc86ef2c6"), new Guid("dbd55106-cbca-4197-bf2b-e1b21b36e7bd"), 2 },
                    { new Guid("963b24e5-4ff6-4a05-a617-c057c01e86cf"), new Guid("cd127a73-703c-4717-88bf-7726dd09944c"), new Guid("52f89479-6e11-4698-8602-e7cbe63004ec"), 3 },
                    { new Guid("a3c1f0b2-5d7e-4c19-8b6a-2e9f4d1c7a30"), new Guid("8e33e585-870c-4f60-bd6e-995bc86ef2c6"), new Guid("52f89479-6e11-4698-8602-e7cbe63004ec"), 2 },
                    { new Guid("d2a0dd13-1f5e-448b-a748-44b769996004"), new Guid("7034103d-0cce-40d5-a06a-345b47276ded"), new Guid("dbd55106-cbca-4197-bf2b-e1b21b36e7bd"), 6 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PackageContents_ItemId",
                table: "PackageContents",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PackageContents_PackageTypeId_ItemId",
                table: "PackageContents",
                columns: new[] { "PackageTypeId", "ItemId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PackageContents");

            migrationBuilder.DropTable(
                name: "PackageTypes");

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("7034103d-0cce-40d5-a06a-345b47276ded"));

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("8e33e585-870c-4f60-bd6e-995bc86ef2c6"));

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("cd127a73-703c-4717-88bf-7726dd09944c"));

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Items");
        }
    }
}
