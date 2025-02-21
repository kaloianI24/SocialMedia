using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Migrations
{
    /// <inheritdoc />
    public partial class Privacy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Comments_CommentId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_SocialMediaUserSocialMediaUser_AspNetUsers_FollowersId",
                table: "SocialMediaUserSocialMediaUser");

            migrationBuilder.DropTable(
                name: "CloudResourceComment");

            migrationBuilder.RenameColumn(
                name: "FollowersId",
                table: "SocialMediaUserSocialMediaUser",
                newName: "FollowingId");

            migrationBuilder.RenameColumn(
                name: "CommentId",
                table: "Comments",
                newName: "SocialMediaCommentId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_CommentId",
                table: "Comments",
                newName: "IX_Comments_SocialMediaCommentId");

            migrationBuilder.AddColumn<bool>(
                name: "isPrivate",
                table: "AspNetUsers",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "CloudResourceSocialMediaComment",
                columns: table => new
                {
                    AttachmentsId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SocialMediaCommentId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CloudResourceSocialMediaComment", x => new { x.AttachmentsId, x.SocialMediaCommentId });
                    table.ForeignKey(
                        name: "FK_CloudResourceSocialMediaComment_Attachments_AttachmentsId",
                        column: x => x.AttachmentsId,
                        principalTable: "Attachments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CloudResourceSocialMediaComment_Comments_SocialMediaCommentId",
                        column: x => x.SocialMediaCommentId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_CloudResourceSocialMediaComment_SocialMediaCommentId",
                table: "CloudResourceSocialMediaComment",
                column: "SocialMediaCommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Comments_SocialMediaCommentId",
                table: "Comments",
                column: "SocialMediaCommentId",
                principalTable: "Comments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SocialMediaUserSocialMediaUser_AspNetUsers_FollowingId",
                table: "SocialMediaUserSocialMediaUser",
                column: "FollowingId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Comments_SocialMediaCommentId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_SocialMediaUserSocialMediaUser_AspNetUsers_FollowingId",
                table: "SocialMediaUserSocialMediaUser");

            migrationBuilder.DropTable(
                name: "CloudResourceSocialMediaComment");

            migrationBuilder.DropColumn(
                name: "isPrivate",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "FollowingId",
                table: "SocialMediaUserSocialMediaUser",
                newName: "FollowersId");

            migrationBuilder.RenameColumn(
                name: "SocialMediaCommentId",
                table: "Comments",
                newName: "CommentId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_SocialMediaCommentId",
                table: "Comments",
                newName: "IX_Comments_CommentId");

            migrationBuilder.CreateTable(
                name: "CloudResourceComment",
                columns: table => new
                {
                    AttachmentsId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CommentId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CloudResourceComment", x => new { x.AttachmentsId, x.CommentId });
                    table.ForeignKey(
                        name: "FK_CloudResourceComment_Attachments_AttachmentsId",
                        column: x => x.AttachmentsId,
                        principalTable: "Attachments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CloudResourceComment_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_CloudResourceComment_CommentId",
                table: "CloudResourceComment",
                column: "CommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Comments_CommentId",
                table: "Comments",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SocialMediaUserSocialMediaUser_AspNetUsers_FollowersId",
                table: "SocialMediaUserSocialMediaUser",
                column: "FollowersId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
