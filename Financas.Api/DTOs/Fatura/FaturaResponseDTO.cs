using Financas.Api.Entities.Enums;

namespace Financas.Api.DTOs.Fatura
{
    public class FaturaResponseDTO
    {
        public int Id { get; set; }

        public int CartaoCreditoId { get; set; }

        public DateTime DataInicio { get; set; }

        public DateTime DataFechamento { get; set; }

        public DateTime DataVencimento { get; set; }

        public decimal ValorTotal { get; set; }

        public decimal ValorPago { get; set; }

        public string Status { get; set; } = string.Empty;
    }
}
