using Financas.Api.Entities.Enums;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;

namespace Financas.Api.DTOs.ContaBancaria
{
    /// <summary>
    /// DTO utilizado para a atualização dos dados de uma conta bancária existente.
    /// As propriedades são opcionais (anuláveis) para permitir atualizações parciais.
    /// </summary>
    public class AtualizarContaBancariaDTO
    {
        /// <summary>
        /// Novo nome para a conta bancária.
        /// Se enviado, deve respeitar o limite de 100 caracteres definido no mapeamento.
        /// </summary>
        [MaxLength(100, ErrorMessage = "O Nome da Conta Bancaria não pode ultrapassar 100 caracteres.")]
        public string? Nome { get; set; }

        /// <summary>
        /// Nova categoria da conta (ex: Corrente, Poupança).
        /// </summary>
        public TipoContaBancaria? Tipo { get; set; }

        /// <summary>
        /// Novo valor de saldo. Geralmente utilizado para ajustes manuais ou correções.
        /// </summary>
        public decimal? Saldo { get; set; }
    }
}
