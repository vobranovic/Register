using System;
using Microsoft.AspNetCore.Identity;
using System.Text;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Register.Data.Migrations
{
    public partial class initialAndAdmin : Migration
    {
        const string _adminUserGuid = "69d4fadb-3097-46a0-8c7b-b6ae35a97334";
        const string _adminRoleGuid = "c2efb0b9-c676-4c27-a452-7b30244279dd";
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var hasher = new PasswordHasher<IdentityUser>();
            var passwordHashed = hasher.HashPassword(null, "Password123!");

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("INSERT INTO AspNetUsers(Id, UserName, NormalizedUserName, Email, EmailConfirmed, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount, NormalizedEmail, PasswordHash, SecurityStamp)");
            sb.AppendLine("VALUES(");
            sb.AppendLine($"'{_adminUserGuid}'");
            sb.AppendLine(", 'admin@admin.com'");
            sb.AppendLine(", 'ADMIN@ADMIN.COM'");
            sb.AppendLine(", 'admin@admin.com'");
            sb.AppendLine(", 0");
            sb.AppendLine(", 0");
            sb.AppendLine(", 0");
            sb.AppendLine(", 0");
            sb.AppendLine(", 0");
            sb.AppendLine(", 'ADMIN@ADMIN.COM'");
            sb.AppendLine($", '{passwordHashed}'");
            sb.AppendLine(", ''");
            sb.AppendLine(")");

            //insert korisnika
            migrationBuilder.Sql(sb.ToString());

            //insert rola
            migrationBuilder.Sql($"INSERT INTO AspNetRoles (Id, Name, NormalizedName) VALUES('{_adminRoleGuid}', 'Admin', 'ADMIN')");

            //insert rola za korisnika
            migrationBuilder.Sql($"INSERT INTO AspNetUserRoles(UserId, RoleId) VALUES ('{_adminUserGuid}', '{_adminRoleGuid}')");

            migrationBuilder.CreateTable(
                name: "Club",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Club", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Personnel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RegistrationId = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Birthday = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personnel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Player",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RegistrationId = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Birthday = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Player", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Season",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Year = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Season", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Team",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Category = table.Column<int>(type: "int", nullable: false),
                    SeasonId = table.Column<int>(type: "int", nullable: false),
                    ClubId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Team", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Team_Club_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Club",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Team_Season_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "Season",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeamPersonnel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeamId = table.Column<int>(type: "int", nullable: false),
                    PersonnelId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamPersonnel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeamPersonnel_Personnel_PersonnelId",
                        column: x => x.PersonnelId,
                        principalTable: "Personnel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamPersonnel_Team_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Team",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeamPlayer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeamId = table.Column<int>(type: "int", nullable: false),
                    PlayerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamPlayer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeamPlayer_Player_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Player",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamPlayer_Team_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Team",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Club_Name",
                table: "Club",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Personnel_RegistrationId",
                table: "Personnel",
                column: "RegistrationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Player_RegistrationId",
                table: "Player",
                column: "RegistrationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Season_Year",
                table: "Season",
                column: "Year",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Team_ClubId",
                table: "Team",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "IX_Team_SeasonId",
                table: "Team",
                column: "SeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamPersonnel_PersonnelId",
                table: "TeamPersonnel",
                column: "PersonnelId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamPersonnel_TeamId",
                table: "TeamPersonnel",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamPlayer_PlayerId",
                table: "TeamPlayer",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamPlayer_TeamId",
                table: "TeamPlayer",
                column: "TeamId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TeamPersonnel");

            migrationBuilder.DropTable(
                name: "TeamPlayer");

            migrationBuilder.DropTable(
                name: "Personnel");

            migrationBuilder.DropTable(
                name: "Player");

            migrationBuilder.DropTable(
                name: "Team");

            migrationBuilder.DropTable(
                name: "Club");

            migrationBuilder.DropTable(
                name: "Season");


            migrationBuilder.Sql($"DELETE FROM AspNetUserRoles WHERE UserId = '{_adminUserGuid}' AND RoleId = '{_adminRoleGuid}'");
            migrationBuilder.Sql($"DELETE FROM AspNetUsers WHERE Id='{_adminUserGuid}'");
            migrationBuilder.Sql($"DELETE FROM AspNetRoles WHERE Id='{_adminRoleGuid}'");
        }
    }
}
