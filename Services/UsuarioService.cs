using Financas.Api.Data;
using Financas.Api.DTOs;
using Financas.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Financas.Api.Services
{
    public class UsuarioService
    {
        // O serviço de usuário é responsável por lidar com a lógica de negócios relacionada aos usuários, como criação e recuperação de usuários.
        private readonly FinancasDbContext _financasDbContext;

        // O construtor do serviço recebe o contexto do banco de dados via injeção de dependência, permitindo que o serviço interaja com a base de dados para realizar operações relacionadas aos usuários.
        public UsuarioService(FinancasDbContext dbContext) 
        {
            _financasDbContext = dbContext;
        }

        // O método CriarUsuario é responsável por criar um novo usuário. Ele verifica se o e-mail fornecido já está em uso, e se não estiver, cria um novo registro de usuário no banco de dados. O método retorna um DTO de resposta contendo as informações do usuário criado.
        public async Task<UsuarioResponseDTO> CriarUsuario(CriarUsuarioDTO dto)
        {
            // Antes de criar um novo usuário, o método verifica se o e-mail fornecido já existe no banco de dados. Se o e-mail já estiver em uso, uma exceção é lançada para informar que o e-mail não pode ser utilizado.
            var emailExistente = await _financasDbContext.Usuarios
                .AnyAsync(u => u.Email == dto.Email);

            // Se o e-mail já estiver em uso, uma exceção é lançada para informar que o e-mail não pode ser utilizado.
            if (emailExistente)
            {
                throw new InvalidOperationException("Este E-mail já está em uso.");
            }

            // Para garantir a segurança dos dados, a senha fornecida pelo usuário é hashada usando a biblioteca BCrypt antes de ser armazenada no banco de dados. Isso ajuda a proteger as senhas dos usuários em caso de acesso não autorizado ao banco de dados.
            var senhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            // Se o e-mail não estiver em uso, um novo objeto Usuario é criado com as informações fornecidas no DTO. A senha é armazenada diretamente, mas em um cenário real, ela deve ser hashada para garantir a segurança dos dados.
            var usuario = new Usuario
            {
                Username = dto.Username,
                Email = dto.Email,
                Password = senhaHash, // A senha é armazenada de forma segura utilizando hash com BCrypt
                DataCadastro = DateTime.UtcNow
            };

            // O novo usuário é adicionado ao contexto do banco de dados e as alterações são salvas. Após a criação do usuário, um DTO de resposta é retornado contendo as informações do usuário criado, como o ID, username e e-mail.
            _financasDbContext.Usuarios.Add(usuario);
            await _financasDbContext.SaveChangesAsync();

            // Após a criação do usuário, um DTO de resposta é retornado contendo as informações do usuário criado, como o ID, username e e-mail.
            return new UsuarioResponseDTO
            { 
                Id = usuario.Id,
                Username = usuario.Username,
                Email = usuario.Email
            };
        }

        // O método GetUsuario é responsável por recuperar a lista de usuários do banco de dados. Ele consulta a tabela de usuários, converte os registros em uma lista de DTOs de resposta e retorna essa lista para o cliente.
        public async Task<List<UsuarioResponseDTO>> GetUsuario()
        {
            var usuarios = await _financasDbContext.Usuarios.ToListAsync();

            return usuarios.Select(u => new UsuarioResponseDTO
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email
            }).ToList();
        }
    }
}
