using Financas.Api.DTOs.ContaBancaria;
using Financas.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Financas.Api.Controllers
{
    [ApiController]
    [Route("api/contas-bancarias")]
    public class ContaBancariaController : ControllerBase
    {
        private readonly ContaBancariaService _contaBancariaService;

        /// <summary>
        /// Construtor que recebe o serviço via Injeção de Dependência.
        /// </summary>
        public ContaBancariaController(ContaBancariaService contaBancariaService)
        {
            _contaBancariaService = contaBancariaService;
        }

        /// <summary>
        /// Endpoint para criação de conta. 
        /// Retorna 201 (Created) em caso de sucesso.
        /// </summary>
        [HttpPost]
        [Authorize] // Garante que apenas usuários logados acessem
        public async Task<ActionResult<ContaBancariaResponseDTO>> CriarContaBancaria([FromBody] CriarContaBancariaDTO dto)
        {
            try
            {
                // Extrai o ID do usuário diretamente das Claims do Token JWT
                var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

                var contaBancaria = await _contaBancariaService.CriarContaBancaria(dto, usuarioId);

                // Retorna o status 201 e o cabeçalho 'Location' apontando para o GET
                return CreatedAtAction(nameof(GetContaBancaria), new { id = contaBancaria.Id }, contaBancaria);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Recupera todas as contas bancárias do usuário autenticado.
        /// </summary>
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ContaBancariaResponseDTO>>> GetContaBancaria()
        {
            try
            {
                var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var contasBancarias = await _contaBancariaService.GetContaBancaria(usuarioId);
                return Ok(contasBancarias);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Atualiza parcialmente os dados de uma conta (PATCH).
        /// </summary>
        [HttpPatch("{id}")]
        [Authorize]
        public async Task<ActionResult<ContaBancariaResponseDTO>> AtualizarContaBancaria([FromBody] AtualizarContaBancariaDTO dto, int id)
        {
            try
            {
                var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var contaBancariaAtualizada = await _contaBancariaService.AtualizarContaBancaria(dto, id, usuarioId);

                return Ok(contaBancariaAtualizada);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); // 404 se a conta não existir
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid(); // 403 se tentar editar conta de outro usuário
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Remove uma conta bancária. Retorna 204 (No Content) em caso de sucesso.
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> DeletarContaBancaria(int id)
        {
            try
            {
                var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                await _contaBancariaService.DeletarContaBancaria(id, usuarioId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
