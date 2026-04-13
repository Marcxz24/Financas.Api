using Financas.Api.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace Financas.Api.DTOs.ContaBancaria
{
    /// <summary>
    /// DTO para transferência de dados durante a criação de uma nova conta bancária.
    /// Centraliza as regras de validação de entrada para garantir a integridade dos dados financeiros.
    /// </summary>
    public class CriarContaBancariaDTO
    {
        /// <summary>
        /// Nome identificador da conta (ex: "Nubank Principal" ou "Carteira").
        /// Limitado a 100 caracteres para manter a consistência com o mapeamento do banco de dados.
        /// </summary>
        [Required(ErrorMessage = "O Nome da Conta é Obrigatório.")]
        [MaxLength(100, ErrorMessage = "O nome da Conta não pode ultrapassar 100 Caracteres.")]
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// Classificação da conta baseada no enum TipoContaBancaria.
        /// O atributo EnumDataType garante que o valor recebido corresponda a uma das opções válidas do enum.
        /// </summary>
        [Required(ErrorMessage = "O Tipo da Conta é Obrigatório.")]
        [EnumDataType(typeof(TipoContaBancaria), ErrorMessage = "Tipo da conta inválido.")]
        public TipoContaBancaria Tipo { get; set; }

        /// <summary>
        /// Saldo inicial da conta. 
        /// A validação Range impede que a conta seja criada com valores negativos, forçando um estado inicial estável.
        /// </summary>
        [Range(0, double.MaxValue, ErrorMessage = "O Saldo deve ser um valor positivo.")]
        public decimal Saldo { get; set; }
    }
}
