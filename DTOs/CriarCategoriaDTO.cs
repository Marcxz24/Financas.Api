using Financas.Api.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace Financas.Api.DTOs
{
    public class CriarCategoriaDTO
    {
        /// <summary>
        /// Nome da categoria, como "Alimentação", "Transporte", "Salário", etc. 
        /// Validamos o tamanho para evitar truncamento no banco de dados (Limite: 100).
        /// </summary>
        [Required(ErrorMessage = "O nome da categoria é obrigatório")]
        [MaxLength(100, ErrorMessage = "O nome da categoria não pode ultrapassar 100 caracteres")]
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// Identificador visual da categoria (ex: classe de ícone ou caminho de recurso).
        /// Armazena a referência que será utilizada pela camada de apresentação (Frontend) 
        /// para renderizar o elemento gráfico correspondente.
        /// </summary>
        [MaxLength(100, ErrorMessage = "O ícone da categoria não pode ultrapassar 100 caracteres")]
        public string Icone { get; set; } = string.Empty;

        /// <summary>
        /// Identifica se é 'Receita (1)' ou 'Despesa (2)'.
        /// O ASP.NET fará o bind automático da string no JSON para o valor numérico do Enum.
        /// </summary>
        [Required(ErrorMessage = "O tipo de lançamento (Receita/Despesa) é obrigatório")]
        [EnumDataType(typeof(TipoLancamento), ErrorMessage = "Tipo de lançamento inválido")]
        public TipoLancamento Tipo { get; set; }
    }
}
