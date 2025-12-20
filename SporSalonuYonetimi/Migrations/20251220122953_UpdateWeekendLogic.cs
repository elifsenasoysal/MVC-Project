using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SporSalonuYonetimi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateWeekendLogic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OpenHour",
                table: "SalonConfigs",
                newName: "WeekendMorningStart");

            migrationBuilder.RenameColumn(
                name: "IsWeekendOpen",
                table: "SalonConfigs",
                newName: "IsSundayOpen");

            migrationBuilder.RenameColumn(
                name: "CloseHour",
                table: "SalonConfigs",
                newName: "WeekendMorningEnd");

            migrationBuilder.AddColumn<int>(
                name: "WeekDayEveningEnd",
                table: "SalonConfigs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WeekDayEveningStart",
                table: "SalonConfigs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WeekDayMorningEnd",
                table: "SalonConfigs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WeekDayMorningStart",
                table: "SalonConfigs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WeekendEveningEnd",
                table: "SalonConfigs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WeekendEveningStart",
                table: "SalonConfigs",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WeekDayEveningEnd",
                table: "SalonConfigs");

            migrationBuilder.DropColumn(
                name: "WeekDayEveningStart",
                table: "SalonConfigs");

            migrationBuilder.DropColumn(
                name: "WeekDayMorningEnd",
                table: "SalonConfigs");

            migrationBuilder.DropColumn(
                name: "WeekDayMorningStart",
                table: "SalonConfigs");

            migrationBuilder.DropColumn(
                name: "WeekendEveningEnd",
                table: "SalonConfigs");

            migrationBuilder.DropColumn(
                name: "WeekendEveningStart",
                table: "SalonConfigs");

            migrationBuilder.RenameColumn(
                name: "WeekendMorningStart",
                table: "SalonConfigs",
                newName: "OpenHour");

            migrationBuilder.RenameColumn(
                name: "WeekendMorningEnd",
                table: "SalonConfigs",
                newName: "CloseHour");

            migrationBuilder.RenameColumn(
                name: "IsSundayOpen",
                table: "SalonConfigs",
                newName: "IsWeekendOpen");
        }
    }
}
