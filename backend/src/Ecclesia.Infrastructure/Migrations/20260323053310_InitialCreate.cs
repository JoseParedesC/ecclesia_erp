using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecclesia.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                schema: "access_manager",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Name",
                schema: "access_manager",
                table: "Users",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Email",
                schema: "access_manager",
                table: "Users",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "access_manager",
                table: "Users",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedByUserId",
                schema: "access_manager",
                table: "Users",
                newName: "updated_by_user_id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                schema: "access_manager",
                table: "Users",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                schema: "access_manager",
                table: "Users",
                newName: "password_hash");

            migrationBuilder.RenameColumn(
                name: "CreatedByUserId",
                schema: "access_manager",
                table: "Users",
                newName: "created_by_user_id");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                schema: "access_manager",
                table: "Users",
                newName: "created_at");

            migrationBuilder.AddPrimaryKey(
                name: "pk_users",
                schema: "access_manager",
                table: "Users",
                column: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_users",
                schema: "access_manager",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "name",
                schema: "access_manager",
                table: "Users",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "email",
                schema: "access_manager",
                table: "Users",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "access_manager",
                table: "Users",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_by_user_id",
                schema: "access_manager",
                table: "Users",
                newName: "UpdatedByUserId");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                schema: "access_manager",
                table: "Users",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "password_hash",
                schema: "access_manager",
                table: "Users",
                newName: "PasswordHash");

            migrationBuilder.RenameColumn(
                name: "created_by_user_id",
                schema: "access_manager",
                table: "Users",
                newName: "CreatedByUserId");

            migrationBuilder.RenameColumn(
                name: "created_at",
                schema: "access_manager",
                table: "Users",
                newName: "CreatedAt");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                schema: "access_manager",
                table: "Users",
                column: "Id");
        }
    }
}
