using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SporSalonuYonetimi.Migrations
{
    /// <inheritdoc />
    public partial class AddIsConfirmedToAppointment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsConfirmed",
                table: "Appointments",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsConfirmed",
                table: "Appointments");
        }
    }
}
