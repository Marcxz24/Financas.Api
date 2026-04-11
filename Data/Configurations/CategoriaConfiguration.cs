using Financas.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Financas.Api.Data.Configurations
{
    public class CategoriaConfiguration : IEntityTypeConfiguration<Categoria>
    {
        public void Configure(EntityTypeBuilder<Categoria> builder)
        {
            // Define o nome físico da tabela no banco de dados. 
            // Por padrão o EF usaria o nome da propriedade no DbContext (categorias).
            builder.ToTable("categorias");

            // Define a Chave Primária (PK) da tabela no campo 'Id'.
            builder.HasKey(key => key.Id);

            // Mapeia a propriedade UsuarioId para a coluna 'usuario_id'.
            // IsRequired() adiciona a restrição NOT NULL no banco de dados.
            builder.Property(fkUser => fkUser.UsuarioId)
                    .HasColumnName("usuario_id")
                    .IsRequired();

            // Configura a coluna 'nome': define como NOT NULL e limita o tamanho 
            // para VARCHAR(100), otimizando o armazenamento no banco.
            builder.Property(name => name.Nome)
                .HasColumnName("nome")
                .IsRequired()
                .HasMaxLength(100);

            // Configura a coluna 'icone': define como VARCHAR(100). 
            // Como IsRequired() não foi chamado, ela permite valores NULL.
            builder.Property(ico => ico.Icone)
                .HasColumnName("icone")
                .HasMaxLength(100)
                .IsRequired(false);

            // Define a conversão do Enum 'Tipo' para 'int' no banco de dados.
            // Isso garante que os valores do Enum sejam salvos como números inteiros.
            builder.Property(type => type.Tipo)
               .HasColumnName("tipo")
               .HasConversion<int>()
               .IsRequired();

            // Mapeia a propriedade DataCadastro para a coluna 'data_cadastro' com restrição NOT NULL.
            builder.Property(date => date.DataCadastro)
                .HasColumnName("data_cadastro")
                .IsRequired();

            // Cria um índice no banco de dados para a coluna 'usuario_id'.
            // Essencial para performance em consultas que filtram categorias por usuário.
            builder.HasIndex(userId => userId.UsuarioId);

            // Configura o Relacionamento (1:N):
            // Uma Categoria possui UM Usuario (HasOne).
            // Um Usuario possui MUITAS Categorias (WithMany).
            // Define explicitamente a FK (UsuarioId).
            // OnDelete(DeleteBehavior.Restrict) impede a exclusão de um usuário que possua categorias vinculadas.
            builder.HasOne(fkUser => fkUser.Usuario)
                .WithMany(user => user.Categorias)
                .HasForeignKey(fk => fk.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
