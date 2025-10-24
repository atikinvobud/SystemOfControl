using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UsersService.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "roleEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roleEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "userEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Login = table.Column<string>(type: "text", nullable: false),
                    HashPassword = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "refreshTokensEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    HashToken = table.Column<string>(type: "text", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_refreshTokensEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_refreshTokensEntities_userEntities_Id",
                        column: x => x.Id,
                        principalTable: "userEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "userInfoEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Surname = table.Column<string>(type: "text", nullable: false),
                    TimeOfCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TimeOfUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userInfoEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_userInfoEntities_userEntities_Id",
                        column: x => x.Id,
                        principalTable: "userEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "userRoleEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userRoleEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_userRoleEntities_roleEntities_RoleId",
                        column: x => x.RoleId,
                        principalTable: "roleEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_userRoleEntities_userEntities_UserId",
                        column: x => x.UserId,
                        principalTable: "userEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_userRoleEntities_RoleId",
                table: "userRoleEntities",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_userRoleEntities_UserId",
                table: "userRoleEntities",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "refreshTokensEntities");

            migrationBuilder.DropTable(
                name: "userInfoEntities");

            migrationBuilder.DropTable(
                name: "userRoleEntities");

            migrationBuilder.DropTable(
                name: "roleEntities");

            migrationBuilder.DropTable(
                name: "userEntities");
        }
    }
}
