using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mc2.CrudTest.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEmailIndexForSoftDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CustomerReadModels_Email",
                table: "CustomerReadModels");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerReadModels_Email",
                table: "CustomerReadModels",
                column: "Email",
                unique: true,
                filter: "[IsDeleted] = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CustomerReadModels_Email",
                table: "CustomerReadModels");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerReadModels_Email",
                table: "CustomerReadModels",
                column: "Email",
                unique: true);
        }
    }
}
