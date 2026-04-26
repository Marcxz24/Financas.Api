using Financas.Api.Entities.Enums;

namespace Financas.Api.Entities
{
    /// <summary>
    /// Representa o ciclo mensal de gastos de um cartão de crédito.
    /// Agrupa os lançamentos realizados entre a data de início e o fechamento.
    /// </summary>
    public class Fatura
    {
        /// <summary>
        /// Identificador único da fatura no banco de dados.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Identificador do cartão de crédito ao qual esta fatura pertence.
        /// </summary>
        public int CartaoCreditoId { get; set; }

        /// <summary>
        /// Propriedade de navegação para acessar os dados do cartão de crédito vinculado.
        /// </summary>
        public virtual CartaoCredito CartaoCredito { get; set; } = null!;

        /// <summary>
        /// Data de início do período de faturamento (Geralmente o dia seguinte ao fechamento anterior).
        /// </summary>
        public DateTime DataInicio { get; set; }

        /// <summary>
        /// Data em que a fatura é "cortada", impedindo novos lançamentos para este ciclo.
        /// </summary>
        public DateTime DataFechamento { get; set; }

        /// <summary>
        /// Data limite para o pagamento da fatura sem incidência de juros ou multa.
        /// </summary>
        public DateTime DataVencimento { get; set; }

        /// <summary>
        /// Soma de todos os lançamentos vinculados a esta fatura.
        /// </summary>
        public decimal ValorTotal { get; set; }

        /// <summary>
        /// Valor efetivamente pago pelo usuário para esta fatura. 
        /// Permite o controle de pagamentos parciais ou totais.
        /// </summary>
        public decimal ValorPago { get; set; } = 0;

        /// <summary>
        /// Define a situação atual da fatura (ex: Aberta, Fechada ou Paga).
        /// Utilizado para controlar o fluxo de vencimento e liberação de limite.
        /// </summary>
        public FaturaStatus Status { get; set; }

        /// <summary>
        /// Coleção de lançamentos (compras/despesas) vinculados a este ciclo de fatura.
        /// Permite o rastreio detalhado de cada item que compõe o ValorTotal.
        /// </summary>
        public virtual ICollection<Lancamento> Lancamentos { get; set; } = new List<Lancamento>();

        /// <summary>
        /// Histórico de pagamentos realizados para esta fatura.
        /// Permite registrar múltiplos pagamentos parciais até a quitação total do ciclo.
        /// </summary>
        public virtual ICollection<PagamentoFatura> Pagamentos { get; set; } = new List<PagamentoFatura>();
    }
}
