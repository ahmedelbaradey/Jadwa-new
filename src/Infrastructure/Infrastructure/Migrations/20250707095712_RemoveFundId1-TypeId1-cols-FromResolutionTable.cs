using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveFundId1TypeId1colsFromResolutionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Resolutions_Funds_FundId1",
                table: "Resolutions");

            migrationBuilder.DropForeignKey(
                name: "FK_Resolutions_ResolutionTypes_ResolutionTypeId1",
                table: "Resolutions");

            migrationBuilder.DropIndex(
                name: "IX_Resolutions_FundId1",
                table: "Resolutions");

            migrationBuilder.DropIndex(
                name: "IX_Resolutions_ResolutionTypeId1",
                table: "Resolutions");

            migrationBuilder.DropColumn(
                name: "FundId1",
                table: "Resolutions");

            migrationBuilder.DropColumn(
                name: "ResolutionTypeId1",
                table: "Resolutions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FundId1",
                table: "Resolutions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ResolutionTypeId1",
                table: "Resolutions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Resolutions_FundId1",
                table: "Resolutions",
                column: "FundId1");

            migrationBuilder.CreateIndex(
                name: "IX_Resolutions_ResolutionTypeId1",
                table: "Resolutions",
                column: "ResolutionTypeId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Resolutions_Funds_FundId1",
                table: "Resolutions",
                column: "FundId1",
                principalTable: "Funds",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Resolutions_ResolutionTypes_ResolutionTypeId1",
                table: "Resolutions",
                column: "ResolutionTypeId1",
                principalTable: "ResolutionTypes",
                principalColumn: "Id");
        }
    }
}
