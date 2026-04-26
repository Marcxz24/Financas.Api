namespace Financas.Api.Entities.Enums
{
    /// <summary>
    /// Representa o status atual do cartão de crédito.
    /// Define se o cartão pode ou não ser utilizado para novos lançamentos.
    /// </summary>
    public enum StatusCartaoCredito
    {
        /// <summary>
        /// Cartão ativo e disponível para uso em compras e geração de faturas.
        /// </summary>
        Ativo = 1,

        /// <summary>
        /// Cartão temporariamente bloqueado, impedindo novas transações.
        /// Pode ser desbloqueado posteriormente.
        /// </summary>
        Bloqueado = 2,

        /// <summary>
        /// Cartão encerrado de forma definitiva.
        /// Não pode mais ser utilizado.
        /// </summary>
        Cancelado = 3
    }
}
