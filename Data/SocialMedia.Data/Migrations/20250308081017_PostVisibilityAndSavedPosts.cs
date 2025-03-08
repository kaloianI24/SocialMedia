using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Migrations
{
    /// <inheritdoc />
    public partial class PostVisibilityAndSavedPosts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Visibility",
                table: "Posts",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SocialMediaPostSocialMediaUser1",
                columns: table => new
                {
                    SavedByUsersId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SavedPostsId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialMediaPostSocialMediaUser1", x => new { x.SavedByUsersId, x.SavedPostsId });
                    table.ForeignKey(
                        name: "FK_SocialMediaPostSocialMediaUser1_AspNetUsers_SavedByUsersId",
                        column: x => x.SavedByUsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SocialMediaPostSocialMediaUser1_Posts_SavedPostsId",
                        column: x => x.SavedPostsId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_SocialMediaPostSocialMediaUser1_SavedPostsId",
                table: "SocialMediaPostSocialMediaUser1",
                column: "SavedPostsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SocialMediaPostSocialMediaUser1");

            migrationBuilder.DropColumn(
                name: "Visibility",
                table: "Posts");
        }
    }
}
