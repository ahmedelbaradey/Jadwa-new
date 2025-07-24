using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _AddHasFilter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Resolutions_Code_Unique",
                table: "Resolutions");

            migrationBuilder.CreateIndex(
                name: "IX_Resolutions_Code_Unique",
                table: "Resolutions",
                column: "Code",
                unique: true,
                filter: "[IsDeleted] = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Resolutions_Code_Unique",
                table: "Resolutions");

            migrationBuilder.CreateIndex(
                name: "IX_Resolutions_Code_Unique",
                table: "Resolutions",
                column: "Code",
                unique: true);
        }
    }
}
