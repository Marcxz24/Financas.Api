using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace Financas.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddPagamentoFatura : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "pagamentos_fatura",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    fatura_id = table.Column<int>(type: "int", nullable: false),
                    valor_pago = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    data_pagamento = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    conta_bancaria_id = table.Column<int>(type: "int", nullable: true),
                    observacao = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pagamentos_fatura", x => x.Id);
                    table.ForeignKey(
                        name: "FK_pagamentos_fatura_contas_bancarias_conta_bancaria_id",
                        column: x => x.conta_bancaria_id,
                        principalTable: "contas_bancarias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_pagamentos_fatura_faturas_fatura_id",
                        column: x => x.fatura_id,
                        principalTable: "faturas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_pagamentos_fatura_conta_bancaria_id",
                table: "pagamentos_fatura",
                column: "conta_bancaria_id");

            migrationBuilder.CreateIndex(
                name: "IX_pagamentos_fatura_fatura_id",
                table: "pagamentos_fatura",
                column: "fatura_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "pagamentos_fatura");
        }
    }
}
