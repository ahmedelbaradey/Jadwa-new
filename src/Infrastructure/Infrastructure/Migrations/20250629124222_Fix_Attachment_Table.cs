using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Fix_Attachment_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ActionName",
                table: "ResolutionStatusHistories",
                newName: "UserRole");

            migrationBuilder.RenameColumn(
                name: "VotingMethodology",
                table: "Resolutions",
                newName: "VotingType");

            migrationBuilder.AddColumn<int>(
                name: "Action",
                table: "ResolutionStatusHistories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Reason",
                table: "ResolutionStatusHistories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "Attachments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "FileSize",
                table: "Attachments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Action",
                table: "ResolutionStatusHistories");

            migrationBuilder.DropColumn(
                name: "Reason",
                table: "ResolutionStatusHistories");

            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "FileSize",
                table: "Attachments");

            migrationBuilder.RenameColumn(
                name: "UserRole",
                table: "ResolutionStatusHistories",
                newName: "ActionName");

            migrationBuilder.RenameColumn(
                name: "VotingType",
                table: "Resolutions",
                newName: "VotingMethodology");
        }
    }
}
