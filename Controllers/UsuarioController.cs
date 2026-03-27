using Financas.Api.Data;
using Financas.Api.Entities;
using Financas.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Financas.Api.Controllers
{
    [ApiController]
    [Route("api/usuarios")]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;

        public UsuarioController(UsuarioService userService) 
        {
            _usuarioService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            var usuarios = await _usuarioService.GetUsuario();
            return Ok(usuarios);
        }

        [HttpPost]
        public async Task<ActionResult<Usuario>> CriarUsuario([FromBody] Usuario usuario)
        {
            var novoUsuario = await _usuarioService.CriarUsuario(usuario);

            return CreatedAtAction(nameof(GetUsuarios), new { id = novoUsuario.Id }, novoUsuario);
        }
    }
}
