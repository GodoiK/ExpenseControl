using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseControl.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryIdToFinancialRecords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId",
                table: "FinancialRecords",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Users_Name_Age",
                table: "Users",
                columns: new[] { "Name", "Age" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FinancialRecords_CategoryId",
                table: "FinancialRecords",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_FinancialRecords_Categories_CategoryId",
                table: "FinancialRecords",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FinancialRecords_Categories_CategoryId",
                table: "FinancialRecords");

            migrationBuilder.DropIndex(
                name: "IX_Users_Name_Age",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_FinancialRecords_CategoryId",
                table: "FinancialRecords");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "FinancialRecords");
        }
    }
}
