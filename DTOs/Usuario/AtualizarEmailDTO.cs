using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;

namespace Financas.Api.DTOs.Usuario
{
    public class AtualizarEmailDTO
    {
        [Required(ErrorMessage = "O Novo E-mail é obrigatório")]
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        [MaxLength(255, ErrorMessage = "O E-mail não pode ultrapassar 255 caracteres")]
        public string NovoEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "A Senha atual é obrigatória")]
        public string SenhaAtual { get; set; } = string.Empty;
    }
}
