using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PH48831_C5_ASM.Migrations
{
    /// <inheritdoc />
    public partial class _9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TrangThaiHoaDonIdTrangThai",
                table: "HoaDons",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HoaDons_TrangThaiHoaDonIdTrangThai",
                table: "HoaDons",
                column: "TrangThaiHoaDonIdTrangThai");

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDons_TrangThaiHoaDons_TrangThaiHoaDonIdTrangThai",
                table: "HoaDons",
                column: "TrangThaiHoaDonIdTrangThai",
                principalTable: "TrangThaiHoaDons",
                principalColumn: "IdTrangThai");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HoaDons_TrangThaiHoaDons_TrangThaiHoaDonIdTrangThai",
                table: "HoaDons");

            migrationBuilder.DropIndex(
                name: "IX_HoaDons_TrangThaiHoaDonIdTrangThai",
                table: "HoaDons");

            migrationBuilder.DropColumn(
                name: "TrangThaiHoaDonIdTrangThai",
                table: "HoaDons");
        }
    }
}
