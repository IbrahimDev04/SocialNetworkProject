using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialNetworkApp.DAL.Migrations
{
    /// <inheritdoc />
    public partial class mig1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_userProfiles_userSettings_UserSettingsId1",
                table: "userProfiles");

            migrationBuilder.DropIndex(
                name: "IX_userProfiles_UserSettingsId1",
                table: "userProfiles");

            migrationBuilder.DropColumn(
                name: "UserSettingsId",
                table: "userProfiles");

            migrationBuilder.DropColumn(
                name: "UserSettingsId1",
                table: "userProfiles");

            migrationBuilder.AddColumn<Guid>(
                name: "UserProfileId",
                table: "userSettings",
                type: "uniqueidentifier",
                nullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_userSettings_userProfiles_UserProfileId",
                table: "userSettings");

            migrationBuilder.DropIndex(
                name: "IX_userSettings_UserProfileId",
                table: "userSettings");

            migrationBuilder.DropColumn(
                name: "UserProfileId",
                table: "userSettings");

            migrationBuilder.AddColumn<string>(
                name: "UserSettingsId",
                table: "userProfiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserSettingsId1",
                table: "userProfiles",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_userProfiles_UserSettingsId1",
                table: "userProfiles",
                column: "UserSettingsId1");

            migrationBuilder.AddForeignKey(
                name: "FK_userProfiles_userSettings_UserSettingsId1",
                table: "userProfiles",
                column: "UserSettingsId1",
                principalTable: "userSettings",
                principalColumn: "Id");
        }
    }
}
