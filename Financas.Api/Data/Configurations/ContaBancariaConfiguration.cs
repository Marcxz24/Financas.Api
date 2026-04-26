using Financas.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Financas.Api.Data.Configurations
{
    /// <summary>
    /// Configuração de mapeamento da entidade ContaBancaria para o banco de dados (Fluent API).
    /// Define regras de nomenclatura, tipos de dados, índices e relacionamentos.
    /// </summary>
    public class ContaBancariaConfiguration : IEntityTypeConfiguration<ContaBancaria>
    {
        public void Configure(EntityTypeBuilder<ContaBancaria> builder)
        {
            // 1. Tabela: Define o nome físico da tabela no banco de dados.
            builder.ToTable("contas_bancarias");

            // 2. Chave Primária: Define o campo Id como o identificador único da tabela.
            builder.HasKey(key => key.Id);

            // 3. Chave Estrangeira (Propriedade): Mapeia o nome da coluna e obriga a presença de um usuário.
            builder.Property(fkUser => fkUser.UsuarioId)
                    .HasColumnName("usuario_id")
                    .IsRequired();

            // 4. Nome da Conta: Define limite de caracteres (100) para evitar desperdício de espaço no MySQL.
            builder.Property(p => p.Nome)
                .HasColumnName("nome_conta_bancaria")
                .IsRequired()
                .HasMaxLength(100);

            // 5. Tipo da Conta: Converte o Enum TipoContaBancaria para inteiro ao salvar no banco.
            builder.Property(p => p.Tipo)
                .HasColumnName("tipo_conta_bancaria")
                .HasConversion<int>()
                .IsRequired();

            // 6. Precisão Monetária: Define o tipo decimal(18,2) para suportar valores grandes com 2 casas decimais.
            builder.Property(p => p.Saldo)
                .HasColumnName("saldo_conta_bancaria")
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            // 7. Performance: Cria um índice na coluna usuario_id para acelerar as buscas de contas de um usuário específico.
            builder.HasIndex(userId => userId.UsuarioId);

            // 8. Relacionamento: Configura a relação 1:N entre Usuario e ContasBancarias.
            // O DeleteBehavior.Restrict impede que um usuário seja deletado se ele ainda possuir contas vinculadas.
            builder.HasOne(fkUser => fkUser.Usuario)
                .WithMany(u => u.ContasBancarias)
                .HasForeignKey(fkUser => fkUser.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
