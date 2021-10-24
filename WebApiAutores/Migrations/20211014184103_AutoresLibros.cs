using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiAutores.Migrations
{
    public partial class AutoresLibros : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "autorId",
                table: "Comentarios",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AutoresLibros",
                columns: table => new
                {
                    LibroId = table.Column<int>(type: "int", nullable: false),
                    AutorId = table.Column<int>(type: "int", nullable: false),
                    Orden = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutoresLibros", x => new { x.AutorId, x.LibroId });
                    table.ForeignKey(
                        name: "FK_AutoresLibros_Autores_AutorId",
                        column: x => x.AutorId,
                        principalTable: "Autores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AutoresLibros_Libros_LibroId",
                        column: x => x.LibroId,
                        principalTable: "Libros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comentarios_autorId",
                table: "Comentarios",
                column: "autorId");

            migrationBuilder.CreateIndex(
                name: "IX_AutoresLibros_LibroId",
                table: "AutoresLibros",
                column: "LibroId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comentarios_Autores_autorId",
                table: "Comentarios",
                column: "autorId",
                principalTable: "Autores",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comentarios_Autores_autorId",
                table: "Comentarios");

            migrationBuilder.DropTable(
                name: "AutoresLibros");

            migrationBuilder.DropIndex(
                name: "IX_Comentarios_autorId",
                table: "Comentarios");

            migrationBuilder.DropColumn(
                name: "autorId",
                table: "Comentarios");
        }
    }
}
