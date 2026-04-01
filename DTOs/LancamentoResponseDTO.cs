using Financas.Api.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace Financas.Api.DTOs
{
    /// <summary>
    /// DTO (Data Transfer Object) de Saída.
    /// Utilizado para enviar os dados de um lançamento para o cliente (Mobile/Web).
    /// Isola a Entidade de domínio, evitando a exposição de propriedades sensíveis ou recursivas.
    /// </summary>
    public class LancamentoResponseDTO
    {
        /// <summary>
        /// Identificador único no banco de dados. 
        /// Essencial no Response para que o Front-end possa realizar edições ou exclusões.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Vínculo com o proprietário do registro.
        /// Útil para conferência de integridade no lado do cliente.
        /// </summary>
        public int UsuarioId { get; set; }

        /// <summary>
        /// Descrição do lançamento.
        /// </summary>
        public string Descricao { get; set; } = string.Empty;

        /// <summary>
        /// Valor financeiro formatado.
        /// </summary>
        public decimal Valor { get; set; }

        /// <summary>
        /// Data em que o lançamento ocorreu.
        /// </summary>
        public DateTime Data { get; set; }

        /// <summary>
        /// Descrição textual do tipo de lançamento (ex: "Receita" ou "Despesa").
        /// </summary>
        public string Tipo { get; set; } = string.Empty;
    }
}
