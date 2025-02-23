using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Migrations
{
    /// <inheritdoc />
    public partial class SocialMediaComments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Comments_SocialMediaCommentId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_UserCommentReaction_AspNetUsers_CreatedById",
                table: "UserCommentReaction");

            migrationBuilder.DropForeignKey(
                name: "FK_UserCommentReaction_AspNetUsers_DeletedById",
                table: "UserCommentReaction");

            migrationBuilder.DropForeignKey(
                name: "FK_UserCommentReaction_AspNetUsers_UpdatedById",
                table: "UserCommentReaction");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPostComment_AspNetUsers_CreatedById",
                table: "UserPostComment");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPostComment_AspNetUsers_DeletedById",
                table: "UserPostComment");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPostComment_AspNetUsers_UpdatedById",
                table: "UserPostComment");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPostReaction_AspNetUsers_CreatedById",
                table: "UserPostReaction");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPostReaction_AspNetUsers_DeletedById",
                table: "UserPostReaction");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPostReaction_AspNetUsers_UpdatedById",
                table: "UserPostReaction");

            migrationBuilder.DropIndex(
                name: "IX_UserPostReaction_CreatedById",
                table: "UserPostReaction");

            migrationBuilder.DropIndex(
                name: "IX_UserPostReaction_DeletedById",
                table: "UserPostReaction");

            migrationBuilder.DropIndex(
                name: "IX_UserPostReaction_UpdatedById",
                table: "UserPostReaction");

            migrationBuilder.DropIndex(
                name: "IX_UserPostComment_CreatedById",
                table: "UserPostComment");

            migrationBuilder.DropIndex(
                name: "IX_UserPostComment_DeletedById",
                table: "UserPostComment");

            migrationBuilder.DropIndex(
                name: "IX_UserPostComment_UpdatedById",
                table: "UserPostComment");

            migrationBuilder.DropIndex(
                name: "IX_UserCommentReaction_CreatedById",
                table: "UserCommentReaction");

            migrationBuilder.DropIndex(
                name: "IX_UserCommentReaction_DeletedById",
                table: "UserCommentReaction");

            migrationBuilder.DropIndex(
                name: "IX_UserCommentReaction_UpdatedById",
                table: "UserCommentReaction");

            migrationBuilder.DropIndex(
                name: "IX_Comments_SocialMediaCommentId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "UserPostReaction");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "UserPostReaction");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "UserPostReaction");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "UserPostReaction");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "UserPostReaction");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "UserPostReaction");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "UserPostComment");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "UserPostComment");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "UserPostComment");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "UserPostComment");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "UserPostComment");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "UserPostComment");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "UserCommentReaction");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "UserCommentReaction");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "UserCommentReaction");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "UserCommentReaction");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "UserCommentReaction");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "UserCommentReaction");

            migrationBuilder.DropColumn(
                name: "SocialMediaCommentId",
                table: "Comments");

            migrationBuilder.AddColumn<string>(
                name: "ParentId",
                table: "Comments",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ParentId",
                table: "Comments",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Comments_ParentId",
                table: "Comments",
                column: "ParentId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Comments_ParentId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_ParentId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Comments");

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "UserPostReaction",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "UserPostReaction",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedById",
                table: "UserPostReaction",
                type: "varchar(255)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "UserPostReaction",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedById",
                table: "UserPostReaction",
                type: "varchar(255)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "UserPostReaction",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "UserPostComment",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "UserPostComment",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedById",
                table: "UserPostComment",
                type: "varchar(255)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "UserPostComment",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedById",
                table: "UserPostComment",
                type: "varchar(255)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "UserPostComment",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "UserCommentReaction",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "UserCommentReaction",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedById",
                table: "UserCommentReaction",
                type: "varchar(255)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "UserCommentReaction",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedById",
                table: "UserCommentReaction",
                type: "varchar(255)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "UserCommentReaction",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SocialMediaCommentId",
                table: "Comments",
                type: "varchar(255)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_UserPostReaction_CreatedById",
                table: "UserPostReaction",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserPostReaction_DeletedById",
                table: "UserPostReaction",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserPostReaction_UpdatedById",
                table: "UserPostReaction",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserPostComment_CreatedById",
                table: "UserPostComment",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserPostComment_DeletedById",
                table: "UserPostComment",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserPostComment_UpdatedById",
                table: "UserPostComment",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserCommentReaction_CreatedById",
                table: "UserCommentReaction",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserCommentReaction_DeletedById",
                table: "UserCommentReaction",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserCommentReaction_UpdatedById",
                table: "UserCommentReaction",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_SocialMediaCommentId",
                table: "Comments",
                column: "SocialMediaCommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Comments_SocialMediaCommentId",
                table: "Comments",
                column: "SocialMediaCommentId",
                principalTable: "Comments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserCommentReaction_AspNetUsers_CreatedById",
                table: "UserCommentReaction",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserCommentReaction_AspNetUsers_DeletedById",
                table: "UserCommentReaction",
                column: "DeletedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserCommentReaction_AspNetUsers_UpdatedById",
                table: "UserCommentReaction",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPostComment_AspNetUsers_CreatedById",
                table: "UserPostComment",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPostComment_AspNetUsers_DeletedById",
                table: "UserPostComment",
                column: "DeletedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPostComment_AspNetUsers_UpdatedById",
                table: "UserPostComment",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPostReaction_AspNetUsers_CreatedById",
                table: "UserPostReaction",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPostReaction_AspNetUsers_DeletedById",
                table: "UserPostReaction",
                column: "DeletedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPostReaction_AspNetUsers_UpdatedById",
                table: "UserPostReaction",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
