using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Financas.Api.Entities
{
    /// <summary>
    /// Representa a entidade de Usuário no sistema.
    /// Armazena as informações de autenticação e o vínculo com os lançamentos financeiros.
    /// </summary>
    public class Usuario
    {
        /// <summary>
        /// Identificador único do usuário (Chave Primária).
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nome de exibição ou login do usuário. 
        /// Inicializado como string vazia para evitar alertas de valores nulos.
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Endereço de e-mail único utilizado para acesso e comunicações.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Senha do usuário (idealmente armazenada como Hash no banco de dados).
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Data e hora em que o usuário foi criado no sistema.
        /// </summary>
        public DateTime DataCadastro { get; set; }

        /// <summary>
        /// Propriedade de Navegação: representa a coleção de lançamentos vinculados a este usuário.
        /// Define o relacionamento 1:N (Um usuário para muitos lançamentos).
        /// </summary>
        public ICollection<Lancamento> Lancamentos { get; set; } = new List<Lancamento>();

        public ICollection<Categoria> Categorias { get; set; } = new List<Categoria>();
    }
}