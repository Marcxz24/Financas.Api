namespace Financas.Api.Entities.Enums
{
    /// <summary>
    /// Define os tipos fixos de movimentação financeira permitidos no sistema.
    /// O uso de Enum garante que apenas valores válidos (Receita ou Despesa) sejam utilizados.
    /// </summary>
    public enum TipoLancamento
    {
        /// <summary>
        /// Representa a entrada de dinheiro (ex: Salário, Rendimentos).
        /// Atribuído ao valor 1 para facilitar o armazenamento numérico no banco.
        /// </summary>
        Receita = 1,

        /// <summary>
        /// Representa a saída de dinheiro (ex: Contas, Compras, Lazer).
        /// Atribuído ao valor 2.
        /// </summary>
        Despesa = 2
    }
}