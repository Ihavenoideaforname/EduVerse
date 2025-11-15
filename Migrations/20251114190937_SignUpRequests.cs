using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduVerse.Migrations
{
    /// <inheritdoc />
    public partial class SignUpRequests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SignUpRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    SchoolName = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: false),
                    SchoolNameShortcut = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    SchoolEmail = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    SchoolPhoneNumber = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    Street = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    BuildingNumber = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    PostalCode = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    City = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    State = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    Country = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    PrincipalName = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    PrincipalSurname = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    RequestLetter = table.Column<string>(type: "NVARCHAR2(2000)", maxLength: 2000, nullable: false),
                    RequestedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    Status = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    ReviewedBy = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: true),
                    ReviewedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SignUpRequests", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SignUpRequests");
        }
    }
}
