using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialNetworkApp.DAL.Migrations
{
    /// <inheritdoc />
    public partial class mig2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_userSettings_userProfiles_UserProfileId",
                table: "userSettings");

            migrationBuilder.DropIndex(
                name: "IX_userSettings_UserProfileId",
                table: "userSettings");

            migrationBuilder.AlterColumn<string>(
                name: "UserProfileId",
                table: "userSettings",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserProfileId1",
                table: "userSettings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_userSettings_UserProfileId1",
                table: "userSettings",
                column: "UserProfileId1");

            migrationBuilder.AddForeignKey(
                name: "FK_userSettings_userProfiles_UserProfileId1",
                table: "userSettings",
                column: "UserProfileId1",
                principalTable: "userProfiles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_userSettings_userProfiles_UserProfileId1",
                table: "userSettings");

            migrationBuilder.DropIndex(
                name: "IX_userSettings_UserProfileId1",
                table: "userSettings");

            migrationBuilder.DropColumn(
                name: "UserProfileId1",
                table: "userSettings");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserProfileId",
                table: "userSettings",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_userSettings_UserProfileId",
                table: "userSettings",
                column: "UserProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_userSettings_userProfiles_UserProfileId",
                table: "userSettings",
                column: "UserProfileId",
                principalTable: "userProfiles",
                principalColumn: "Id");
        }
    }
}
