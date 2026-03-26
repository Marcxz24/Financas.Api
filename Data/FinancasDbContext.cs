using Financas.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Financas.Api.Data
{
    public class FinancasDbContext : DbContext
    {
        public FinancasDbContext(DbContextOptions<FinancasDbContext> options) : base(options)
        { 
            
        }

        public DbSet<Usuario> Usuarios { get; set; }
    }
}
