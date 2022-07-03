using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnDiSpotShop.Server.Migrations
{
    public partial class ChangeConsolesImageLinksforboth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "ImageUrl", "Name" },
                values: new object[] { "https://upload.wikimedia.org/wikipedia/commons/4/43/Xbox-console.jpg", "Xbox" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Description", "ImageUrl", "Name" },
                values: new object[] { "The Super Nintendo Entertainment System (SNES), also known as the Super NES or Super Nintendo, is a 16-bit home video game console developed by Nintendo that was released in 1990 in Japan and South Korea.", "https://upload.wikimedia.org/wikipedia/commons/e/ee/Nintendo-Super-Famicom-Set-FL.jpg", "Super Nintendo Entertainment System" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "ImageUrl", "Name" },
                values: new object[] { "https://static-01.daraz.pk/p/a0f3fa1f5d4e66e617ded6aa2074979b.jpg", "Xbox360" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Description", "ImageUrl", "Name" },
                values: new object[] { "PlayStation 5 Entertainment System (PS5), also known as the Super NES or Super Nintendo, is a 16-bit home video game console developed by Nintendo that was released in 1990 in Japan and South Korea.", "https://assets-prd.ignimgs.com/2022/05/26/ps5-memorial-day-stock-at-walmart-june-2022-ign-1653563010113.png", "PlayStation 5 Entertainment System" });
        }
    }
}
