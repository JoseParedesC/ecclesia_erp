using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecclesia.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAppDbContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "ecclesia");

            migrationBuilder.EnsureSchema(
                name: "accounting");

            migrationBuilder.CreateTable(
                name: "Account",
                schema: "ecclesia",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    type = table.Column<string>(type: "text", nullable: false),
                    parent_account_id = table.Column<Guid>(type: "uuid", nullable: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    created_by_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_by_user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_account", x => x.id);
                    table.ForeignKey(
                        name: "fk_account_account_parent_account_id",
                        column: x => x.parent_account_id,
                        principalSchema: "ecclesia",
                        principalTable: "Account",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "accounting_periods",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    year = table.Column<int>(type: "integer", nullable: false),
                    month = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    closed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    community_id = table.Column<Guid>(type: "uuid", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    created_by_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_by_user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_accounting_periods", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "cash_accounts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "text", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    created_by_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_by_user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cash_accounts", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Community",
                schema: "ecclesia",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    rostro_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_community", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "donors",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "text", nullable: false),
                    document_number = table.Column<string>(type: "text", nullable: false),
                    is_company = table.Column<bool>(type: "boolean", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    created_by_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_by_user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_donors", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "expenses",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    amount = table.Column<decimal>(type: "numeric", nullable: false),
                    cash_account_id = table.Column<Guid>(type: "uuid", nullable: false),
                    community_id = table.Column<Guid>(type: "uuid", nullable: false),
                    journal_voucher_id = table.Column<Guid>(type: "uuid", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    created_by_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_by_user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_expenses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "SequenceControl",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    sequence_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    current_value = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sequence_control", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "journal_vouchers",
                schema: "accounting",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    voucher_number = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    type = table.Column<int>(type: "integer", maxLength: 20, nullable: false),
                    status = table.Column<int>(type: "integer", maxLength: 20, nullable: false),
                    date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    description = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    accounting_period_id = table.Column<Guid>(type: "uuid", nullable: false),
                    rostro_id = table.Column<Guid>(type: "uuid", nullable: false),
                    community_id = table.Column<Guid>(type: "uuid", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    created_by_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_by_user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_journal_vouchers", x => x.id);
                    table.ForeignKey(
                        name: "fk_journal_vouchers_accounting_periods_accounting_period_id",
                        column: x => x.accounting_period_id,
                        principalTable: "accounting_periods",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "incomes",
                schema: "accounting",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    donor_id = table.Column<Guid>(type: "uuid", nullable: true),
                    cash_account_id = table.Column<Guid>(type: "uuid", nullable: false),
                    community_id = table.Column<Guid>(type: "uuid", nullable: false),
                    journal_voucher_id = table.Column<Guid>(type: "uuid", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    created_by_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_by_user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_incomes", x => x.id);
                    table.ForeignKey(
                        name: "fk_incomes_cash_accounts_cash_account_id",
                        column: x => x.cash_account_id,
                        principalTable: "cash_accounts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_incomes_communities_community_id",
                        column: x => x.community_id,
                        principalSchema: "ecclesia",
                        principalTable: "Community",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_incomes_donors_donor_id",
                        column: x => x.donor_id,
                        principalTable: "donors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_incomes_journal_vouchers_journal_voucher_id",
                        column: x => x.journal_voucher_id,
                        principalSchema: "accounting",
                        principalTable: "journal_vouchers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "journal_voucher_lines",
                schema: "accounting",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    journal_voucher_id = table.Column<Guid>(type: "uuid", nullable: false),
                    account_id = table.Column<Guid>(type: "uuid", nullable: false),
                    amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    line_type = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_journal_voucher_lines", x => x.id);
                    table.ForeignKey(
                        name: "fk_journal_voucher_lines_accounts_account_id",
                        column: x => x.account_id,
                        principalSchema: "ecclesia",
                        principalTable: "Account",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_journal_voucher_lines_journal_vouchers_journal_voucher_id",
                        column: x => x.journal_voucher_id,
                        principalSchema: "accounting",
                        principalTable: "journal_vouchers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_account_code",
                schema: "ecclesia",
                table: "Account",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_account_parent_account_id",
                schema: "ecclesia",
                table: "Account",
                column: "parent_account_id");

            migrationBuilder.CreateIndex(
                name: "ix_community_name",
                schema: "ecclesia",
                table: "Community",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "ix_community_rostro_id_name",
                schema: "ecclesia",
                table: "Community",
                columns: new[] { "rostro_id", "name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_incomes_cash_account_id",
                schema: "accounting",
                table: "incomes",
                column: "cash_account_id");

            migrationBuilder.CreateIndex(
                name: "ix_incomes_community_id",
                schema: "accounting",
                table: "incomes",
                column: "community_id");

            migrationBuilder.CreateIndex(
                name: "ix_incomes_donor_id",
                schema: "accounting",
                table: "incomes",
                column: "donor_id");

            migrationBuilder.CreateIndex(
                name: "ix_incomes_journal_voucher_id",
                schema: "accounting",
                table: "incomes",
                column: "journal_voucher_id");

            migrationBuilder.CreateIndex(
                name: "ix_journal_voucher_lines_account_id",
                schema: "accounting",
                table: "journal_voucher_lines",
                column: "account_id");

            migrationBuilder.CreateIndex(
                name: "ix_journal_voucher_lines_journal_voucher_id",
                schema: "accounting",
                table: "journal_voucher_lines",
                column: "journal_voucher_id");

            migrationBuilder.CreateIndex(
                name: "ix_journal_vouchers_accounting_period_id",
                schema: "accounting",
                table: "journal_vouchers",
                column: "accounting_period_id");

            migrationBuilder.CreateIndex(
                name: "ix_journal_vouchers_voucher_number",
                schema: "accounting",
                table: "journal_vouchers",
                column: "voucher_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_sequence_control_sequence_type",
                table: "SequenceControl",
                column: "sequence_type",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "expenses");

            migrationBuilder.DropTable(
                name: "incomes",
                schema: "accounting");

            migrationBuilder.DropTable(
                name: "journal_voucher_lines",
                schema: "accounting");

            migrationBuilder.DropTable(
                name: "SequenceControl");

            migrationBuilder.DropTable(
                name: "cash_accounts");

            migrationBuilder.DropTable(
                name: "Community",
                schema: "ecclesia");

            migrationBuilder.DropTable(
                name: "donors");

            migrationBuilder.DropTable(
                name: "Account",
                schema: "ecclesia");

            migrationBuilder.DropTable(
                name: "journal_vouchers",
                schema: "accounting");

            migrationBuilder.DropTable(
                name: "accounting_periods");
        }
    }
}
