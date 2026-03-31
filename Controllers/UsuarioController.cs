using Financas.Api.DTOs;
using Financas.Api.Entities;
using Financas.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Financas.Api.Controllers
{
    // O controlador de usuários é responsável por lidar com as requisições relacionadas aos usuários, como criação e recuperação de usuários. Ele utiliza o serviço de usuário para realizar as operações necessárias e retornar as respostas adequadas para o cliente.
    [ApiController]
    [Route("api/usuarios")]
    public class UsuarioController : ControllerBase
    {
        // O controlador de usuários depende do serviço de usuário para realizar as operações relacionadas aos usuários. A injeção de dependência é utilizada para fornecer uma instância do serviço de usuário ao controlador, permitindo que ele interaja com a lógica de negócios e o banco de dados para criar e recuperar usuários.
        private readonly UsuarioService _usuarioService;

        // O controlador de usuários depende do serviço de autenticação para lidar com as operações relacionadas à autenticação dos usuários, como login e geração de tokens JWT. A injeção de dependência é utilizada para fornecer uma instância do serviço de autenticação ao controlador, permitindo que ele interaja com a lógica de negócios relacionada à autenticação e acesse as funcionalidades necessárias para autenticar os usuários e gerar tokens JWT.
        private readonly AuthService _authService;

        // O construtor do controlador recebe o serviço de usuário via injeção de dependência, permitindo que o controlador interaja com a lógica de negócios e o banco de dados para criar e recuperar usuários.
        public UsuarioController(UsuarioService userService, AuthService authService)
        {
            _usuarioService = userService;
            _authService = authService;
        }

        // O método GetUsuarios é responsável por lidar com as requisições GET para recuperar a lista de usuários. Ele chama o serviço de usuário para obter os usuários e retorna a resposta adequada para o cliente.
        [HttpGet]
        [Authorize] // O atributo [Authorize] é utilizado para proteger o endpoint de recuperação de usuários, garantindo que apenas usuários autenticados possam acessar essa funcionalidade. Ele verifica se o usuário está autenticado antes de permitir o acesso ao método GetUsuarios, garantindo a segurança dos dados dos usuários.
        public async Task<ActionResult<IEnumerable<UsuarioResponseDTO>>> GetUsuarios()
        {
            // O método GetUsuarios é responsável por lidar com as requisições GET para recuperar a lista de usuários. Ele chama o serviço de usuário para obter os usuários e retorna a resposta adequada para o cliente.
            var usuarios = await _usuarioService.GetUsuario();
            return Ok(usuarios);
        }

        // O método CriarUsuario é responsável por lidar com as requisições POST para criar um novo usuário. Ele recebe os dados do usuário no corpo da requisição, chama o serviço de usuário para criar o usuário e retorna a resposta adequada para o cliente.
        [HttpPost]
        public async Task<ActionResult<UsuarioResponseDTO>> CriarUsuario([FromBody] CriarUsuarioDTO dto)
        {
            // O método CriarUsuario é responsável por lidar com as requisições POST para criar um novo usuário. Ele recebe os dados do usuário no corpo da requisição, chama o serviço de usuário para criar o usuário e retorna a resposta adequada para o cliente.
            try
            {
                // O método CriarUsuario é responsável por lidar com as requisições POST para criar um novo usuário. Ele recebe os dados do usuário no corpo da requisição, chama o serviço de usuário para criar o usuário e retorna a resposta adequada para o cliente.
                var usuario = await _usuarioService.CriarUsuario(dto);
                return Ok(usuario);
            }
            // O método CriarUsuario é responsável por lidar com as requisições POST para criar um novo usuário. Ele recebe os dados do usuário no corpo da requisição, chama o serviço de usuário para criar o usuário e retorna a resposta adequada para o cliente. Se ocorrer algum erro durante a criação do usuário, ele captura a exceção e retorna uma resposta de erro apropriada para o cliente.
            catch (Exception ex)
            {
                // O método CriarUsuario é responsável por lidar com as requisições POST para criar um novo usuário. Ele recebe os dados do usuário no corpo da requisição, chama o serviço de usuário para criar o usuário e retorna a resposta adequada para o cliente. Se ocorrer algum erro durante a criação do usuário, ele captura a exceção e retorna uma resposta de erro apropriada para o cliente.
                return BadRequest(ex.Message);
            }
        }

        // O método Login é responsável por lidar com as requisições POST para autenticar um usuário. Ele recebe os dados de login no corpo da requisição, chama o serviço de autenticação para realizar a autenticação e retorna a resposta adequada para o cliente.
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginDTO dto)
        {
            // O método Login é responsável por lidar com as requisições POST para autenticar um usuário. Ele recebe os dados de login no corpo da requisição, chama o serviço de autenticação para realizar a autenticação e retorna a resposta adequada para o cliente. Se a autenticação for bem-sucedida, ele retorna um token JWT para o cliente. Caso contrário, ele retorna uma resposta de erro indicando que as credenciais são inválidas.
            try
            {
                // O método Login é responsável por lidar com as requisições POST para autenticar um usuário. Ele recebe os dados de login no corpo da requisição, chama o serviço de autenticação para realizar a autenticação e retorna a resposta adequada para o cliente. Se a autenticação for bem-sucedida, ele retorna um token JWT para o cliente. Caso contrário, ele retorna uma resposta de erro indicando que as credenciais são inválidas.
                var token = await _authService.Login(dto);
                return Ok(token);
            }
            catch
            {
                // O método Login é responsável por lidar com as requisições POST para autenticar um usuário. Ele recebe os dados de login no corpo da requisição, chama o serviço de autenticação para realizar a autenticação e retorna a resposta adequada para o cliente. Se a autenticação for bem-sucedida, ele retorna um token JWT para o cliente. Caso contrário, ele retorna uma resposta de erro indicando que as credenciais são inválidas.
                return Unauthorized("Usuário ou senha inválidos");
            }
        }
    }
}
