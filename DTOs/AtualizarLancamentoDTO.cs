using Financas.Api.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace Financas.Api.DTOs
{
    /// <summary>
    /// DTO para atualização de lançamentos existentes.
    /// O uso de propriedades anuláveis (?) permite que o cliente envie apenas 
    /// os campos que deseja alterar, mantendo os outros intactos (Patch Pattern).
    /// </summary>
    public class AtualizarLancamentoDTO
    {
        /// <summary>
        /// Nova descrição para o lançamento. 
        /// Sincronizado com o limite de 255 caracteres da configuração do banco.
        /// </summary>
        [MaxLength(255, ErrorMessage = "A descrição não pode ultrapassar 255 caracteres")]
        public string? Descricao { get; set; }

        /// <summary>
        /// Novo valor monetário. 
        /// O Range é aplicado apenas se o valor for enviado.
        /// </summary>
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser um número positivo maior que zero")]
        public decimal? Valor { get; set; }

        /// <summary>
        /// Nova data de competência do lançamento.
        /// </summary>
        public DateTime? Data { get; set; }

        /// <summary>
        /// Novo tipo (Receita ou Despesa).
        /// </summary>
        public TipoLancamento? Tipo { get; set; }
    }
}
