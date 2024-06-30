using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialNetworkApp.DAL.Migrations
{
    /// <inheritdoc />
    public partial class asdg2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_postStats_userPosts_PostId1",
                table: "postStats");

            migrationBuilder.DropIndex(
                name: "IX_postStats_PostId1",
                table: "postStats");

            migrationBuilder.DropColumn(
                name: "LikeCount",
                table: "postStats");

            migrationBuilder.DropColumn(
                name: "PostId1",
                table: "postStats");

            migrationBuilder.AlterColumn<Guid>(
                name: "PostId",
                table: "postStats",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "postStats",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_postStats_PostId",
                table: "postStats",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_postStats_UserId",
                table: "postStats",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_postStats_AspNetUsers_UserId",
                table: "postStats",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_postStats_userPosts_PostId",
                table: "postStats",
                column: "PostId",
                principalTable: "userPosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_postStats_AspNetUsers_UserId",
                table: "postStats");

            migrationBuilder.DropForeignKey(
                name: "FK_postStats_userPosts_PostId",
                table: "postStats");

            migrationBuilder.DropIndex(
                name: "IX_postStats_PostId",
                table: "postStats");

            migrationBuilder.DropIndex(
                name: "IX_postStats_UserId",
                table: "postStats");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "postStats");

            migrationBuilder.AlterColumn<string>(
                name: "PostId",
                table: "postStats",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<int>(
                name: "LikeCount",
                table: "postStats",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "PostId1",
                table: "postStats",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_postStats_PostId1",
                table: "postStats",
                column: "PostId1");

            migrationBuilder.AddForeignKey(
                name: "FK_postStats_userPosts_PostId1",
                table: "postStats",
                column: "PostId1",
                principalTable: "userPosts",
                principalColumn: "Id");
        }
    }
}
