using Financas.Api.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace Financas.Api.DTOs.Lancamento
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

        /// <summary>
        /// Identificador da categoria associada ao lançamento. Nulo se não categorizado.
        /// </summary>
        public int? CategoriaId { get; set; }

        /// <summary>
        /// Nome da categoria associada. Nulo se o lançamento não tiver categoria.
        /// </summary>
        public string? CategoriaNome { get; set; }

        /// <summary>
        /// Identificador da conta bancária de origem ou destino do lançamento.
        /// Pode ser nulo caso o lançamento não esteja vinculado a uma conta específica.
        /// </summary>
        public int? ContaBancariaId { get; set; }

        /// <summary>
        /// Nome descritivo da conta bancária (ex: "Carteira" ou "Banco Inter").
        /// Utilizado para exibição direta no front-end sem necessidade de consultas extras.
        /// </summary>
        public string? ContaBancariaNome { get; set; }
    }
}
