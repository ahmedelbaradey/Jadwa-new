using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Fix_Cascading_ReolutionAttachment_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResolutionAttachments_Attachments_AttachmentId",
                table: "ResolutionAttachments");

            migrationBuilder.DropForeignKey(
                name: "FK_ResolutionAttachments_Resolutions_ResolutionId",
                table: "ResolutionAttachments");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "ResolutionAttachments",
                type: "datetime2",
                nullable: true,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ResolutionId",
                table: "ResolutionAttachments",
                type: "int",
                nullable: false,
                comment: "Foreign key reference to Resolution entity",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ResolutionAttachments",
                type: "bit",
                nullable: true,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "ResolutionAttachments",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<int>(
                name: "AttachmentId",
                table: "ResolutionAttachments",
                type: "int",
                nullable: false,
                comment: "Foreign key reference to Attachment entity",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_ResolutionAttachments_Resolution_Attachment_Unique",
                table: "ResolutionAttachments",
                columns: new[] { "ResolutionId", "AttachmentId" },
                unique: true,
                filter: "([IsDeleted] IS NULL)");

            migrationBuilder.AddForeignKey(
                name: "FK_ResolutionAttachments_Attachments",
                table: "ResolutionAttachments",
                column: "AttachmentId",
                principalTable: "Attachments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ResolutionAttachments_Resolutions",
                table: "ResolutionAttachments",
                column: "ResolutionId",
                principalTable: "Resolutions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResolutionAttachments_Attachments",
                table: "ResolutionAttachments");

            migrationBuilder.DropForeignKey(
                name: "FK_ResolutionAttachments_Resolutions",
                table: "ResolutionAttachments");

            migrationBuilder.DropIndex(
                name: "IX_ResolutionAttachments_Resolution_Attachment_Unique",
                table: "ResolutionAttachments");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "ResolutionAttachments",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<int>(
                name: "ResolutionId",
                table: "ResolutionAttachments",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Foreign key reference to Resolution entity");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ResolutionAttachments",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "ResolutionAttachments",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<int>(
                name: "AttachmentId",
                table: "ResolutionAttachments",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Foreign key reference to Attachment entity");

            migrationBuilder.AddForeignKey(
                name: "FK_ResolutionAttachments_Attachments_AttachmentId",
                table: "ResolutionAttachments",
                column: "AttachmentId",
                principalTable: "Attachments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ResolutionAttachments_Resolutions_ResolutionId",
                table: "ResolutionAttachments",
                column: "ResolutionId",
                principalTable: "Resolutions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
