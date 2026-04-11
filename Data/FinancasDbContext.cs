using Financas.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Financas.Api.Data
{
    /// <summary>
    /// DbContext é a classe principal que gerencia a conexão com o banco de dados e as operações de leitura/gravação.
    /// </summary>

    // O FinancasDbContext herda de DbContext, que é a classe base do Entity Framework Core para trabalhar com bancos de dados.
    public class FinancasDbContext : DbContext
    {
        // O construtor recebe as opções de configuração do DbContext, como a string de conexão, e as passa para a classe base.
        public FinancasDbContext(DbContextOptions<FinancasDbContext> options) : base(options) { }
        
        // A propriedade DbSet<Usuario> representa a coleção de entidades do tipo Usuario no banco de dados. 
        // O Entity Framework Core usará essa propriedade para mapear a tabela correspondente no banco de dados,
        // e realizar operações CRUD (Create, Read, Update, Delete) sobre os registros de usuários.
        public DbSet<Usuario> Usuarios { get; set; }

        // Define uma propriedade que representa a tabela "Lancamentos" no banco de dados.
        // O EF Core usa o tipo 'DbSet<T>' para mapear a classe 'Lancamento' para uma tabela real.
        public DbSet<Lancamento> Lancamentos { get; set; }

        // Representa a coleção de entidades 'Categoria' no nível de memória.
        // O EF Core utiliza este DbSet para traduzir operações de código em comandos DML 
        // (Data Manipulation Language) específicos para a tabela física 'categorias'.
        public DbSet<Categoria> Categorias { get; set; }

        // O método usado para configurar o modelo do banco de dados (mapeamento das entidades)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Aplica automaticamente todas as classes de configuração (Fluent API)
            // Ele procura no assembly (projeto) por classes que implementam IEntityTypeConfiguration.
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(FinancasDbContext).Assembly);

            // Chama a implementação base do DbContext.
            base.OnModelCreating(modelBuilder);
        }
    }
}
