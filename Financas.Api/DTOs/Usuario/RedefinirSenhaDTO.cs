using System.ComponentModel.DataAnnotations;

namespace Financas.Api.DTOs.Usuario
{
    /// <summary>
    /// DTO utilizado para capturar as informações necessárias para a definição de uma nova senha.
    /// Contém regras de validação para garantir a integridade dos dados antes de chegarem ao serviço.
    /// </summary>
    public class RedefinirSenhaDTO
    {
        /// <summary>
        /// Token de segurança enviado por e-mail para validar o pedido de redefinição.
        /// </summary>
        [Required(ErrorMessage = "O Token é obrigatório.")]
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// A nova senha escolhida pelo usuário. 
        /// Impõe um limite mínimo de 6 caracteres para garantir um nível básico de segurança.
        /// </summary>
        [Required(ErrorMessage = "A Nova senha é obrigatória.")]
        [MinLength(6, ErrorMessage = "A nova senha deve conter no mínimo 6 caracteres")]
        public string NewPassword { get; set; } = string.Empty;

        /// <summary>
        /// Campo de verificação para evitar erros de digitação. 
        /// O atributo 'Compare' garante que este valor seja idêntico ao campo 'NewPassword'.
        /// </summary>
        [Required(ErrorMessage = "A Confirmação da senha é obrigatória.")]
        [Compare("NewPassword", ErrorMessage = "A nova senha e a confirmação devem ser iguais")]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}