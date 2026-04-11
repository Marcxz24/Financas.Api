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

            // Define se o usuário validou o cadastro via link enviado ao e-mail (Padrão: false).
            builder.Property(e => e.EmailConfirmado)
                .HasColumnName("email_confirmado")
                .IsRequired()
                .HasDefaultValue(false);

            // Armazena a chave única temporária enviada por e-mail para validar a identidade.
            builder.Property(t => t.TokenConfirmacao)
                .HasColumnName("token_confirmacao")
                .HasMaxLength(255)
                .IsRequired(false); // Permite null, pois o token só existe durante o processo de confirmação.

            // Registra o limite de tempo para que o token de confirmação seja considerado válido.
            builder.Property(te => te.TokenExpiracao)
                .HasColumnName("token_expiracao")
                .IsRequired(false); // Permite null, pois o token só existe durante o processo de confirmação.  

            // Define o index para o campo Email, garantindo que seja único.
            builder.HasIndex(u => u.Email).IsUnique(); // Garante que o email seja único
        }
    }
}
