using Financas.Api.Data;
using Financas.Api.DTOs.Lancamento;
using Financas.Api.Entities;
using Financas.Api.Entities.Enums;
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

        // Este método aplica uma alteração financeira ao saldo atual da conta
        private void AplicarValor(ContaBancaria conta, decimal valor, TipoLancamento tipo)
        {
            if (conta == null)
                throw new ArgumentNullException(nameof(conta), "A conta bancária não pode ser nula.");

            if (valor <= 0)
                throw new ArgumentException("O valor deve ser maior que zero.");

            // Verifica a direção do fluxo financeiro através do Enum 'TipoLancamento'
            if (tipo == TipoLancamento.Receita)
            {
                // Algoritmo: Se for entrada (Receita), utiliza o operador de atribuição aditiva
                // O valor é somado ao saldo existente na memória da entidade
                conta.Saldo += valor;
            }
            else
            {
                // Antes de subtrair, verifica se o saldo é suficiente para cobrir a despesa.
                if (conta.Saldo < valor)
                    throw new InvalidOperationException("Saldo insuficiente.");

                // Algoritmo: Se for saída (Despesa), utiliza o operador de atribuição subtrativa
                // O valor é deduzido do saldo atual da conta
                conta.Saldo -= valor;
            }
        }

        // Este método reverte uma operação financeira anterior para restaurar o saldo original
        private void EstornarValor(ContaBancaria conta, decimal valor, TipoLancamento tipo)
        {
            if (conta == null)
                throw new ArgumentNullException(nameof(conta), "A conta bancária não pode ser nula.");

            if (valor <= 0)
                throw new ArgumentException("O valor deve ser maior que zero.");

            // O algoritmo de estorno inverte a lógica do método AplicarValor
            if (tipo == TipoLancamento.Receita)
            {
                // Algoritmo: Para cancelar uma Receita que já foi somada, subtraímos o valor
                // Isso devolve o saldo ao estado anterior à criação do lançamento
                conta.Saldo -= valor;
            }
            else
            {
                // Algoritmo: Para cancelar uma Despesa que já foi subtraída, somamos o valor de volta
                // Isso recompõe o saldo que havia sido reduzido indevidamente ou que será editado
                conta.Saldo += valor;
            }
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

            if (dto.ContaBancariaId == 0)
                dto.ContaBancariaId = null; // Permite que o usuário envie "0" para criar um lançamento sem conta bancária

            if (dto.CategoriaId != null)
            {
                // Verifica se a categoria existe e pertence ao usuário
                var categoria = await _financasDbContext.Categorias
                    .FirstOrDefaultAsync(c => c.Id == dto.CategoriaId && c.UsuarioId == usuarioId);

                if (categoria == null)
                    throw new KeyNotFoundException("Categoria não encontrada ou não pertence ao usuário.");
            }

            ContaBancaria? contaBancaria = null;

            if (dto.ContaBancariaId != null)
            {
                contaBancaria = await _financasDbContext.ContasBancarias
                    .FirstOrDefaultAsync(c => c.Id == dto.ContaBancariaId && c.UsuarioId == usuarioId);

                if (contaBancaria == null)
                    throw new KeyNotFoundException("Conta bancária não encontrada ou não pertence ao usuário.");
            }

            // 2. Mapeamento: Transforma o DTO (dados que vieram da web) na Entidade (classe que vai pro banco)
            var lancamento = new Lancamento
            {
                Descricao = dto.Descricao,
                Valor = dto.Valor,
                Data = dto.Data,
                Tipo = dto.Tipo,
                UsuarioId = usuarioId, // Vincula o lançamento ao ID do usuário logado
                CategoriaId = dto.CategoriaId, // Pode ser nulo, o que é permitido pela configuração do banco
                ContaBancariaId = dto.ContaBancariaId // Pode ser nulo, o que é permitido pela configuração do banco
            };

            // Se houver uma conta vinculada, atualiza o saldo em memória
            if (contaBancaria != null)
            {
                // Soma se for Receita ou subtrai se for Despesa, 
                // preparando a alteração para ser salva no banco de dados.
                AplicarValor(contaBancaria, dto.Valor, dto.Tipo);
            }

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
                CategoriaNome = lancamento.Categoria?.Nome,
                ContaBancariaId = lancamento.ContaBancariaId,
                ContaBancariaNome = lancamento.ContaBancaria?.Nome
            };
        }

        // Método para buscar todos os lançamentos de um usuário específico
        public async Task<List<LancamentoResponseDTO>> GetLancamentos(int usuarioId)
        {
            // Busca no banco apenas os lançamentos que pertencem ao usuário (Filtro UsuarioId)
            var lancamentos = await _financasDbContext.Lancamentos
                .Include(l => l.Categoria) // Inclui os dados da categoria relacionada, se houver
                .Include(c => c.ContaBancaria) // Inclui os dados da conta bancária relacionada, se houver
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
                CategoriaNome = l.Categoria?.Nome,
                ContaBancariaId = l.ContaBancariaId,
                ContaBancariaNome = l.ContaBancaria?.Nome
            }).ToList();
        }

        public async Task<LancamentoResponseDTO> AtualizarLancamento(AtualizarLancamentoDTO dto, int lancamentoId, int usuarioId)
        {
            // 1. Busca o lançamento no banco de dados pelo ID fornecido
            var lancamento = await _financasDbContext.Lancamentos
                .Include(l => l.Categoria) // Inclui os dados da categoria para possível retorno no DTO
                .Include(c => c.ContaBancaria) // Inclui os dados da conta bancária para possível retorno no DTO
                .FirstOrDefaultAsync(l => l.Id == lancamentoId);

            // 2. Validação de existência: Verifica se o registro realmente existe no MySQL
            if (lancamento == null)
                throw new KeyNotFoundException("Lançamento não encontrado.");

            // 3. Regra de Segurança Crítica: Verifica se o lançamento pertence ao usuário logado.
            // Isso impede que um usuário tente editar o ID de um lançamento de terceiros via API.
            if (lancamento.UsuarioId != usuarioId)
                throw new UnauthorizedAccessException("Sem permissão para alterar este lançamento.");

            if (dto.ContaBancariaId.HasValue && dto.ContaBancariaId != lancamento.ContaBancariaId)
                throw new InvalidOperationException("Não é permitido alterar a conta bancária de um lançamento. Delete e recrie.");

            if (dto.Valor.HasValue && dto.Valor <= 0)
                throw new ArgumentException("O valor deve ser maior que zero.");

            var contaAntiga = lancamento.ContaBancaria; // Armazena a conta antiga para estorno caso seja necessário
            var valorAntigo = lancamento.Valor; // Armazena o valor antigo para estorno caso seja necessário
            var tipoAntigo = lancamento.Tipo; // Armazena o tipo antigo para estorno caso seja necessário

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

            var houveMudancaFincaneira = 
                lancamento.Valor != valorAntigo ||
                lancamento.Tipo != tipoAntigo;

            if (dto.CategoriaId != null)
            {
                var categoria = await _financasDbContext.Categorias
                    .FirstOrDefaultAsync(c => c.Id == dto.CategoriaId && c.UsuarioId == usuarioId);

                if (categoria == null)
                    throw new KeyNotFoundException("Categoria não encontrada ou não pertence ao usuário.");

                lancamento.CategoriaId = dto.CategoriaId; // Permite atualizar a categoria, inclusive para null (sem categoria)

                lancamento.Categoria = categoria; // Atualiza a referência da categoria para garantir que os dados relacionados sejam carregados corretamente no retorno do DTO
            }

            // 5. Persistência: O EF Core detecta que o objeto 'lancamento' foi modificado e gera o comando UPDATE
            using var transaction = await _financasDbContext.Database.BeginTransactionAsync(); // Inicia uma transação para garantir a atomicidade das operações

            try
            {
                if (houveMudancaFincaneira)
                {
                    if (contaAntiga != null)
                        EstornarValor(contaAntiga, valorAntigo, tipoAntigo); // Reverte o valor antigo para restaurar o saldo da conta bancária antes de aplicar a nova alteração

                    if (lancamento.ContaBancaria != null)
                        AplicarValor(lancamento.ContaBancaria, lancamento.Valor, lancamento.Tipo); // Aplica o novo valor para atualizar o saldo da conta bancária com a alteração feita   
                }

                await _financasDbContext.SaveChangesAsync(); // Salva as alterações no banco de dados, incluindo o lançamento e a conta bancária (se houver)
                await transaction.CommitAsync(); // Confirma a transação, garantindo que todas as alterações sejam aplicadas no banco de dados
            }
            catch
            {
                await transaction.RollbackAsync(); // Em caso de erro, desfaz todas as alterações
                throw; // Relança a exceção para que o controlador possa lidar com ela adequadamente
            }

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
                CategoriaNome = lancamento.Categoria?.Nome,
                ContaBancariaId = lancamento.ContaBancariaId,
                ContaBancariaNome = lancamento.ContaBancaria?.Nome
            };
        }

        public async Task DeletarLancamento(int lancamentoId, int usuarioId)
        {
            // 1. Busca o lançamento no banco de dados
            // É necessário buscar o objeto completo para que o EF saiba o que remover
            var lancamento = await _financasDbContext.Lancamentos
                .Include(l => l.ContaBancaria)
                .FirstOrDefaultAsync(l => l.Id == lancamentoId);

            // 2. Verificação de existência
            // Evita erro de referência nula caso o ID não exista no MySQL
            if (lancamento == null)
                throw new KeyNotFoundException("Lançamento não encontrado.");

            // 3. Barreira de Segurança:
            // Mesmo que alguém saiba o ID de um lançamento, ele só consegue deletar
            // se o UsuarioId do registro for igual ao ID extraído do Token JWT.
            if (lancamento.UsuarioId != usuarioId)
                throw new UnauthorizedAccessException("Não é possível excluir um lançamento de outro usuário.");

            using var transaction = await _financasDbContext.Database.BeginTransactionAsync(); // Inicia uma transação para garantir a atomicidade das operações

            try
            {
                if (lancamento.ContaBancaria != null)
                    EstornarValor(lancamento.ContaBancaria, lancamento.Valor, lancamento.Tipo); // Reverte o valor para restaurar o saldo da conta bancária antes de excluir o lançamento

                // 4. Marcação para remoção:
                // O método .Remove() avisa ao Entity Framework que este objeto deve ser deletado
                _financasDbContext.Lancamentos.Remove(lancamento);

                // 5. Execução no Banco:
                // Aqui o EF Core gera o comando SQL "DELETE FROM Lancamentos WHERE Id = ..."
                await _financasDbContext.SaveChangesAsync();
                await transaction.CommitAsync(); // Confirma a transação, garantindo que todas as alterações sejam aplicadas no banco de dados
            }
            catch
            {
                await transaction.RollbackAsync(); // Em caso de erro, desfaz todas as alterações
                throw; // Relança a exceção para que o controlador possa lidar com ela adequadamente
            }
        }
    }
}