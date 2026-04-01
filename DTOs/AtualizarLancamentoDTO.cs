using Financas.Api.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace Financas.Api.DTOs
{
    public class AtualizarLancamentoDTO
    {
        [MaxLength(255, ErrorMessage = "A descrição não pode ultrapassar 255 caracteres")]
        public string? Descricao { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser um número positivo maior que zero")]
        public decimal? Valor { get; set; }

        public DateTime? Data { get; set; }

        public TipoLancamento? Tipo { get; set; }
    }
}
