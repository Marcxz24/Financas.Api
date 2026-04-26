namespace Financas.Api.Entities
{
    public class PagamentoFatura
    {
        public int Id { get; set; }

        public int FaturaId { get; set; }
        public virtual Fatura Fatura { get; set; } = null!;

        public decimal ValorPago { get; set; }

        public DateTime DataPagamento { get; set; }

        public int? ContaBancariaId { get; set; }
        public virtual ContaBancaria? ContaBancaria { get; set; }

        public string? Observacao { get; set; }
    }
}
