using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseControl.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixUserRelationshipIgnoreRecords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FinancialRecords_Users_UserId1",
                table: "FinancialRecords");

            migrationBuilder.DropIndex(
                name: "IX_FinancialRecords_UserId1",
                table: "FinancialRecords");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "FinancialRecords");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId1",
                table: "FinancialRecords",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FinancialRecords_UserId1",
                table: "FinancialRecords",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_FinancialRecords_Users_UserId1",
                table: "FinancialRecords",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
