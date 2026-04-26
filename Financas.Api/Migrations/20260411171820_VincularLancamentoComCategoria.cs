using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Financas.Api.Migrations
{
    /// <inheritdoc />
    public partial class VincularLancamentoComCategoria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "categoria_id",
                table: "lancamentos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_lancamentos_categoria_id",
                table: "lancamentos",
                column: "categoria_id");

            migrationBuilder.AddForeignKey(
                name: "FK_lancamentos_categorias_categoria_id",
                table: "lancamentos",
                column: "categoria_id",
                principalTable: "categorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_lancamentos_categorias_categoria_id",
                table: "lancamentos");

            migrationBuilder.DropIndex(
                name: "IX_lancamentos_categoria_id",
                table: "lancamentos");

            migrationBuilder.DropColumn(
                name: "categoria_id",
                table: "lancamentos");
        }
    }
}
