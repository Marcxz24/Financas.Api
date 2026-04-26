namespace Financas.Api.DTOs.Fatura
{
    public class PagamentoResponseDTO
    {
        /// <summary>
        /// Identificador único do pagamento
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Identificador da fatura relacionada
        /// </summary>
        public int FaturaId { get; set; }

        /// <summary>
        /// Valor efetivamente pago
        /// </summary>
        public decimal ValorPago { get; set; }

        /// <summary>
        /// Data em que o pagamento foi realizado
        /// </summary>
        public DateTime DataPagamento { get; set; }

        /// <summary>
        /// Conta bancária utilizada no pagamento (opcional)
        /// </summary>
        public int? ContaBancariaId { get; set; }

        /// <summary>
        /// Observação livre sobre o pagamento
        /// </summary>
        public string? Observacao { get; set; }
    }
}
