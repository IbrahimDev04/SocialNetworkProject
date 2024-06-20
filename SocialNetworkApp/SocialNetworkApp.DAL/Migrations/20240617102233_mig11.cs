using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialNetworkApp.DAL.Migrations
{
    /// <inheritdoc />
    public partial class mig11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FollowingStatus",
                table: "userFriends");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "userFriends",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "userFriends");

            migrationBuilder.AddColumn<bool>(
                name: "FollowingStatus",
                table: "userFriends",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
