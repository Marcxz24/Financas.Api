using Financas.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Financas.Api.Data.Configurations
{
    public class PagamentoFaturaConfiguration : IEntityTypeConfiguration<PagamentoFatura>
    {
        public void Configure(EntityTypeBuilder<PagamentoFatura> builder)
        {
            // Nome da tabela
            builder.ToTable("pagamentos_fatura");

            // Chave primária
            builder.HasKey(p => p.Id);

            // FK Fatura
            builder.Property(p => p.FaturaId)
                .HasColumnName("fatura_id")
                .IsRequired();

            // Valor pago
            builder.Property(p => p.ValorPago)
                .HasColumnName("valor_pago")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            // Data pagamento
            builder.Property(p => p.DataPagamento)
                .HasColumnName("data_pagamento")
                .IsRequired();

            // FK Conta (opcional)
            builder.Property(p => p.ContaBancariaId)
                .HasColumnName("conta_bancaria_id")
                .IsRequired(false);

            // Observação
            builder.Property(p => p.Observacao)
                .HasColumnName("observacao")
                .HasMaxLength(500)
                .IsRequired(false);

            // Relacionamento com Fatura (1:N)
            builder.HasOne(p => p.Fatura)
                .WithMany(f => f.Pagamentos)
                .HasForeignKey(p => p.FaturaId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relacionamento com Conta Bancária (opcional)
            builder.HasOne(p => p.ContaBancaria)
                .WithMany()
                .HasForeignKey(p => p.ContaBancariaId)
                .OnDelete(DeleteBehavior.Restrict);

            // Índice para performance
            builder.HasIndex(p => p.FaturaId);
        }
    }
}
