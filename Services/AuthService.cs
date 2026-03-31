using Financas.Api.Data;
using Financas.Api.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Financas.Api.Services
{
    public class AuthService
    {
        // O serviço de autenticação é responsável por lidar com a lógica de negócios relacionada à autenticação dos usuários, como login e geração de tokens JWT.
        private readonly FinancasDbContext _FinancasDbContext;

        // O construtor do serviço recebe o contexto do banco de dados e a configuração da aplicação via injeção de dependência, permitindo que o serviço interaja com a base de dados para realizar operações relacionadas à autenticação e acesse as configurações necessárias para gerar tokens JWT.
        private readonly IConfiguration _configuration;

        // O construtor do serviço recebe o contexto do banco de dados e a configuração da aplicação via injeção de dependência, permitindo que o serviço interaja com a base de dados para realizar operações relacionadas à autenticação e acesse as configurações necessárias para gerar tokens JWT.
        public AuthService(FinancasDbContext dbContext, IConfiguration configuration)
        {
            _FinancasDbContext = dbContext;
            _configuration = configuration;
        }

        // O método Login é responsável por autenticar um usuário com base nas credenciais fornecidas (email e senha) e, se a autenticação for bem-sucedida, gerar e retornar um token JWT que pode ser usado para acessar recursos protegidos na aplicação.
        public async Task<string> Login(LoginDTO dto)
        {

            var usuario = await _FinancasDbContext.Usuarios
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (usuario == null)
                throw new Exception("Usuário não encontrado");

            var senhaValida = BCrypt.Net.BCrypt.Verify(dto.Password, usuario.Password);

            if (!senhaValida)
                throw new Exception("Senha inválida");

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Name, usuario.Username)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
