using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialNetworkApp.DAL.Migrations
{
    /// <inheritdoc />
    public partial class mig46 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_postComments_userPosts_UserPostsId",
                table: "postComments");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "postComments");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserPostsId",
                table: "postComments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_postComments_userPosts_UserPostsId",
                table: "postComments",
                column: "UserPostsId",
                principalTable: "userPosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_postComments_userPosts_UserPostsId",
                table: "postComments");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserPostsId",
                table: "postComments",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<string>(
                name: "PostId",
                table: "postComments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_postComments_userPosts_UserPostsId",
                table: "postComments",
                column: "UserPostsId",
                principalTable: "userPosts",
                principalColumn: "Id");
        }
    }
}
