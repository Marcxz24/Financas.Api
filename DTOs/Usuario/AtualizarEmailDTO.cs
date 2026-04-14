using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;

namespace Financas.Api.DTOs.Usuario
{
    /// <summary>
    /// DTO utilizado para a alteração do endereço de e-mail do usuário.
    /// Implementa uma camada de segurança adicional ao exigir a senha atual para confirmar a operação.
    /// </summary>
    public class AtualizarEmailDTO
    {
        /// <summary>
        /// O novo endereço de e-mail desejado pelo usuário.
        /// Validado para garantir que segue o formato correto de e-mail e os limites do banco de dados.
        /// </summary>
        [Required(ErrorMessage = "O Novo E-mail é obrigatório")]
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        [MaxLength(255, ErrorMessage = "O E-mail não pode ultrapassar 255 caracteres")]
        public string NovoEmail { get; set; } = string.Empty;

        /// <summary>
        /// Senha atual do usuário. 
        /// Funciona como um fator de confirmação para garantir que o dono da conta autorizou a mudança.
        /// </summary>
        [Required(ErrorMessage = "A Senha atual é obrigatória")]
        public string SenhaAtual { get; set; } = string.Empty;
    }
}
