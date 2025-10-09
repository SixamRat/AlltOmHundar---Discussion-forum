using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlltOmHundar.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Sync_20251009_0608 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Posts_TopicId",
                table: "Posts");

            migrationBuilder.CreateTable(
                name: "GroupInvitations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    InvitedUserId = table.Column<int>(type: "int", nullable: false),
                    InvitedByUserId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupInvitations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupInvitations_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupInvitations_Users_InvitedByUserId",
                        column: x => x.InvitedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GroupInvitations_Users_InvitedUserId",
                        column: x => x.InvitedUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Posts_TopicId_CreatedAt",
                table: "Posts",
                columns: new[] { "TopicId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_GroupInvitations_GroupId_InvitedUserId",
                table: "GroupInvitations",
                columns: new[] { "GroupId", "InvitedUserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GroupInvitations_InvitedByUserId",
                table: "GroupInvitations",
                column: "InvitedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupInvitations_InvitedUserId",
                table: "GroupInvitations",
                column: "InvitedUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupInvitations");

            migrationBuilder.DropIndex(
                name: "IX_Posts_TopicId_CreatedAt",
                table: "Posts");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_TopicId",
                table: "Posts",
                column: "TopicId");
        }
    }
}
