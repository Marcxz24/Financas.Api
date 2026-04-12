using Financas.Api.DTOs.Lancamento;
using Financas.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Financas.Api.Controllers
{
    // Indica que esta classe é um controlador de API e habilita comportamentos automáticos (como validação de modelos)
    [ApiController]
    // Define a rota base para acessar este controlador: ex: https://localhost:7041/api/lancamentos
    [Route("api/lancamentos")]
    public class LancamentoController : ControllerBase
    {
        private readonly LancamentoService _lancamentoService;

        // Injeção de Dependência: O ASP.NET fornece o serviço de lançamentos automaticamente
        public LancamentoController(LancamentoService lancamentoService)
        {
            _lancamentoService = lancamentoService;
        }

        // Endpoint para criar um novo registro (Receita ou Despesa)
        [HttpPost]
        [Authorize] // Bloqueia o acesso se o usuário não estiver logado (sem token JWT)
        public async Task<ActionResult<LancamentoResponseDTO>> CriarLancamento([FromBody] CriarLancamentoDTO dto)
        {
            try
            {
                // Extrai o ID do usuário autenticado a partir dos Claims (informações dentro do Token JWT)
                var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

                // Chama o serviço para salvar o lançamento no banco de dados
                var lancamento = await _lancamentoService.CriarLancamento(dto, usuarioId);

                // Retorna o status 201 (Created) e indica onde o recurso pode ser consultado
                return CreatedAtAction(nameof(GetLancamentos), new { id = lancamento.Id }, lancamento);
            }
            catch (Exception ex)
            {
                // Retorna erro 400 caso algo dê errado (ex: saldo insuficiente ou dados inválidos)
                return BadRequest(ex.Message);
            }
        }

        // Endpoint para listar todos os lançamentos do usuário logado
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<LancamentoResponseDTO>>> GetLancamentos()
        {
            try
            {
                // Identifica quem é o usuário para não mostrar os lançamentos de outras pessoas
                var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

                // Busca a lista de lançamentos no banco via serviço
                var lancamentos = await _lancamentoService.GetLancamentos(usuarioId);

                // Retorna status 200 (OK) com a lista de dados
                return Ok(lancamentos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Define que este método responde ao verbo HTTP PATCH.
        // O PATCH é usado para atualizações parciais (mudar apenas alguns campos).
        // O "{id}" na rota mapeia o ID do lançamento vindo da URL.
        [HttpPatch("{id}")]
        [Authorize] // Proteção para garantir que apenas usuários logados acessem.
        public async Task<ActionResult> AtualizarLancamento(int id, [FromBody] AtualizarLancamentoDTO dto)
        {
            try
            {
                // 1. Identificação: Extrai o ID do usuário logado do Token JWT.
                // O "!" confirma que o valor não será nulo, pois o [Authorize] já validou o acesso.
                var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

                // 2. Processamento: Chama o Service enviando:
                // - O 'dto': que contém apenas os campos que o usuário quer mudar.
                // - O 'id': do lançamento que será editado.
                // - O 'usuarioId': para validar se o usuário é dono deste lançamento.
                var lancamentoAtualizado = await _lancamentoService.AtualizarLancamento(dto, id, usuarioId);

                // 3. Resposta: Retorna o status 200 (OK) enviando de volta o lançamento já atualizado.
                // Isso é útil para o Frontend confirmar que os dados foram salvos corretamente.
                return Ok(lancamentoAtualizado);
            }
            catch (Exception ex)
            {
                // 4. Erro: Se o registro não existir ou houver falha na permissão, retorna 400 (Bad Request).
                return BadRequest(ex.Message);
            }
        }

        // Define que este método responde ao verbo HTTP DELETE.
        // O parâmetro "{lancamentoId}" na rota indica que o ID deve vir na URL (ex: api/lancamentos/10).
        [HttpDelete("{lancamentoId}")]
        [Authorize] // Garante que apenas usuários autenticados (com token JWT) acessem este endpoint.
        public async Task<ActionResult> DeletarLancamento(int lancamentoId)
        {
            try
            {
                // 1. Identificação do Autor: Extrai o ID do usuário logado a partir das Claims do Token.
                // O "!" ao final indica ao compilador que temos certeza que esse valor não será nulo (devido ao [Authorize]).
                var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

                // 2. Chamada do Serviço: Repassa a responsabilidade de exclusão para a camada de negócio.
                // Enviamos o ID do lançamento e o ID do usuário para validar a posse do registro.
                await _lancamentoService.DeletarLancamento(lancamentoId, usuarioId);

                // 3. Resposta de Sucesso: O 'NoContent()' retorna o status HTTP 204.
                // É o padrão mais elegante para exclusões, pois indica que a operação foi um sucesso, 
                // mas não há mais conteúdo para exibir (já que o registro sumiu).
                return NoContent();
            }
            catch (Exception ex)
            {
                // 4. Tratamento de Erro: Se o lançamento não existir ou pertencer a outro usuário,
                // o erro disparado no Service é capturado aqui e retorna um status 400 (Bad Request).
                return BadRequest(ex.Message);
            }
        }
    }
}