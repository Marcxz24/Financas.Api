using System.ComponentModel.DataAnnotations;

namespace Financas.Api.DTOs.Usuario
{
    public class EsqueciSenhaDTO
    {
        // Identificador único do usuário para o processo de recuperação.
        [Required(ErrorMessage = "O E-mail é obrigatório")]
        // Validação automática de formato (ex: deve conter '@' e um domínio válido).
        [EmailAddress(ErrorMessage = "O E-mail deve ser válido")]
        public string Email { get; set; } = string.Empty;
    }
}
