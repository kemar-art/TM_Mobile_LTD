using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnDiSpotShop.Server.Migrations
{
    public partial class Seeddatat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "ImageUrl", "Name", "Price" },
                values: new object[] { 1, "The iPhone X (Roman numeral X pronounced ten, also known as iPhone 10)[11][12] is a smartphone designed, developed and marketed by Apple Inc. The 11th generation of the iPhone, it was available to pre-order on October 27, 2017, and was released on November 3, 2017. The naming of the iPhone X (skipping the iPhone 9) is to mark the 10th anniversary of the iPhone. ", "https://upload.wikimedia.org/wikipedia/commons/3/32/IPhone_X_vector.svg", "iPhone X", 100m });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "ImageUrl", "Name", "Price" },
                values: new object[] { 2, "A laptop, laptop computer, or notebook computer is a small, portable personal computer (PC) with a screen and alphanumeric keyboard. Laptops typically have a clam shell form factor with the screen mounted on the inside of the upper lid and the keyboard on the inside of the lower lid, although 2-in-1 PCs with a detachable keyboard are often marketed as laptops or as having a laptop mode. Laptops are folded shut for transportation, and thus are suitable for mobile use.[1] Its name comes from lap, as it was deemed practical to be placed on a person's lap when being used. Today, laptops are used in a variety of settings, such as at work, in education, for playing games, web browsing, for personal multimedia, and for general home computer use. ", "https://upload.wikimedia.org/wikipedia/commons/5/50/Macbook_Air_M1_Silver_PNG.png", "Laptop", 100m });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "ImageUrl", "Name", "Price" },
                values: new object[] { 3, "The Samsung Galaxy Note 10 (stylized as Samsung Galaxy Note10) is a line of Android-based phablets designed, developed, produced, and marketed by Samsung Electronics as part of the Samsung Galaxy Note series. They were unveiled on 7 August 2019, as the successors to the Samsung Galaxy Note 9.[3] Details about the phablets were widely leaked in the months leading up to the phablets' announcement.", "https://upload.wikimedia.org/wikipedia/commons/a/aa/Samsung_Galaxy_Phone.jpg", "Samsung Galaxy Note 10", 100m });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
