using System.ComponentModel.DataAnnotations;

namespace Financas.Api.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public DateTime DataCadastro { get; set; }
    }
}
