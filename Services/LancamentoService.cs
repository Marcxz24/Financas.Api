using Financas.Api.Data;
using Financas.Api.DTOs.Lancamento;
using Financas.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Financas.Api.Services
{
    public class LancamentoService
    {
        // Variável privada para armazenar o contexto do banco de dados (MySQL)
        private readonly FinancasDbContext _financasDbContext;

        // Construtor que recebe o contexto via Injeção de Dependência
        public LancamentoService(FinancasDbContext financasDbContext)
        {
            _financasDbContext = financasDbContext;
        }

        // Método para criar um novo lançamento (receita ou despesa)
        public async Task<LancamentoResponseDTO> CriarLancamento(CriarLancamentoDTO dto, int usuarioId)
        {
            // 1. Validação: Verifica se o usuário que está tentando criar o lançamento realmente existe
            var usuario = await _financasDbContext.Usuarios
                .FirstOrDefaultAsync(u => u.Id == usuarioId);

            if (usuario == null)
                throw new Exception("Usuário não encontrado");

            if (dto.CategoriaId == 0)
                dto.CategoriaId = null; // Permite que o usuário envie "0" para criar um lançamento sem categoria

            if (dto.CategoriaId != null)
            {
                // Verifica se a categoria existe e pertence ao usuário
                var categoria = await _financasDbContext.Categorias
                    .FirstOrDefaultAsync(c => c.Id == dto.CategoriaId && c.UsuarioId == usuarioId);

                if (categoria == null)
                    throw new KeyNotFoundException("Categoria não encontrada ou não pertence ao usuário.");
            }

            // 2. Mapeamento: Transforma o DTO (dados que vieram da web) na Entidade (classe que vai pro banco)
            var lancamento = new Lancamento
            {
                Descricao = dto.Descricao,
                Valor = dto.Valor,
                Data = dto.Data,
                Tipo = dto.Tipo,
                UsuarioId = usuarioId, // Vincula o lançamento ao ID do usuário logado
                CategoriaId = dto.CategoriaId // Pode ser nulo, o que é permitido pela configuração do banco
            };

            // 3. Persistência: Adiciona o objeto ao rastreamento do EF Core e salva no MySQL
            _financasDbContext.Lancamentos.Add(lancamento);
            await _financasDbContext.SaveChangesAsync();

            // 4. Retorno: Transforma a entidade salva em um DTO de resposta (Response)
            return new LancamentoResponseDTO
            {
                Id = lancamento.Id,
                UsuarioId = usuarioId,
                Descricao = lancamento.Descricao,
                Valor = lancamento.Valor,
                Data = lancamento.Data,
                Tipo = lancamento.Tipo.ToString(), // Converte o Enum de Tipo para String
                CategoriaId = lancamento.CategoriaId,
                CategoriaNome = lancamento.Categoria?.Nome
            };
        }

        // Método para buscar todos os lançamentos de um usuário específico
        public async Task<List<LancamentoResponseDTO>> GetLancamentos(int usuarioId)
        {
            // Busca no banco apenas os lançamentos que pertencem ao usuário (Filtro UsuarioId)
            var lancamentos = await _financasDbContext.Lancamentos
                .Include(l => l.Categoria) // Inclui os dados da categoria relacionada, se houver
                .Where(l => l.UsuarioId == usuarioId)
                .ToListAsync();

            // Converte a lista de entidades para uma lista de DTOs antes de enviar para o Controller
            return lancamentos.Select(l => new LancamentoResponseDTO
            {
                Id = l.Id,
                UsuarioId = l.UsuarioId,
                Descricao = l.Descricao,
                Valor = l.Valor,
                Data = l.Data,
                Tipo = l.Tipo.ToString(),
                CategoriaId = l.CategoriaId,
                CategoriaNome = l.Categoria?.Nome
            }).ToList();
        }

        public async Task<LancamentoResponseDTO> AtualizarLancamento(AtualizarLancamentoDTO dto, int lancamentoId, int usuarioId)
        {
            // 1. Busca o lançamento no banco de dados pelo ID fornecido
            var lancamento = await _financasDbContext.Lancamentos
                .Include(l => l.Categoria) // Inclui os dados da categoria para possível retorno no DTO
                .FirstOrDefaultAsync(l => l.Id == lancamentoId);

            // 2. Validação de existência: Verifica se o registro realmente existe no MySQL
            if (lancamento == null)
                throw new KeyNotFoundException("Lançamento não encontrado.");

            // 3. Regra de Segurança Crítica: Verifica se o lançamento pertence ao usuário logado.
            // Isso impede que um usuário tente editar o ID de um lançamento de terceiros via API.
            if (lancamento.UsuarioId != usuarioId)
                throw new UnauthorizedAccessException("Sem permissão para alterar este lançamento.");

            // 4. Atualização Parcial: Os IFs abaixo permitem que o usuário envie apenas o que deseja mudar.
            // Se o campo no DTO estiver nulo, o valor atual no banco é preservado.

            if (dto.CategoriaId == 0)
                dto.CategoriaId = null; // Permite que o usuário envie "0" para remover a categoria

            if (dto.Descricao != null)
                lancamento.Descricao = dto.Descricao;

            if (dto.Valor != null)
                lancamento.Valor = (decimal)dto.Valor; // Realiza o cast para decimal

            if (dto.Tipo != null)
                lancamento.Tipo = (Entities.Enums.TipoLancamento)dto.Tipo; // Converte o valor recebido para o Enum correspondente

            if (dto.Data != null)
                lancamento.Data = dto.Data.Value;

            if (dto.CategoriaId != null)
            {
                var categoria = await _financasDbContext.Categorias
                    .FirstOrDefaultAsync(c => c.Id == dto.CategoriaId && c.UsuarioId == usuarioId);

                if (categoria == null)
                    throw new KeyNotFoundException("Categoria não encontrada ou não pertence ao usuário.");

                lancamento.CategoriaId = dto.CategoriaId; // Permite atualizar a categoria, inclusive para null (sem categoria)
            }

            // 5. Persistência: O EF Core detecta que o objeto 'lancamento' foi modificado e gera o comando UPDATE
            await _financasDbContext.SaveChangesAsync();

            // 6. Retorno: Devolve o objeto atualizado formatado para a resposta da API (DTO)
            return new LancamentoResponseDTO
            {
                Id = lancamento.Id,
                UsuarioId = usuarioId,
                Descricao = lancamento.Descricao,
                Valor = lancamento.Valor,
                Tipo = lancamento.Tipo.ToString(), // Converte o Enum para string (ex: "Receita")
                Data = lancamento.Data,
                CategoriaId = lancamento.CategoriaId,
                CategoriaNome = lancamento.Categoria?.Nome
            };
        }

        public async Task DeletarLancamento(int lancamentoId, int usuarioId)
        {
            // 1. Busca o lançamento no banco de dados
            // É necessário buscar o objeto completo para que o EF saiba o que remover
            var lancamento = await _financasDbContext.Lancamentos
                .FirstOrDefaultAsync(l => l.Id == lancamentoId);

            // 2. Verificação de existência
            // Evita erro de referência nula caso o ID não exista no MySQL
            if (lancamento == null)
                throw new Exception("Lançamento não encontrado.");

            // 3. Barreira de Segurança:
            // Mesmo que alguém saiba o ID de um lançamento, ele só consegue deletar
            // se o UsuarioId do registro for igual ao ID extraído do Token JWT.
            if (lancamento.UsuarioId != usuarioId)
                throw new Exception("Não é possível excluir um lançamento de outro usuário.");

            // 4. Marcação para remoção:
            // O método .Remove() avisa ao Entity Framework que este objeto deve ser deletado
            _financasDbContext.Lancamentos.Remove(lancamento);

            // 5. Execução no Banco:
            // Aqui o EF Core gera o comando SQL "DELETE FROM Lancamentos WHERE Id = ..."
            await _financasDbContext.SaveChangesAsync();
        }
    }
}