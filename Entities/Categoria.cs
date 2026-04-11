using Financas.Api.Entities.Enums;

namespace Financas.Api.Entities
{
    public class Categoria
    {
        // Identificador primário (Primary Key) da entidade. 
        // No banco de dados, será mapeado como uma coluna INT com AUTO_INCREMENT.
        public int Id { get; set; }

        // Chave Estrangeira (Foreign Key). Define o particionamento lógico dos dados 
        // por proprietário, garantindo o isolamento (Multi-tenancy) no nível de banco.
        public int UsuarioId { get; set; }

        // Propriedade de Navegação (Navigation Property). 
        // Utilizada pelo Entity Framework para realizar Joins (Eager ou Lazy Loading) 
        // entre a tabela Categorias e a tabela Usuarios.
        public Usuario Usuario { get; set; } = null!;

        // Atributo de persistência de dados. No MySQL, será uma coluna VARCHAR.
        // Armazena o rótulo identificador da categoria para o usuário final.
        public string Nome { get; set; } = string.Empty;

        // Metadado de interface (UI). Armazena o identificador de string (ex: nome de classe CSS 
        // ou identificador SVG) para renderização dinâmica no lado do cliente (Frontend).
        public string Icone { get; set; } = string.Empty;

        // Atributo de domínio/regra de negócio. Mapeado via Enum para restringir 
        // os valores aceitáveis no banco (geralmente mapeado como INT ou SMALLINT).
        public TipoLancamento Tipo { get; set; }

        // Atributo de auditoria/temporalidade. Registra o timestamp de persistência 
        // inicial do objeto no banco de dados.
        public DateTime DataCadastro { get; set; }
    }
}