using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mc2.CrudTest.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueConstraintForCustomerNameAndDob : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CustomerReadModels_FirstName_LastName_DateOfBirth",
                table: "CustomerReadModels");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerReadModels_FirstName_LastName_DateOfBirth",
                table: "CustomerReadModels",
                columns: new[] { "FirstName", "LastName", "DateOfBirth" },
                unique: true,
                filter: "[IsDeleted] = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CustomerReadModels_FirstName_LastName_DateOfBirth",
                table: "CustomerReadModels");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerReadModels_FirstName_LastName_DateOfBirth",
                table: "CustomerReadModels",
                columns: new[] { "FirstName", "LastName", "DateOfBirth" });
        }
    }
}
