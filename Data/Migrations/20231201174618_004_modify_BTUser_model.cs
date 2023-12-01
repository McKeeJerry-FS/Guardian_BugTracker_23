using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Guardian_BugTracker_23.Data.Migrations
{
    /// <inheritdoc />
    public partial class _004_modify_BTUser_model : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AvatarContentType",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "AvatarData",
                table: "AspNetUsers",
                type: "bytea",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvatarContentType",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AvatarData",
                table: "AspNetUsers");
        }
    }
}
