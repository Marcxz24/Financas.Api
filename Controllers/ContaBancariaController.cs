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

        public ContaBancariaController(ContaBancariaService contaBancariaService)
        {
            _contaBancariaService = contaBancariaService;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ContaBancariaResponseDTO>> CriarContaBancaria([FromBody] CriarContaBancariaDTO dto)
        {
            try
            {
                var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

                var contaBancaria = await _contaBancariaService.CriarContaBancaria(dto, usuarioId);
                return CreatedAtAction(nameof(GetContaBancaria), new { id = contaBancaria.Id }, contaBancaria);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

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
