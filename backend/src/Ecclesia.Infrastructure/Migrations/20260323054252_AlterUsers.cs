using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecclesia.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlterUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Users",
                schema: "access_manager",
                newName: "users",
                newSchema: "access_manager");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                schema: "access_manager",
                table: "users",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "email",
                schema: "access_manager",
                table: "users",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "user_name",
                schema: "access_manager",
                table: "users",
                type: "character varying(25)",
                maxLength: 25,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "ix_users_email",
                schema: "access_manager",
                table: "users",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_users_email",
                schema: "access_manager",
                table: "users");

            migrationBuilder.DropColumn(
                name: "user_name",
                schema: "access_manager",
                table: "users");

            migrationBuilder.RenameTable(
                name: "users",
                schema: "access_manager",
                newName: "Users",
                newSchema: "access_manager");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                schema: "access_manager",
                table: "Users",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "email",
                schema: "access_manager",
                table: "Users",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256);
        }
    }
}
