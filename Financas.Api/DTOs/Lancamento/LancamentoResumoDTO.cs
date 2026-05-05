namespace Financas.Api.DTOs.Lancamento
{
    /// <summary>
    /// DTO simplificado para exibição em listas e históricos.
    /// Utilizado para reduzir o volume de dados trafegados entre API e React.
    /// </summary>
    public class LancamentoResumoDTO
    {
        // Nome ou título do lançamento (ex: "Pagamento Aluguel")
        public string Descricao { get; set; } = string.Empty;

        // Valor monetário do registro
        public decimal Valor { get; set; }

        // Data e hora em que o lançamento ocorreu no banco de dados
        public DateTime DataLancamento { get; set; }

        // Representação em texto do Enum (Receita ou Despesa) para o Front-end
        public string Tipo { get; set; } = string.Empty;
    }
}