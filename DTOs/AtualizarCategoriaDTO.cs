using Financas.Api.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace Financas.Api.DTOs
{
    public class AtualizarCategoriaDTO
    {
        /// <summary>
        /// Representa o novo rótulo identificador da categoria para atualização.
        /// Definido como anulável (nullable) para suportar atualizações parciais onde o campo pode ser omitido.
        /// </summary>
        [MaxLength(100, ErrorMessage = "O nome da categoria não pode ultrapassar 100 caracteres")]
        public string? Nome { get; set; }

        /// <summary>
        /// Representa a nova referência visual da categoria para atualização.
        /// Armazena o metadado (string) que vincula a categoria a um elemento gráfico no frontend.
        /// </summary>
        [MaxLength(100, ErrorMessage = "O icone da categoria não pode ultrapassar 100 caracteres")]
        public string? Icone { get; set; }

        /// <summary>
        /// Novo tipo (Receita ou Despesa).
        /// </summary>
        public TipoLancamento? Tipo { get; set; }
    }
}
