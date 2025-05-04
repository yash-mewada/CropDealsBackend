using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CropDeals.Migrations
{
    /// <inheritdoc />
    public partial class ChangeImageUrlToImageBase64InCropListing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "CropListings",
                newName: "ImageBase64");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageBase64",
                table: "CropListings",
                newName: "ImageUrl");
        }
    }
}
