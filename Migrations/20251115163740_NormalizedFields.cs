using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduVerse.Migrations
{
    /// <inheritdoc />
    public partial class NormalizedFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NormalizedSchoolEmail",
                table: "SignUpRequests",
                type: "NVARCHAR2(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NormalizedSchoolName",
                table: "SignUpRequests",
                type: "NVARCHAR2(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NormalizedSchoolNameShortcut",
                table: "SignUpRequests",
                type: "NVARCHAR2(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NormalizedSchoolEmail",
                table: "SignUpRequests");

            migrationBuilder.DropColumn(
                name: "NormalizedSchoolName",
                table: "SignUpRequests");

            migrationBuilder.DropColumn(
                name: "NormalizedSchoolNameShortcut",
                table: "SignUpRequests");
        }
    }
}
