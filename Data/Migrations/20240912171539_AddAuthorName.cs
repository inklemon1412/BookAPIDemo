using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookAPIDemo.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAuthorName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Author");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Author",
                newName: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Author",
                newName: "FirstName");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Author",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
