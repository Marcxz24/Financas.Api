using System.ComponentModel.DataAnnotations;

namespace Financas.Api.DTOs
{
    public class RedefinirSenhaDTO
    {
        [Required(ErrorMessage = "O Token é obrigatório.")]
        public string Token { get; set; } = string.Empty;

        [Required(ErrorMessage = "A Nova senha é obrigatória.")]
        [MinLength(6, ErrorMessage = "A nova senha deve conter no mínimo 6 caracteres")]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "A Confirmação da senha é obrigatória.")]
        [Compare("NewPassword", ErrorMessage = "A nova senha e a confirmação devem ser iguais")]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}
