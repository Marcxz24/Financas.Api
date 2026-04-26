using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Financas.Api.Migrations
{
    public partial class CorrigirNomeContaBancaria : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Remove a FK (Este passo funcionou no log anterior, continue assim)
            // migrationBuilder.Sql("ALTER TABLE lancamentos DROP FOREIGN KEY FK_lancamentos_contas_bancarias_ContaBancariaId;");

            // 2. SQL MANUAL para MySQL 5.7: Usa CHANGE em vez de RENAME
            // É necessário repetir o tipo (INT) e se aceita NULL (neste caso, sim, pois é int?)
            migrationBuilder.Sql("ALTER TABLE lancamentos CHANGE COLUMN ContaBancariaId conta_bancaria_id INT NULL;");

            // 3. Renomeia o Índice
            // No MySQL 5.7, se o RENAME INDEX falhar, você pode usar DROP INDEX e CREATE INDEX
            migrationBuilder.Sql("ALTER TABLE lancamentos RENAME INDEX IX_lancamentos_ContaBancariaId TO IX_lancamentos_conta_bancaria_id;");

            // 4. Adiciona a FK novamente
            migrationBuilder.AddForeignKey(
                name: "FK_lancamentos_contas_bancarias_conta_bancaria_id",
                table: "lancamentos",
                column: "conta_bancaria_id",
                principalTable: "contas_bancarias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_lancamentos_contas_bancarias_conta__id",
                table: "lancamentos");

            // Reverte via SQL Manual
            migrationBuilder.Sql("ALTER TABLE lancamentos RENAME COLUMN conta_bancaria_id TO ContaBancariaId;");
            migrationBuilder.Sql("ALTER TABLE lancamentos RENAME INDEX IX_lancamentos_conta_bancaria_id TO IX_lancamentos_ContaBancariaId;");

            migrationBuilder.AddForeignKey(
                name: "FK_lancamentos_contas_bancarias_ContaBancariaId",
                table: "lancamentos",
                column: "ContaBancariaId",
                principalTable: "contas_bancarias",
                principalColumn: "Id");
        }
    }
}