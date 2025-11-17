using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduVerse.Migrations
{
    /// <inheritdoc />
    public partial class SchoolEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SchoolAccountId",
                table: "AspNetUsers",
                type: "RAW(16)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Schools",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    SignUpRequestId = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: false),
                    NormalizedName = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: false),
                    NameShortcut = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    NormalizedNameShortcut = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    NormalizedEmail = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    Street = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    BuildingNumber = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    PostalCode = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    City = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    State = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    Country = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schools", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Schools_SignUpRequests_SignUpRequestId",
                        column: x => x.SignUpRequestId,
                        principalTable: "SignUpRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SchoolRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    SchoolId = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    NormalizedName = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    Hierarchy = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    IsParent = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    IsStudent = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    IsStaff = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    CanManageAccounts = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    CanManageRoles = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    CanManageGroups = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    CanManageCourses = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    CanManageStudents = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchoolRoles_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    TeacherId = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    NormalizedName = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SchoolAccounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    UserId = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    SchoolId = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    SchoolRoleId = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    GroupId = table.Column<Guid>(type: "RAW(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchoolAccounts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SchoolAccounts_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SchoolAccounts_SchoolRoles_SchoolRoleId",
                        column: x => x.SchoolRoleId,
                        principalTable: "SchoolRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SchoolAccounts_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentParents",
                columns: table => new
                {
                    StudentId = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    ParentId = table.Column<Guid>(type: "RAW(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentParents", x => new { x.StudentId, x.ParentId });
                    table.ForeignKey(
                        name: "FK_StudentParents_SchoolAccounts_ParentId",
                        column: x => x.ParentId,
                        principalTable: "SchoolAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentParents_SchoolAccounts_StudentId",
                        column: x => x.StudentId,
                        principalTable: "SchoolAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Groups_TeacherId",
                table: "Groups",
                column: "TeacherId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SchoolAccounts_GroupId",
                table: "SchoolAccounts",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolAccounts_SchoolId",
                table: "SchoolAccounts",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolAccounts_SchoolRoleId",
                table: "SchoolAccounts",
                column: "SchoolRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolAccounts_UserId",
                table: "SchoolAccounts",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SchoolRoles_SchoolId",
                table: "SchoolRoles",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_Schools_SignUpRequestId",
                table: "Schools",
                column: "SignUpRequestId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentParents_ParentId",
                table: "StudentParents",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_SchoolAccounts_TeacherId",
                table: "Groups",
                column: "TeacherId",
                principalTable: "SchoolAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_SchoolAccounts_TeacherId",
                table: "Groups");

            migrationBuilder.DropTable(
                name: "StudentParents");

            migrationBuilder.DropTable(
                name: "SchoolAccounts");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "SchoolRoles");

            migrationBuilder.DropTable(
                name: "Schools");

            migrationBuilder.DropColumn(
                name: "SchoolAccountId",
                table: "AspNetUsers");
        }
    }
}
