using Microsoft.EntityFrameworkCore.Migrations;

namespace PartWebApp2.Migrations
{
    public partial class ne : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Party_Area_areaId",
                table: "Party");

            migrationBuilder.DropForeignKey(
                name: "FK_Party_Genre_genreId",
                table: "Party");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Genre",
                table: "Genre");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Area",
                table: "Area");

            migrationBuilder.RenameTable(
                name: "Genre",
                newName: "Genres");

            migrationBuilder.RenameTable(
                name: "Area",
                newName: "Areas");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Genres",
                table: "Genres",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Areas",
                table: "Areas",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Party_Areas_areaId",
                table: "Party",
                column: "areaId",
                principalTable: "Areas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Party_Genres_genreId",
                table: "Party",
                column: "genreId",
                principalTable: "Genres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Party_Areas_areaId",
                table: "Party");

            migrationBuilder.DropForeignKey(
                name: "FK_Party_Genres_genreId",
                table: "Party");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Genres",
                table: "Genres");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Areas",
                table: "Areas");

            migrationBuilder.RenameTable(
                name: "Genres",
                newName: "Genre");

            migrationBuilder.RenameTable(
                name: "Areas",
                newName: "Area");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Genre",
                table: "Genre",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Area",
                table: "Area",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Party_Area_areaId",
                table: "Party",
                column: "areaId",
                principalTable: "Area",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Party_Genre_genreId",
                table: "Party",
                column: "genreId",
                principalTable: "Genre",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
