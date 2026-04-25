using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace Financas.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddCartaoEFAndFaturaConfigurations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "cartao_credito_id",
                table: "lancamentos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "fatura_id",
                table: "lancamentos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "cartao_credito",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    usuario_id = table.Column<int>(type: "int", nullable: false),
                    nome_cartao_credito = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    limite_cartao_credito = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    dia_fechamento = table.Column<int>(type: "int", nullable: false),
                    dia_vencimento = table.Column<int>(type: "int", nullable: false),
                    status_cartao_credito = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cartao_credito", x => x.Id);
                    table.ForeignKey(
                        name: "FK_cartao_credito_usuarios_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "faturas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    cartao_credito_id = table.Column<int>(type: "int", nullable: false),
                    data_inicio = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    data_fechamento = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    data_vencimento = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    valor_total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    valor_pago = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    status_fatura = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_faturas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_faturas_cartao_credito_cartao_credito_id",
                        column: x => x.cartao_credito_id,
                        principalTable: "cartao_credito",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_lancamentos_cartao_credito_id",
                table: "lancamentos",
                column: "cartao_credito_id");

            migrationBuilder.CreateIndex(
                name: "IX_lancamentos_fatura_id",
                table: "lancamentos",
                column: "fatura_id");

            migrationBuilder.CreateIndex(
                name: "IX_cartao_credito_usuario_id",
                table: "cartao_credito",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_faturas_cartao_credito_id",
                table: "faturas",
                column: "cartao_credito_id");

            migrationBuilder.CreateIndex(
                name: "IX_faturas_cartao_credito_id_data_inicio_data_fechamento",
                table: "faturas",
                columns: new[] { "cartao_credito_id", "data_inicio", "data_fechamento" });

            migrationBuilder.AddForeignKey(
                name: "FK_lancamentos_cartao_credito_cartao_credito_id",
                table: "lancamentos",
                column: "cartao_credito_id",
                principalTable: "cartao_credito",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_lancamentos_faturas_fatura_id",
                table: "lancamentos",
                column: "fatura_id",
                principalTable: "faturas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_lancamentos_cartao_credito_cartao_credito_id",
                table: "lancamentos");

            migrationBuilder.DropForeignKey(
                name: "FK_lancamentos_faturas_fatura_id",
                table: "lancamentos");

            migrationBuilder.DropTable(
                name: "faturas");

            migrationBuilder.DropTable(
                name: "cartao_credito");

            migrationBuilder.DropIndex(
                name: "IX_lancamentos_cartao_credito_id",
                table: "lancamentos");

            migrationBuilder.DropIndex(
                name: "IX_lancamentos_fatura_id",
                table: "lancamentos");

            migrationBuilder.DropColumn(
                name: "cartao_credito_id",
                table: "lancamentos");

            migrationBuilder.DropColumn(
                name: "fatura_id",
                table: "lancamentos");
        }
    }
}
