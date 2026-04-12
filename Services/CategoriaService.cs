using Financas.Api.Data;
using Financas.Api.DTOs.Categoria;
using Financas.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Financas.Api.Services
{
    public class CategoriaService
    {
        // Campo privado e somente leitura que armazena a instância do contexto do banco de dados.
        // O modificador 'readonly' garante que a referência não seja alterada após a inicialização.
        private readonly FinancasDbContext _financasDbContext;

        // Construtor da classe que utiliza Injeção de Dependência (DI).
        // O ASP.NET Core instanciará o DbContext e o injetará automaticamente no serviço.
        public CategoriaService(FinancasDbContext financasDbContext)
        {
            _financasDbContext = financasDbContext;
        }

        // Método assíncrono para a criação de novas categorias no sistema.
        public async Task<CategoriaResponseDTO> CriarCategorias(CriarCategoriaDTO dto, int usuarioId)
        {
            // 1. Validação de Integridade: Verifica no banco de dados se o 'usuarioId' enviado existe.
            // Isso impede a criação de categorias "órfãs" que não pertenceriam a ninguém.
            var usuario = await _financasDbContext.Usuarios
                .FirstOrDefaultAsync(u => u.Id == usuarioId);

            // 2. Fluxo de Exceção: Interrompe a execução caso o usuário não seja localizado.
            if (usuario == null)
                throw new Exception("Usuário não encontrado");

            // 3. Mapeamento de Objeto (Entidade): Transforma os dados de entrada (DTO) no modelo de domínio.
            // Atribui a data de cadastro no padrão UTC para garantir consistência temporal global.
            var categoria = new Categoria
            {
                UsuarioId = usuarioId,
                Nome = dto.Nome,
                Icone = dto.Icone,
                Tipo = dto.Tipo,
                DataCadastro = DateTime.UtcNow
            };

            // 4. Persistência:
            // .Add(categoria): Adiciona o objeto ao rastreamento do EF Core (Estado: Added).
            // SaveChangesAsync(): Executa o comando SQL 'INSERT' de forma assíncrona no MySQL.
            _financasDbContext.Categorias.Add(categoria);
            await _financasDbContext.SaveChangesAsync();

            // 5. Mapeamento de Saída (Response): Converte a Entidade persistida de volta para um DTO.
            // O campo 'Tipo' é convertido de Enum para String (.ToString()) para o consumo da API.
            return new CategoriaResponseDTO
            {
                Id = categoria.Id,
                UsuarioId = usuarioId,
                Nome = categoria.Nome,
                Icone = categoria.Icone,
                Tipo = categoria.Tipo.ToString(),
                DataCadastro = categoria.DataCadastro
            };
        }

        // Método para buscar todas as categorias de um usuário específico
        public async Task<List<CategoriaResponseDTO>> GetCategorias(int usuarioId)
        {
            // Consulta assíncrona para obter todas as categorias associadas a um usuário específico.
            var categorias = await _financasDbContext.Categorias
                .Where(c => c.UsuarioId == usuarioId)
                .ToListAsync();

            // Mapeamento de cada entidade 'Categoria' para um DTO de resposta 'CategoriaResponseDTO'.
            return categorias.Select(c => new CategoriaResponseDTO
            {
                Id = c.Id,
                UsuarioId = c.UsuarioId,
                Nome = c.Nome,
                Icone = c.Icone,
                Tipo = c.Tipo.ToString(),
                DataCadastro = c.DataCadastro
            }).ToList();
        }

        public async Task<CategoriaResponseDTO> AtualizarCategoria(AtualizarCategoriaDTO dto, int categoriaId, int usuarioId)
        {
            // 1. Recuperação de Estado: Realiza uma consulta assíncrona para localizar a entidade no banco.
            // O rastreamento (Tracking) do EF Core é ativado automaticamente para permitir a detecção de mudanças.
            var categoria = await _financasDbContext.Categorias
                .FirstOrDefaultAsync(c => c.Id == categoriaId);

            // 2. Validação de Existência: Interrompe o fluxo caso o identificador (PK) não corresponda a nenhum registro.
            // O uso de 'KeyNotFoundException' é semanticamente correto para erros de busca por chave.
            if (categoria == null)
                throw new KeyNotFoundException("Categoria não encontrada");

            // 3. Verificação de Autorização (Multi-tenancy): Valida se o 'UsuarioId' do registro 
            // coincide com o 'UsuarioId' extraído do Token. Isso impede a manipulação de dados entre usuários.
            if (categoria.UsuarioId != usuarioId)
                throw new UnauthorizedAccessException("A categoria não pertence ao usuário");

            // 4. Algoritmo de Atualização Parcial (PATCH Logic):
            // Cada condicional verifica a presença de dados no DTO (null check).
            // Se o valor for 'null', a propriedade da entidade permanece com o valor original do banco.

            if (dto.Nome != null)
                categoria.Nome = dto.Nome;

            if (dto.Icone != null)
                categoria.Icone = dto.Icone;

            if (dto.Tipo != null)
                categoria.Tipo = dto.Tipo.Value; // Atribui o valor do Enum contido no Nullable

            // 5. Sincronização e Persistência:
            // O comando 'SaveChangesAsync' dispara o motor de 'Change Tracking' do EF Core,
            // que gera um comando SQL 'UPDATE' contendo apenas as colunas que sofreram alteração.
            await _financasDbContext.SaveChangesAsync();

            // 6. Projeção de Saída: Instancia e retorna o DTO de resposta atualizado.
            // Converte o estado final da entidade (incluindo o Enum para String) para o contrato da API.
            return new CategoriaResponseDTO
            {
                Id = categoria.Id,
                UsuarioId = usuarioId,
                Nome = categoria.Nome,
                Icone = categoria.Icone,
                Tipo = categoria.Tipo.ToString(),
                DataCadastro = categoria.DataCadastro
            };
        }

        public async Task DeletarCategoria(int categoriaId, int usuarioId)
        {
            // 1. Recuperação de Estado: Localiza a entidade no banco de dados para garantir que ela exista.
            var categoria = await _financasDbContext.Categorias
                .FirstOrDefaultAsync(c => c.Id == categoriaId);

            // 2. Validação de Existência: Interrompe o fluxo caso o registro não seja encontrado.
            if (categoria == null)
                throw new KeyNotFoundException("Categoria não encontrada");

            // 3. Verificação de Autorização: Garante que o usuário só possa deletar suas próprias categorias.
            if (categoria.UsuarioId != usuarioId)
                throw new UnauthorizedAccessException("A categoria não pertence ao usuário");

            // 4. Remoção e Persistência:
            // O método 'Remove' marca a entidade para exclusão, e 'SaveChangesAsync' executa o comando SQL 'DELETE'.
            _financasDbContext.Categorias.Remove(categoria);
            await _financasDbContext.SaveChangesAsync();
        }
    }
}
