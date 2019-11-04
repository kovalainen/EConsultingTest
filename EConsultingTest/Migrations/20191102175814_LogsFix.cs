using Microsoft.EntityFrameworkCore.Migrations;

namespace EConsultingTest.Migrations
{
    public partial class LogsFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "Logs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Message",
                table: "Logs");
        }
    }
}
