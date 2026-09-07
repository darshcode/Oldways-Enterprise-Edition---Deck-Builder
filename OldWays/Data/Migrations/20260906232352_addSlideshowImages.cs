using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OldWays.Data.Migrations
{
    /// <inheritdoc />
    public partial class addSlideshowImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SlideshowImage_Slideshows_SlideshowId",
                table: "SlideshowImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SlideshowImage",
                table: "SlideshowImage");

            migrationBuilder.RenameTable(
                name: "SlideshowImage",
                newName: "SlideshowImages");

            migrationBuilder.RenameIndex(
                name: "IX_SlideshowImage_SlideshowId",
                table: "SlideshowImages",
                newName: "IX_SlideshowImages_SlideshowId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SlideshowImages",
                table: "SlideshowImages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SlideshowImages_Slideshows_SlideshowId",
                table: "SlideshowImages",
                column: "SlideshowId",
                principalTable: "Slideshows",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SlideshowImages_Slideshows_SlideshowId",
                table: "SlideshowImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SlideshowImages",
                table: "SlideshowImages");

            migrationBuilder.RenameTable(
                name: "SlideshowImages",
                newName: "SlideshowImage");

            migrationBuilder.RenameIndex(
                name: "IX_SlideshowImages_SlideshowId",
                table: "SlideshowImage",
                newName: "IX_SlideshowImage_SlideshowId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SlideshowImage",
                table: "SlideshowImage",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SlideshowImage_Slideshows_SlideshowId",
                table: "SlideshowImage",
                column: "SlideshowId",
                principalTable: "Slideshows",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
