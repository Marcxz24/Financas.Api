using Financas.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Financas.Api.Data.Configurations
{
    /// <summary>
    /// Configuração de mapeamento da entidade Fatura para o banco de dados (Fluent API).
    /// Define regras de nomenclatura, tipos de dados, índices e relacionamentos.
    /// </summary>
    public class FaturaConfiguration : IEntityTypeConfiguration<Fatura>
    {
        public void Configure(EntityTypeBuilder<Fatura> builder)
        {
            // Define o nome físico da tabela no banco de dados.
            builder.ToTable("faturas");

            // Chave primária.
            builder.HasKey(f => f.Id);

            // Mapeamento de propriedades.
            builder.Property(f => f.CartaoCreditoId)
                .HasColumnName("cartao_credito_id")
                .IsRequired();

            // 2. Data de Início: Define a data de início da fatura.
            builder.Property(f => f.DataInicio)
                .HasColumnName("data_inicio")
                .IsRequired();

            // 3. Data de Fechamento: Define a data de fechamento da fatura.
            builder.Property(f => f.DataFechamento)
                .HasColumnName("data_fechamento")
                .IsRequired();

            // 4. Data de Vencimento: Define a data de vencimento da fatura.
            builder.Property(f => f.DataVencimento)
                .HasColumnName("data_vencimento")
                .IsRequired();

            // 5. Valor Total: Define o valor total da fatura.
            builder.Property(f => f.ValorTotal)
                .HasColumnName("valor_total")
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            // 6. Valor Pago: Define o valor pago da fatura.
            builder.Property(f => f.ValorPago)
                .HasColumnName("valor_pago")
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            // 7. Status da Fatura: Converte o Enum FaturaStatus para inteiro ao salvar no banco.
            builder.Property(f => f.Status)
                .HasColumnName("status_fatura")
                .HasConversion<int>()
                .IsRequired();

            // 8. Índices para busca por cartão/ciclo.
            builder.HasIndex(f => f.CartaoCreditoId);
            builder.HasIndex(f => new { f.CartaoCreditoId, f.DataInicio, f.DataFechamento });

            // 9. Relacionamento com Cartão de Crédito: Vincula a fatura ao cartão de crédito.
            // O uso do Restrict garante que você não delete um cartão de crédito que ainda possua 
            // faturas, preservando a rastreabilidade financeira.
            builder.HasOne(f => f.CartaoCredito)
                .WithMany(c => c.Faturas)
                .HasForeignKey(f => f.CartaoCreditoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
