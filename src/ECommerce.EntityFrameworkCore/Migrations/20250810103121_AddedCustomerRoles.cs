using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.Migrations
{
    /// <inheritdoc />
    public partial class AddedCustomerRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CustomerId1",
                table: "CustomersPasswords",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CustomerRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    SystemName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FreeShipping = table.Column<bool>(type: "bit", nullable: false),
                    TaxExempt = table.Column<bool>(type: "bit", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    IsSystemRole = table.Column<bool>(type: "bit", nullable: false),
                    EnablePasswordLifeTime = table.Column<bool>(type: "bit", nullable: false),
                    OverrideTaxDisplayType = table.Column<int>(type: "int", nullable: true),
                    DefaultTaxDisplayType = table.Column<int>(type: "int", nullable: false),
                    PurchasedWithProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customer_Customer_Mapping",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerRoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer_Customer_Mapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customer_Customer_Mapping_CustomerRoles_CustomerRoleId",
                        column: x => x.CustomerRoleId,
                        principalTable: "CustomerRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Customer_Customer_Mapping_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomersPasswords_CustomerId1",
                table: "CustomersPasswords",
                column: "CustomerId1");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_Customer_Mapping_CustomerId_CustomerRoleId",
                table: "Customer_Customer_Mapping",
                columns: new[] { "CustomerId", "CustomerRoleId" });

            migrationBuilder.CreateIndex(
                name: "IX_Customer_Customer_Mapping_CustomerRoleId",
                table: "Customer_Customer_Mapping",
                column: "CustomerRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomersPasswords_Customers_CustomerId1",
                table: "CustomersPasswords",
                column: "CustomerId1",
                principalTable: "Customers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomersPasswords_Customers_CustomerId1",
                table: "CustomersPasswords");

            migrationBuilder.DropTable(
                name: "Customer_Customer_Mapping");

            migrationBuilder.DropTable(
                name: "CustomerRoles");

            migrationBuilder.DropIndex(
                name: "IX_CustomersPasswords_CustomerId1",
                table: "CustomersPasswords");

            migrationBuilder.DropColumn(
                name: "CustomerId1",
                table: "CustomersPasswords");
        }
    }
}
