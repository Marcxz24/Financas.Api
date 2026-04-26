namespace Financas.Api.Entities.Enums
{
    /// <summary>
    /// Representa o status da fatura no ciclo de cobrança.
    /// </summary>
    public enum FaturaStatus
    {
        /// <summary>
        /// Fatura ainda está recebendo lançamentos (compras).
        /// </summary>
        Aberta = 1,

        /// <summary>
        /// Fatura foi fechada e não aceita mais lançamentos.
        /// </summary>
        Fechada = 2,

        /// <summary>
        /// Fatura foi totalmente paga pelo usuário.
        /// </summary>
        Paga = 3,

        /// <summary>
        /// Fatura passou do vencimento sem pagamento total.
        /// </summary>
        Atrasada = 4
    }
}
