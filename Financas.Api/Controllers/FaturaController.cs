using Financas.Api.DTOs.Fatura;
using Financas.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Financas.Api.Controllers
{
    [ApiController]
    [Route("api/fatura")]
    [Authorize]
    public class FaturaController : ControllerBase
    {
        private readonly FaturaService _faturaService;

        /// <summary>
        /// Construtor da Controller, injetando as dependências necessárias para lidar com faturas.
        /// </summary>
        public FaturaController(FaturaService faturaService)
        {
            _faturaService = faturaService;
        }

        /// <summary>
        /// Endpoint para realizar o fechamento manual de uma fatura específica.
        /// </summary>
        /// <param name="faturaId">ID da fatura extraído da URL.</param>
        /// <returns>Mensagem de sucesso ou erro formatado.</returns>
        [HttpPost("{faturaId}/fechar")]
        [Authorize]
        public async Task<IActionResult> FecharFatura(int faturaId)
        {
            try
            {
                // Extrai o ID do usuário do Token JWT para garantir que ele só manipule seus próprios dados
                var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userIdClaim))
                    return Unauthorized(new { mensagem = "Usuário não autenticado." });

                var usuarioId = int.Parse(userIdClaim);

                // Chama o serviço para processar o fechamento e a abertura do novo ciclo
                await _faturaService.FecharFatura(faturaId, usuarioId);

                return Ok(new { mensagem = "Fatura fechada com sucesso." });
            }
            catch (Exception ex)
            {
                // Retorna erros de validação (ex: tentar fechar antes da data) como BadRequest
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Endpoint para listar todas as faturas associadas ao usuário autenticado.
        /// </summary>
        /// <returns>Uma lista de FaturaResponseDTO com o histórico financeiro.</returns>
        [HttpGet("listar")]
        [Authorize]
        public async Task<IActionResult> GetFatura()
        {
            try
            {
                var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userIdClaim))
                    return Unauthorized(new { mensagem = "Usuário não autenticado." });

                var usuarioId = int.Parse(userIdClaim);

                // Busca as faturas ordenadas pelo Service
                var fatura = await _faturaService.ListarFaturas(usuarioId);

                return Ok(fatura);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Endpoint para processar o pagamento (total ou parcial) de uma fatura.
        /// </summary>
        /// <param name="dto">Dados do pagamento enviados no corpo da requisição.</param>
        /// <returns>Confirmação de pagamento ou mensagem de erro.</returns>
        [HttpPost("pagar")]
        [Authorize]
        public async Task<IActionResult> PagarFatura([FromBody] PagarFaturaDTO dto)
        {
            try
            {
                var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userIdClaim))
                    return Unauthorized(new { mensagem = "Usuário não autenticado." });

                var usuarioId = int.Parse(userIdClaim);

                // Processa o pagamento, baixa no saldo da conta e atualização de status
                await _faturaService.PagarFatura(dto, usuarioId);

                return Ok(new { mensagem = "Fatura paga com sucesso." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{faturaId}/extrato")]
        [Authorize]
        public async Task<IActionResult> ObterExtrato(int faturaId)
        {
            try
            {
                var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userIdClaim))
                    return Unauthorized(new { mensagem = "Usuário não autenticado." });

                var usuarioId = int.Parse(userIdClaim);

                var extrato = await _faturaService.ObterExtratoFatura(faturaId, usuarioId);

                return Ok(extrato);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }
    }
}