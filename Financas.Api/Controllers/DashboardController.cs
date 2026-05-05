using Financas.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Financas.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [Authorize]
        [HttpGet("resumo-mensal")]
        public async Task<IActionResult> GetResumoMensal([FromQuery] int? mes, [FromQuery] int? ano)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized(new { mensagem = "Usuário não autenticado." });

            int usuarioId = int.Parse(userIdClaim);
            int mesFiltro = mes ?? DateTime.Now.Month;
            int anoFiltro = ano ?? DateTime.Now.Year;

            var resumo = await _dashboardService.GetResumoMensalAsync(mesFiltro, anoFiltro, usuarioId);

            return Ok(resumo);
        }
    }
}
