using Financas.Api.Data;
using Financas.Api.DTOs.CartaoCredito;
using Financas.Api.Entities;
using Financas.Api.Entities.Enums;
using Microsoft.EntityFrameworkCore;

namespace Financas.Api.Services
{
    // Classe de serviço para o Cartão de Crédito, responsável por realizar as operações de CRUD
    public class CartaoCreditoService
    {
        // Injeção de dependência do contexto do banco de dados para acessar os dados relacionados ao Cartão de Crédito
        private readonly FinancasDbContext _FinancasDbContext;

        // Construtor para injetar o contexto do banco de dados
        public CartaoCreditoService(FinancasDbContext financasDbContext)
        {
            _FinancasDbContext = financasDbContext;
        }

        /// <summary>
        /// Métodos Responsáveis para realizar as operações de CRUD para o Cartão de Crédito
        /// </summary>

        // Método para criar um novo Cartão de Crédito, recebendo os dados necessários através do DTO e o ID do usuário para associar o cartão ao usuário correto
        public async Task<CartaoCreditoResponseDTO> CriarCartaoCredito(CriarCartaoCreditoDTO dto, int usuarioId)
        {
            // 1. Validação de usuário
            var usuario = await _FinancasDbContext.Usuarios
                .FirstOrDefaultAsync(u => u.Id == usuarioId);

            if (usuario == null)
                throw new Exception("Usuário não encontrado");

            // 2. Regra de negócio
            if (dto.Limite <= 0)
                throw new ArgumentException("O limite do cartão deve ser maior que zero.");

            // 3. Criação da entidade
            var cartao = new CartaoCredito
            {
                UsuarioId = usuarioId,
                Nome = dto.Nome,
                Limite = dto.Limite,
                DiaFechamento = dto.DiaFechamento,
                Status = StatusCartaoCredito.Ativo
            };

            // 4. Persistência
            _FinancasDbContext.CartaoCredito.Add(cartao);
            await _FinancasDbContext.SaveChangesAsync();

            // 5. Response
            return new CartaoCreditoResponseDTO
            {
                Id = cartao.Id,
                UsuarioId = usuarioId,
                Nome = cartao.Nome,
                Limite = cartao.Limite,
                DiaFechamento = cartao.DiaFechamento,
                Status = cartao.Status.ToString()
            };
        }

        /// <summary>
        /// Listar os cartões de crédito de um usuário específico, filtrando por status (Ativo/Inativo) e ordenando pela data de fechamento mais próxima.
        /// </summary>

        // Método para listar os cartões de crédito de um usuário específico, filtrando por status (Ativo/Inativo) e ordenando pela data de fechamento mais próxima
        public async Task<List<CartaoCreditoResponseDTO>> GetCartaoCredito(int usuarioId)
        {
            // 1. Consulta ao Banco de Dados: O método inicia realizando uma consulta ao banco de dados para recuperar todos os cartões de crédito associados ao ID do usuário fornecido. A consulta é feita usando o Entity Framework Core, filtrando os cartões pelo ID do usuário e convertendo o resultado em uma lista assíncrona
            var cartoes = await _FinancasDbContext.CartaoCredito
                .Where(c => c.UsuarioId == usuarioId)
                .ToListAsync();

            // 2. Transformação dos Dados: Após recuperar os cartões de crédito do banco de dados, o método realiza uma transformação dos dados para convertê-los em uma lista de DTOs de resposta (CartaoCreditoResponseDTO). Cada cartão de crédito é mapeado para um DTO que contém as informações relevantes, como ID, ID do usuário, nome, limite, dia de fechamento e status do cartão. O status do cartão é convertido para uma string para facilitar a leitura no front-end
            return cartoes.Select(c => new CartaoCreditoResponseDTO
            {
                Id = c.Id,
                UsuarioId = c.UsuarioId,
                Nome = c.Nome,
                Limite = c.Limite,
                DiaFechamento = c.DiaFechamento,
                Status = c.Status.ToString()
            }).ToList();
        }

        /// <summary>
        /// Método responsável por atualizar o status de um cartão de crédito específico, permitindo ativar ou desativar o cartão conforme necessário.
        /// </summary>

        // Método para atualizar o status de um cartão de crédito específico, permitindo ativar ou desativar o cartão conforme necessário
        public async Task<CartaoCreditoResponseDTO> AtualizarCartaoCredito(
        AtualizarCartaoCreditoDTO dto,
        int cartaoId,
        int usuarioId)
        {
            // 1. Busca
            var cartao = await _FinancasDbContext.CartaoCredito
                .FirstOrDefaultAsync(c => c.Id == cartaoId);

            if (cartao == null)
                throw new Exception("Cartão não encontrado");

            // 2. Autorização
            if (cartao.UsuarioId != usuarioId)
                throw new UnauthorizedAccessException("Cartão não pertence ao usuário");

            // 3. Regras de atualização
            if (dto.Nome != null)
                cartao.Nome = dto.Nome;

            if (dto.Limite.HasValue)
            {
                if (dto.Limite.Value <= 0)
                    throw new ArgumentException("Limite deve ser maior que zero.");

                cartao.Limite = dto.Limite.Value;
            }

            if (dto.Status.HasValue)
                cartao.Status = dto.Status.Value;

            if (dto.DiaFechamento.HasValue)
                cartao.DiaFechamento = dto.DiaFechamento.Value;

            // 4. Persistência
            await _FinancasDbContext.SaveChangesAsync();

            // 5. Response
            return new CartaoCreditoResponseDTO
            {
                Id = cartao.Id,
                UsuarioId = usuarioId,
                Nome = cartao.Nome,
                Limite = cartao.Limite,
                DiaFechamento = cartao.DiaFechamento,
                Status = cartao.Status.ToString()
            };
        }

        /// <summary>
        /// Método responsável por excluir um cartão de crédito específico, removendo-o do banco de dados e garantindo que ele não possa mais ser acessado ou utilizado.
        /// </summary>

        // Método para excluir um cartão de crédito específico, removendo-o do banco de dados e garantindo que ele não possa mais ser acessado ou utilizado
        public async Task DeletarCartao(int cartaoId, int usuarioId)
        {
            var cartao = await _FinancasDbContext.CartaoCredito
                .FirstOrDefaultAsync(c => c.Id == cartaoId);

            if (cartao == null)
                throw new Exception("Cartão não encontrado");

            if (cartao.UsuarioId != usuarioId)
                throw new UnauthorizedAccessException("Cartão não pertence ao usuário");

            _FinancasDbContext.CartaoCredito.Remove(cartao);
            await _FinancasDbContext.SaveChangesAsync();
        }
    }
}
