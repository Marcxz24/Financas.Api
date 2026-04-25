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

            // Define o nome da coluna que armazena o ID da categoria associada ao lançamento
            builder.Property(fkCategory => fkCategory.CategoriaId)
                .HasColumnName("categoria_id")
                .IsRequired(false);

            // Mapeia a FK da conta bancária. O IsRequired(false) permite que um lançamento 
            // exista temporariamente sem estar vinculado a uma conta (ex: lançamento pendente).
            builder.Property(l => l.ContaBancariaId)
                .HasColumnName("conta_bancaria_id")
                .IsRequired(false);

            // Mapeia a FK do cartão de crédito. O IsRequired(false) permite que um lançamento 
            // exista temporariamente sem estar vinculado a um cartão de crédito (ex: lançamento pendente).
            builder.Property(l => l.CartaoCreditoId)
                .HasColumnName("cartao_credito_id")
                .IsRequired(false);

            // Mapeia a FK da fatura. O IsRequired(false) permite que um lançamento 
            // exista temporariamente sem estar vinculado a uma fatura (ex: lançamento pendente).
            builder.Property(l => l.FaturaId)
                .HasColumnName("fatura_id")
                .IsRequired(false);

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

            // Configuração do Relacionamento (1:N):
            // HasOne: Define que cada Lançamento está associado a exatamente UMA Categoria.
            // WithMany: Define que uma Categoria pode estar vinculada a MUITOS Lançamentos.
            // HasForeignKey: Especifica explicitamente que 'CategoriaId' é a propriedade que faz o vínculo (FK).
            // OnDelete(DeleteBehavior.Restrict): Aplica uma regra de integridade referencial. 
            // O banco de dados impedirá a exclusão de uma categoria se houver qualquer lançamento vinculado a ela,
            // evitando que registros financeiros fiquem sem uma classificação (dados órfãos).
            builder.HasOne(l => l.Categoria)
                .WithMany(c => c.Lancamentos)
                .HasForeignKey(l => l.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuração do Relacionamento com Conta Bancária:
            // Vincula o lançamento à conta de onde o dinheiro saiu ou para onde entrou.
            // O uso do Restrict garante que você não delete uma conta bancária que ainda possua 
            // histórico de transações, preservando a rastreabilidade financeira.
            builder.HasOne(l => l.ContaBancaria)
                .WithMany(c => c.Lancamentos)
                .HasForeignKey(l => l.ContaBancariaId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuração do Relacionamento com Cartão de Crédito:
            // Vincula o lançamento ao cartão de crédito utilizado.
            // O uso do Restrict garante que você não delete um cartão de crédito que ainda possua 
            // histórico de transações, preservando a rastreabilidade financeira.
            builder.HasOne(l => l.CartaoCredito)
                .WithMany(c => c.Lancamentos)
                .HasForeignKey(l => l.CartaoCreditoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuração do Relacionamento com Fatura:
            // Vincula o lançamento à fatura utilizada.
            // O uso do Restrict garante que você não delete uma fatura que ainda possua 
            // histórico de transações, preservando a rastreabilidade financeira.
            builder.HasOne(l => l.Fatura)
                .WithMany(f => f.Lancamentos)
                .HasForeignKey(l => l.FaturaId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}