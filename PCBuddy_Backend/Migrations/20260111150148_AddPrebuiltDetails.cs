using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PCBuddy_Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddPrebuiltDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "PrebuiltPCs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "PrebuiltPCs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "PrebuiltPCs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "PrebuiltPCs");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "PrebuiltPCs");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "PrebuiltPCs");
        }
    }
}
