using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Migrations
{
    /// <inheritdoc />
    public partial class UserPostRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_CreatedById",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_FriendRequests_AspNetUsers_CreatedById",
                table: "FriendRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_PostSocialMediaUser_AspNetUsers_TaggedUsersId",
                table: "PostSocialMediaUser");

            migrationBuilder.DropForeignKey(
                name: "FK_PostSocialMediaUser_Posts_PostsId",
                table: "PostSocialMediaUser");

            migrationBuilder.DropForeignKey(
                name: "FK_Reactions_AspNetUsers_CreatedById",
                table: "Reactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostSocialMediaUser",
                table: "PostSocialMediaUser");

            migrationBuilder.RenameTable(
                name: "PostSocialMediaUser",
                newName: "PostTaggedUsers");

            migrationBuilder.RenameColumn(
                name: "PostsId",
                table: "PostTaggedUsers",
                newName: "TaggedPostsId");

            migrationBuilder.RenameIndex(
                name: "IX_PostSocialMediaUser_TaggedUsersId",
                table: "PostTaggedUsers",
                newName: "IX_PostTaggedUsers_TaggedUsersId");

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
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "UserPostReaction",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedById",
                table: "UserPostReaction",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "UserPostReaction",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

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
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "UserPostComment",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedById",
                table: "UserPostComment",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "UserPostComment",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

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
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "UserCommentReaction",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedById",
                table: "UserCommentReaction",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "UserCommentReaction",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Tags",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Tags",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedById",
                table: "Tags",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Tags",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedById",
                table: "Tags",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "Tags",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Reactions",
                keyColumn: "CreatedById",
                keyValue: null,
                column: "CreatedById",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "Reactions",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "DeletedById",
                table: "Reactions",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "UpdatedById",
                table: "Reactions",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Posts",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Posts",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedById",
                table: "Posts",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Posts",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "SocialMediaUserId",
                table: "Posts",
                type: "varchar(255)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "UpdatedById",
                table: "Posts",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "Posts",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "FriendRequests",
                keyColumn: "CreatedById",
                keyValue: null,
                column: "CreatedById",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "FriendRequests",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "DeletedById",
                table: "FriendRequests",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "UpdatedById",
                table: "FriendRequests",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "CreatedById",
                keyValue: null,
                column: "CreatedById",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "Comments",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "DeletedById",
                table: "Comments",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "UpdatedById",
                table: "Comments",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostTaggedUsers",
                table: "PostTaggedUsers",
                columns: new[] { "TaggedPostsId", "TaggedUsersId" });

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
                name: "IX_Tags_CreatedById",
                table: "Tags",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_DeletedById",
                table: "Tags",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_UpdatedById",
                table: "Tags",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_DeletedById",
                table: "Reactions",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_UpdatedById",
                table: "Reactions",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_CreatedById",
                table: "Posts",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_DeletedById",
                table: "Posts",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_SocialMediaUserId",
                table: "Posts",
                column: "SocialMediaUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_UpdatedById",
                table: "Posts",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_FriendRequests_DeletedById",
                table: "FriendRequests",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_FriendRequests_UpdatedById",
                table: "FriendRequests",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_DeletedById",
                table: "Comments",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UpdatedById",
                table: "Comments",
                column: "UpdatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_CreatedById",
                table: "Comments",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_DeletedById",
                table: "Comments",
                column: "DeletedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_UpdatedById",
                table: "Comments",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FriendRequests_AspNetUsers_CreatedById",
                table: "FriendRequests",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FriendRequests_AspNetUsers_DeletedById",
                table: "FriendRequests",
                column: "DeletedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FriendRequests_AspNetUsers_UpdatedById",
                table: "FriendRequests",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_AspNetUsers_CreatedById",
                table: "Posts",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_AspNetUsers_DeletedById",
                table: "Posts",
                column: "DeletedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_AspNetUsers_SocialMediaUserId",
                table: "Posts",
                column: "SocialMediaUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_AspNetUsers_UpdatedById",
                table: "Posts",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PostTaggedUsers_AspNetUsers_TaggedUsersId",
                table: "PostTaggedUsers",
                column: "TaggedUsersId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostTaggedUsers_Posts_TaggedPostsId",
                table: "PostTaggedUsers",
                column: "TaggedPostsId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reactions_AspNetUsers_CreatedById",
                table: "Reactions",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reactions_AspNetUsers_DeletedById",
                table: "Reactions",
                column: "DeletedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reactions_AspNetUsers_UpdatedById",
                table: "Reactions",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_AspNetUsers_CreatedById",
                table: "Tags",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_AspNetUsers_DeletedById",
                table: "Tags",
                column: "DeletedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_AspNetUsers_UpdatedById",
                table: "Tags",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserCommentReaction_AspNetUsers_UpdatedById",
                table: "UserCommentReaction",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPostComment_AspNetUsers_UpdatedById",
                table: "UserPostComment",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPostReaction_AspNetUsers_UpdatedById",
                table: "UserPostReaction",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_CreatedById",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_DeletedById",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_UpdatedById",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_FriendRequests_AspNetUsers_CreatedById",
                table: "FriendRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_FriendRequests_AspNetUsers_DeletedById",
                table: "FriendRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_FriendRequests_AspNetUsers_UpdatedById",
                table: "FriendRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_AspNetUsers_CreatedById",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_AspNetUsers_DeletedById",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_AspNetUsers_SocialMediaUserId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_AspNetUsers_UpdatedById",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_PostTaggedUsers_AspNetUsers_TaggedUsersId",
                table: "PostTaggedUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_PostTaggedUsers_Posts_TaggedPostsId",
                table: "PostTaggedUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Reactions_AspNetUsers_CreatedById",
                table: "Reactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Reactions_AspNetUsers_DeletedById",
                table: "Reactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Reactions_AspNetUsers_UpdatedById",
                table: "Reactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Tags_AspNetUsers_CreatedById",
                table: "Tags");

            migrationBuilder.DropForeignKey(
                name: "FK_Tags_AspNetUsers_DeletedById",
                table: "Tags");

            migrationBuilder.DropForeignKey(
                name: "FK_Tags_AspNetUsers_UpdatedById",
                table: "Tags");

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
                name: "IX_Tags_CreatedById",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_DeletedById",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_UpdatedById",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Reactions_DeletedById",
                table: "Reactions");

            migrationBuilder.DropIndex(
                name: "IX_Reactions_UpdatedById",
                table: "Reactions");

            migrationBuilder.DropIndex(
                name: "IX_Posts_CreatedById",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_DeletedById",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_SocialMediaUserId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_UpdatedById",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_FriendRequests_DeletedById",
                table: "FriendRequests");

            migrationBuilder.DropIndex(
                name: "IX_FriendRequests_UpdatedById",
                table: "FriendRequests");

            migrationBuilder.DropIndex(
                name: "IX_Comments_DeletedById",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_UpdatedById",
                table: "Comments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostTaggedUsers",
                table: "PostTaggedUsers");

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
                name: "CreatedById",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "Reactions");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "Reactions");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "SocialMediaUserId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "FriendRequests");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "FriendRequests");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "Comments");

            migrationBuilder.RenameTable(
                name: "PostTaggedUsers",
                newName: "PostSocialMediaUser");

            migrationBuilder.RenameColumn(
                name: "TaggedPostsId",
                table: "PostSocialMediaUser",
                newName: "PostsId");

            migrationBuilder.RenameIndex(
                name: "IX_PostTaggedUsers_TaggedUsersId",
                table: "PostSocialMediaUser",
                newName: "IX_PostSocialMediaUser_TaggedUsersId");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "Reactions",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "FriendRequests",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "Comments",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostSocialMediaUser",
                table: "PostSocialMediaUser",
                columns: new[] { "PostsId", "TaggedUsersId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_CreatedById",
                table: "Comments",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FriendRequests_AspNetUsers_CreatedById",
                table: "FriendRequests",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PostSocialMediaUser_AspNetUsers_TaggedUsersId",
                table: "PostSocialMediaUser",
                column: "TaggedUsersId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostSocialMediaUser_Posts_PostsId",
                table: "PostSocialMediaUser",
                column: "PostsId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reactions_AspNetUsers_CreatedById",
                table: "Reactions",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
