using Financas.Api.Entities.Enums;

namespace Financas.Api.Entities
{
    /// <summary>
    /// Classe que representa a entidade de Lançamento Financeiro.
    /// Utilizada para registrar tanto Despesas quanto Receitas no sistema.
    /// </summary>
    public class Lancamento
    {
        /// <summary>
        /// Identificador único do lançamento (Chave Primária).
        /// Geralmente configurado como Auto-Incremento no banco de dados.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Descrição ou título da movimentação (ex: "Aluguel", "Salário", "Mercado").
        /// </summary>
        public string Descricao { get; set; } = string.Empty;

        /// <summary>
        /// Valor monetário da transação. 
        /// O tipo 'decimal' é o mais indicado para evitar erros de precisão em cálculos financeiros.
        /// </summary>
        public decimal Valor { get; set; }

        /// <summary>
        /// Data e hora em que a transação ocorreu ou foi registrada.
        /// </summary>
        public DateTime Data { get; set; }

        /// <summary>
        /// Define a categoria do lançamento. 
        /// Comumente utilizado para diferenciar "Receita" de "Despesa".
        /// </summary>
        public TipoLancamento Tipo { get; set; }

        /// <summary>
        /// Chave Estrangeira (FK) que vincula este lançamento a um usuário específico.
        /// </summary>
        public int UsuarioId { get; set; }

        /// <summary>
        /// Chave estrangeira (FK) que armazena o identificador da categoria vinculada.
        /// O uso do tipo 'int?' (nullable) permite que um lançamento exista temporariamente 
        /// sem uma categoria definida (ex: lançamentos pendentes de classificação).
        /// </summary>
        public int? CategoriaId { get; set; }

        /// <summary>
        /// Propriedade de navegação que permite acessar os dados completos da categoria associada.
        /// É marcada como anulável para evitar avisos de compilador (null safety) e indicar 
        /// que o carregamento depende de um JOIN explícito (.Include) na consulta.
        /// </summary>
        public Categoria? Categoria { get; set; }

        /// <summary>
        /// Propriedade de Navegação. 
        /// Permite que o Entity Framework carregue os dados do objeto Usuario associado.
        /// </summary>
        public Usuario Usuario { get; set; } = null!;
    }
}
