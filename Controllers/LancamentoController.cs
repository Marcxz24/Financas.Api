using Financas.Api.DTOs;
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
    }
}