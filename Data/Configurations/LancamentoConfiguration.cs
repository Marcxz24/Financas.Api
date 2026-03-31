using Financas.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Financas.Api.Data.Configurations
{
    /// <summary>
    /// Configuração da Fluent API para a entidade Lancamento.
    /// Define como as propriedades da classe se transformam em colunas no banco de dados.
    /// </summary>
    public class LancamentoConfiguration : IEntityTypeConfiguration<Lancamento>
    {
        public void Configure(EntityTypeBuilder<Lancamento> builder)
        {
            // Define o nome físico da tabela no MySQL como "lancamentos"
            builder.ToTable("lancamentos");

            // Configura o campo 'Id' como a Chave Primária (PK) da tabela
            builder.HasKey(key => key.Id);

            // Configuração da Descrição: nome da coluna, obrigatoriedade e limite de caracteres
            builder.Property(desc => desc.Descricao)
                .HasColumnName("descricao")
                .IsRequired()
                .HasMaxLength(255);

            // Configuração do Valor: Define precisão decimal (10 dígitos no total, 2 decimais)
            // Essencial para evitar arredondamentos incorretos em finanças.
            builder.Property(value => value.Valor)
                .HasColumnName("valor")
                .IsRequired()
                .HasColumnType("decimal(10,2)");

            // Mapeia a data do lançamento para a coluna 'data_lancamento'
            builder.Property(date => date.Data)
                .HasColumnName("data_lancamento")
                .IsRequired();

            // Configura o tipo (ex: "Receita" ou "Despesa") com limite de 25 caracteres
            builder.Property(type => type.Tipo)
                .HasColumnName("tipo")
                .HasConversion<int>()
                .IsRequired();

            // Define o nome da coluna que armazena o ID do proprietário do registro
            builder.Property(fkUser => fkUser.UsuarioId)
                .HasColumnName("usuario_id")
                .IsRequired();

            // Cria um índice para melhorar performance nas consultas por usuário
            builder.HasIndex(l => l.UsuarioId);

            // Configuração do Relacionamento (FK):
            // Um Lançamento possui um Usuário. Um Usuário pode ter muitos Lançamentos.
            // OnDelete(DeleteBehavior.Restrict) significa que se o usuário possuir lançamentos, 
            // o sistema IRÁ IMPEDIR a exclusão desse usuário até que seus lançamentos sejam apagados manualmente.
            // Isso garante a integridade dos dados, evitando que existam lançamentos sem dono.
            builder.HasOne(l => l.Usuario)
                .WithMany(u => u.Lancamentos)
                .HasForeignKey(l => l.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
