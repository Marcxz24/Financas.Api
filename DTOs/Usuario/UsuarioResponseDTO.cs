namespace Financas.Api.DTOs.Usuario
{
    public class UsuarioResponseDTO
    {
        // Propriedades para retornar informações do usuário
        public int Id { get; set; }

        // O nome de usuário é útil para exibir informações do usuário, mas não deve ser usado para autenticação
        public string Username { get; set; } = string.Empty;

        // O email é útil para exibir informações do usuário, mas não deve ser usado para autenticação
        public string Email { get; set; } = string.Empty;
    }
}
