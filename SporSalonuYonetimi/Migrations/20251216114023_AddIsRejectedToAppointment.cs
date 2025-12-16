using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SporSalonuYonetimi.Migrations
{
    /// <inheritdoc />
    public partial class AddIsRejectedToAppointment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRejected",
                table: "Appointments",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRejected",
                table: "Appointments");
        }
    }
}
