using Microsoft.EntityFrameworkCore.Migrations;

namespace brightcast.Migrations
{
    public partial class SecondMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Campaigns_UserProfiles_UserProfileId",
                table: "Campaigns");

            migrationBuilder.AlterColumn<int>(
                name: "UserProfileId",
                table: "Campaigns",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Businesses",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Campaigns_UserProfiles_UserProfileId",
                table: "Campaigns",
                column: "UserProfileId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Campaigns_UserProfiles_UserProfileId",
                table: "Campaigns");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Businesses");

            migrationBuilder.AlterColumn<int>(
                name: "UserProfileId",
                table: "Campaigns",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Campaigns_UserProfiles_UserProfileId",
                table: "Campaigns",
                column: "UserProfileId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
