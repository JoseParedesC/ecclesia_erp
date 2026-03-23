using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecclesia.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class SetAccountingSchemas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "expenses",
                newName: "expenses",
                newSchema: "accounting");

            migrationBuilder.RenameTable(
                name: "donors",
                newName: "donors",
                newSchema: "ecclesia");

            migrationBuilder.RenameTable(
                name: "cash_accounts",
                newName: "cash_accounts",
                newSchema: "accounting");

            migrationBuilder.RenameTable(
                name: "accounting_periods",
                newName: "accounting_periods",
                newSchema: "accounting");

            migrationBuilder.RenameTable(
                name: "Account",
                schema: "ecclesia",
                newName: "Account",
                newSchema: "accounting");

            migrationBuilder.AlterColumn<decimal>(
                name: "amount",
                schema: "accounting",
                table: "expenses",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.CreateIndex(
                name: "ix_expenses_journal_voucher_id",
                schema: "accounting",
                table: "expenses",
                column: "journal_voucher_id");

            migrationBuilder.AddForeignKey(
                name: "fk_expenses_journal_vouchers_journal_voucher_id",
                schema: "accounting",
                table: "expenses",
                column: "journal_voucher_id",
                principalSchema: "accounting",
                principalTable: "journal_vouchers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_expenses_journal_vouchers_journal_voucher_id",
                schema: "accounting",
                table: "expenses");

            migrationBuilder.DropIndex(
                name: "ix_expenses_journal_voucher_id",
                schema: "accounting",
                table: "expenses");

            migrationBuilder.RenameTable(
                name: "expenses",
                schema: "accounting",
                newName: "expenses");

            migrationBuilder.RenameTable(
                name: "donors",
                schema: "ecclesia",
                newName: "donors");

            migrationBuilder.RenameTable(
                name: "cash_accounts",
                schema: "accounting",
                newName: "cash_accounts");

            migrationBuilder.RenameTable(
                name: "accounting_periods",
                schema: "accounting",
                newName: "accounting_periods");

            migrationBuilder.RenameTable(
                name: "Account",
                schema: "accounting",
                newName: "Account",
                newSchema: "ecclesia");

            migrationBuilder.AlterColumn<decimal>(
                name: "amount",
                table: "expenses",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);
        }
    }
}
