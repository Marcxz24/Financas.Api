using Financas.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Financas.Api.Data.Configurations
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        // Configurações para a entidade Usuario
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            // Define o nome da tabela
            builder.ToTable("usuarios");

            // Configura as propriedades
            // Configura a chave primária
            builder.HasKey(key => key.Id);

            // Username - Obrigatório, com tamanho máximo de 100 caracteres
            builder.Property(username => username.Username)
                .HasColumnName("username")
                .IsRequired()
                .HasMaxLength(100);

            // Email - Obrigatório, com tamanho máximo de 255 caracteres
            builder.Property(email => email.Email)
                .HasColumnName("email")
                .IsRequired()
                .HasMaxLength(255);

            // Password - Obrigatório
            builder.Property(password => password.Password)
                .HasColumnName("password")
                .IsRequired();

            // DataCadastro - Obrigatório
            builder.Property(dataCadastro => dataCadastro.DataCadastro)
                .HasColumnName("data_cadastro")
                .IsRequired();

            // Define o index para o campo Email, garantindo que seja único.
            builder.HasIndex(u => u.Email).IsUnique(); // Garante que o email seja único
        }
    }
}
