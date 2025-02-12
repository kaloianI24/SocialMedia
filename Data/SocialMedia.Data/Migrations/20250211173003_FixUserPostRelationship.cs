using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Migrations
{
    /// <inheritdoc />
    public partial class FixUserPostRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_AspNetUsers_SocialMediaUserId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_SocialMediaUserId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "SocialMediaUserId",
                table: "Posts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SocialMediaUserId",
                table: "Posts",
                type: "varchar(255)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_SocialMediaUserId",
                table: "Posts",
                column: "SocialMediaUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_AspNetUsers_SocialMediaUserId",
                table: "Posts",
                column: "SocialMediaUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
