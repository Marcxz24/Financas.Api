using System.ComponentModel.DataAnnotations;

namespace Financas.Api.DTOs
{
    public class AtualizarSenhaDTO
    {
        // Campo para validar a identidade do usuário através da senha atual.
        [Required(ErrorMessage = "A senha Atual é obrigatória")]
        public string CurrentPassword { get; set; } = string.Empty;

        // Define a nova senha com regra de complexidade mínima (6 caracteres).
        [Required(ErrorMessage = "A nova senha é obrigatória")]
        [MinLength(6, ErrorMessage = "A nova senha deve conter no mínimo 6 caracteres")]
        public string NewPassword { get; set; } = string.Empty;

        // Garante que o usuário não cometeu erros de digitação ao definir a nova senha.
        [Required(ErrorMessage = "A confirmação da nova senha é obrigatória")]
        [Compare("NewPassword", ErrorMessage = "A nova senha e a confirmação devem ser iguais")]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}