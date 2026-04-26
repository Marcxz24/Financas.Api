using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace Financas.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddContasBancarias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ContaBancariaId",
                table: "lancamentos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "contas_bancarias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    usuario_id = table.Column<int>(type: "int", nullable: false),
                    nome_conta_bancaria = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    tipo_conta_bancaria = table.Column<int>(type: "int", nullable: false),
                    saldo_conta_bancaria = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contas_bancarias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_contas_bancarias_usuarios_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_lancamentos_ContaBancariaId",
                table: "lancamentos",
                column: "ContaBancariaId");

            migrationBuilder.CreateIndex(
                name: "IX_contas_bancarias_usuario_id",
                table: "contas_bancarias",
                column: "usuario_id");

            migrationBuilder.AddForeignKey(
                name: "FK_lancamentos_contas_bancarias_ContaBancariaId",
                table: "lancamentos",
                column: "ContaBancariaId",
                principalTable: "contas_bancarias",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_lancamentos_contas_bancarias_ContaBancariaId",
                table: "lancamentos");

            migrationBuilder.DropTable(
                name: "contas_bancarias");

            migrationBuilder.DropIndex(
                name: "IX_lancamentos_ContaBancariaId",
                table: "lancamentos");

            migrationBuilder.DropColumn(
                name: "ContaBancariaId",
                table: "lancamentos");
        }
    }
}
