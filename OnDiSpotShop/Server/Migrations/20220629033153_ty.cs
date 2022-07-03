using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnDiSpotShop.Server.Migrations
{
    public partial class ty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageUrl",
                value: "https://upload.wikimedia.org/wikipedia/en/b/bd/H2G2_UK_front_cover.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImageUrl",
                value: "https://upload.wikimedia.org/wikipedia/en/a/a4/Ready_Player_One_cover.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                column: "ImageUrl",
                value: "https://assets-prd.ignimgs.com/2021/11/12/unlocked520-blogroll-1636755236102.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11,
                column: "ImageUrl",
                value: "https://www.craftbymerlin.com/media/catalog/product/cache/60806c3b330fd240ef6156e64cd75b39/p/s/ps5-red-matte_1.png");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageUrl",
                value: "https://upload.wikimedia.org/wikipedia/commons/4/43/Xbox-console.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImageUrl",
                value: "https://upload.wikimedia.org/wikipedia/commons/8/82/NES-Console-Set.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                column: "ImageUrl",
                value: "https://upload.wikimedia.org/wikipedia/commons/4/43/Xbox-console.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11,
                column: "ImageUrl",
                value: "https://upload.wikimedia.org/wikipedia/commons/8/82/NES-Console-Set.jpg");
        }
    }
}
