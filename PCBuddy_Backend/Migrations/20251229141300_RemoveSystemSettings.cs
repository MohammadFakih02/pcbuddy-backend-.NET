using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PCBuddy_Backend.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSystemSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SystemSettings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SystemSettings",
                columns: table => new
                {
                    SettingKey = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SettingValue = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemSettings", x => x.SettingKey);
                });

            migrationBuilder.InsertData(
                table: "SystemSettings",
                columns: new[] { "SettingKey", "SettingValue" },
                values: new object[] { "DataVersion", "1.0.0" });
        }
    }
}
