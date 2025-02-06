using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Migrations
{
    /// <inheritdoc />
    public partial class EditUserPostRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_Posts_PostId",
                table: "Attachments");

            migrationBuilder.DropTable(
                name: "PostTag");

            migrationBuilder.RenameColumn(
                name: "PostId",
                table: "Attachments",
                newName: "SocialMediaPostId");

            migrationBuilder.RenameIndex(
                name: "IX_Attachments_PostId",
                table: "Attachments",
                newName: "IX_Attachments_SocialMediaPostId");

            migrationBuilder.CreateTable(
                name: "SocialMediaPostSocialMediaTag",
                columns: table => new
                {
                    PostsId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TagsId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialMediaPostSocialMediaTag", x => new { x.PostsId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_SocialMediaPostSocialMediaTag_Posts_PostsId",
                        column: x => x.PostsId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SocialMediaPostSocialMediaTag_Tags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_SocialMediaPostSocialMediaTag_TagsId",
                table: "SocialMediaPostSocialMediaTag",
                column: "TagsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_Posts_SocialMediaPostId",
                table: "Attachments",
                column: "SocialMediaPostId",
                principalTable: "Posts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_Posts_SocialMediaPostId",
                table: "Attachments");

            migrationBuilder.DropTable(
                name: "SocialMediaPostSocialMediaTag");

            migrationBuilder.RenameColumn(
                name: "SocialMediaPostId",
                table: "Attachments",
                newName: "PostId");

            migrationBuilder.RenameIndex(
                name: "IX_Attachments_SocialMediaPostId",
                table: "Attachments",
                newName: "IX_Attachments_PostId");

            migrationBuilder.CreateTable(
                name: "PostTag",
                columns: table => new
                {
                    PostsId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TagsId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostTag", x => new { x.PostsId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_PostTag_Posts_PostsId",
                        column: x => x.PostsId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostTag_Tags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_PostTag_TagsId",
                table: "PostTag",
                column: "TagsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_Posts_PostId",
                table: "Attachments",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id");
        }
    }
}
