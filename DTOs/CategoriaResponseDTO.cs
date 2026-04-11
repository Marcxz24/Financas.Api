namespace Financas.Api.DTOs
{
    public class CategoriaResponseDTO
    {
        // Identificador único da categoria. 
        // Essencial para que o Frontend saiba qual ID passar em futuras chamadas de Update ou Delete.
        public int Id { get; set; }

        // Identificador do proprietário do recurso. 
        // Utilizado para validações de contexto e filtragem no lado do cliente.
        public int UsuarioId { get; set; }

        // Rótulo descritivo da categoria. 
        // Transportado como string bruta para exibição direta em componentes de interface.
        public string Nome { get; set; } = string.Empty;

        // Identificador da referência visual. 
        // O Frontend utilizará esta string para mapear e renderizar o ícone correspondente.
        public string Icone { get; set; } = string.Empty;

        // Representação textual do tipo (ex: "Receita" ou "Despesa").
        // Diferente da Entidade (Enum/Int), aqui o dado é convertido para string 
        // para facilitar a leitura e evitar lógica de tradução no Frontend.
        public string Tipo { get; set; } = string.Empty;

        // Timestamp de criação do registro. 
        // Fornecido para que a interface possa exibir a data de cadastro ou ordenar listas cronologicamente.
        public DateTime DataCadastro { get; set; }
    }
}
