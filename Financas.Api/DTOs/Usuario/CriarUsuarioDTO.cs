using System.ComponentModel.DataAnnotations;

namespace Financas.Api.DTOs.Usuario
{
    public class CriarUsuarioDTO
    {
        // Propriedades para criar um novo usuário
        [Required(ErrorMessage = "Username é obrigatório")]
        public string Username { get; set; } = string.Empty;

        // O email é necessário para o processo de autenticação e recuperação de senha
        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "E-mail inválido")]
        public string Email { get; set; } = string.Empty;

        // A senha é necessária para o processo de autenticação
        [Required(ErrorMessage = "Password é obrigatório")]
        [MinLength(6, ErrorMessage = "A senha deve conter pelo menos 6 caracteres")]
        public string Password { get; set; } = string.Empty;
    }
}
