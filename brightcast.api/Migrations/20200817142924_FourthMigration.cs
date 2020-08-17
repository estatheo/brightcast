using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace brightcast.Migrations
{
    public partial class FourthMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "KeyString",
                table: "ContactLists",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ChatMessages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderId = table.Column<int>(nullable: false),
                    SenderName = table.Column<string>(nullable: true),
                    AvatarUrl = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Reply = table.Column<bool>(nullable: false),
                    Files = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    CampaignId = table.Column<int>(nullable: false),
                    ContactId = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMessages", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatMessages");

            migrationBuilder.DropColumn(
                name: "KeyString",
                table: "ContactLists");
        }
    }
}
