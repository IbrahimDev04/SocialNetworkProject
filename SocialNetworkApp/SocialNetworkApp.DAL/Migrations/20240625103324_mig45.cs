using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialNetworkApp.DAL.Migrations
{
    /// <inheritdoc />
    public partial class mig45 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_postFavorites_userPosts_PostId1",
                table: "postFavorites");

            migrationBuilder.DropIndex(
                name: "IX_postFavorites_PostId1",
                table: "postFavorites");

            migrationBuilder.DropColumn(
                name: "PostId1",
                table: "postFavorites");

            migrationBuilder.AlterColumn<Guid>(
                name: "PostId",
                table: "postFavorites",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_postFavorites_PostId",
                table: "postFavorites",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_postFavorites_userPosts_PostId",
                table: "postFavorites",
                column: "PostId",
                principalTable: "userPosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_postFavorites_userPosts_PostId",
                table: "postFavorites");

            migrationBuilder.DropIndex(
                name: "IX_postFavorites_PostId",
                table: "postFavorites");

            migrationBuilder.AlterColumn<string>(
                name: "PostId",
                table: "postFavorites",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "PostId1",
                table: "postFavorites",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_postFavorites_PostId1",
                table: "postFavorites",
                column: "PostId1");

            migrationBuilder.AddForeignKey(
                name: "FK_postFavorites_userPosts_PostId1",
                table: "postFavorites",
                column: "PostId1",
                principalTable: "userPosts",
                principalColumn: "Id");
        }
    }
}
