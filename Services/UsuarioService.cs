using Financas.Api.Data;
using Financas.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Financas.Api.Services
{
    public class UsuarioService
    {
        private readonly FinancasDbContext _FinancasDbContext;

        public UsuarioService(FinancasDbContext dbContext) 
        {
            _FinancasDbContext = dbContext;
        }

        public async Task<Usuario> CriarUsuario(Usuario usuario)
        {
            usuario.DataCadastro = DateTime.Now;
            _FinancasDbContext.Usuarios.Add(usuario);
            await _FinancasDbContext.SaveChangesAsync();

            return usuario;
        }

        public async Task<List<Usuario>> GetUsuario()
        {
            return await _FinancasDbContext.Usuarios.ToListAsync();
        }
    }
}
