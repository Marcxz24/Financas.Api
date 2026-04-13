namespace Financas.Api.DTOs.ContaBancaria
{
    /// <summary>
    /// DTO de resposta utilizado para retornar os dados de uma conta bancária.
    /// Transforma a entidade interna em um formato otimizado para exibição no cliente (Front-end/Mobile).
    /// </summary>
    public class ContaBancariaResponseDTO
    {
        /// <summary>
        /// Identificador único da conta.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Identificador do usuário proprietário, útil para filtros no lado do cliente.
        /// </summary>
        public int UsuarioId { get; set; }

        /// <summary>
        /// Nome descritivo da conta (ex: "Banco do Brasil").
        /// </summary>
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// Saldo atualizado da conta.
        /// </summary>
        public decimal Saldo { get; set; }

        /// <summary>
        /// Representação textual do tipo de conta (ex: "Corrente", "Poupança").
        /// Diferente da entidade, aqui o tipo é retornado como string para facilitar a exibição direta.
        /// </summary>
        public string Tipo { get; set; } = string.Empty;
    }
}
