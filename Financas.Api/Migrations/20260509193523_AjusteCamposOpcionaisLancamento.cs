using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Financas.Api.Migrations
{
    /// <inheritdoc />
    public partial class AjusteCamposOpcionaisLancamento : Migration
    {
        /// <summary>
        /// Documentação de Sincronização:
        /// Esta migration foi gerada para alinhar o modelo C# com as alterações manuais feitas no MySQL Workbench.
        /// 
        /// COMANDO EXECUTADO NO WORKBENCH:
        /// ALTER TABLE lancamentos MODIFY COLUMN data_lancamento DATETIME;
        /// 
        /// MOTIVO: Garantir que a coluna suporte o armazenamento de horas/minutos.
        /// </summary>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // O método Up permanece vazio porque a alteração estrutural já foi 
            // aplicada manualmente para resolver o problema de tipagem e nulabilidade.
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Reversão lógica: Voltaria o campo para o estado anterior (DATE ou TIMESTAMP)
        }
    }
}
