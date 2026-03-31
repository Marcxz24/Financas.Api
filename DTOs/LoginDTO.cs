namespace Financas.Api.DTOs
{
    /// <summary>
    /// Objeto de Transferência de Dados (DTO) para a operação de Login.
    /// Utilizado para receber as credenciais do usuário vindas da requisição (JSON).
    /// </summary>
    public class LoginDTO
    {
        /// <summary>
        /// Endereço de e-mail do usuário que está tentando se autenticar.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Senha do usuário enviada em texto simples para validação no servidor.
        /// </summary>
        public string Password { get; set; } = string.Empty;
    }
}
