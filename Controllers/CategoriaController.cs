using Financas.Api.DTOs.Categoria;
using Financas.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Financas.Api.Controllers
{
    [ApiController] // Atributo que decora a classe para habilitar comportamentos automáticos de API.
    [Route("api/categorias")] // Define o template de rota global para este recurso.
    public class CategoriaController : ControllerBase // Herança da classe base de controllers do ASP.NET.
    {
        // Campo privado de leitura exclusiva (readonly). 
        // Armazena a referência da instância do serviço durante o tempo de vida do Controller.
        private readonly CategoriaService _categoriaService;

        // Construtor da classe CategoriaController.
        // O motor de DI do ASP.NET Core localiza a implementação de 'CategoriaService' 
        // registrada no Program.cs e a injeta automaticamente no momento da instanciação.
        public CategoriaController(CategoriaService categoriaService)
        {
            // Atribuição da instância injetada ao campo privado.
            // O prefixo '_' é uma convenção técnica para distinguir campos de classe de variáveis locais.
            _categoriaService = categoriaService;
        }

        [HttpPost] // Define que o método responde apenas ao verbo HTTP POST (criação).
        [Authorize] // Middleware de segurança: valida o Token JWT antes de permitir a execução do código.
        public async Task<ActionResult<CategoriaResponseDTO>> CriarCategoria([FromBody] CriarCategoriaDTO dto)
        {
            try
            {
                // 1. Extração de Identidade: Recupera o 'Subject' (ID) do usuário autenticado.
                // O método FindFirstValue busca nas Claims do Token o identificador único do usuário logado.
                var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

                // 2. Delegação: Transfere os dados recebidos no corpo da requisição (JSON) 
                // para o serviço, injetando o contexto do usuário para garantir o vínculo correto.
                var categoria = await _categoriaService.CriarCategorias(dto, usuarioId);

                // 3. Resposta REST Padrão (201 Created):
                // 'CreatedAtAction' gera uma resposta com status 201 e inclui no cabeçalho 'Location' 
                // a URL onde o novo recurso pode ser acessado, além de retornar o objeto criado no corpo.
                return CreatedAtAction(nameof(GetCategorias), new { id = categoria.Id }, categoria);
            }
            catch (Exception ex)
            {
                // 4. Fallback de Erro: Captura falhas de negócio ou infraestrutura e as traduz 
                // em um status 400 (Bad Request), expondo a mensagem de exceção no corpo da resposta.
                return BadRequest(ex.Message);
            }
        }

        [HttpGet] // Define que este método responde apenas ao verbo HTTP GET (leitura).
        [Authorize] // Bloqueia o acesso de usuários não autenticados via middleware JWT.
        public async Task<ActionResult<IEnumerable<CategoriaResponseDTO>>> GetCategorias()
        {
            try
            {
                // 1. Captura de Contexto: Extrai o ID do usuário do Token JWT assinado.
                // Isso garante que a consulta seja filtrada automaticamente pelo dono dos dados.
                var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

                // 2. Processamento: Solicita ao serviço a lista de categorias.
                // O retorno é uma coleção (IEnumerable) de DTOs para evitar exposição de entidades.
                var categoriasAtualizadas = await _categoriaService.GetCategorias(usuarioId);

                // 3. Resposta de Sucesso: Retorna o status 200 (OK) com a lista serializada em JSON.
                return Ok(categoriasAtualizadas);
            }
            catch (Exception ex)
            {
                // 4. Tratamento de Erro: Captura falhas inesperadas e retorna status 400 (Bad Request).
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id}")] // Verbo para atualizações parciais. O '{id}' é um parâmetro de rota.
        [Authorize] // Bloqueia o acesso de usuários não autenticados.
        public async Task<ActionResult<CategoriaResponseDTO>> AtualizarCategoria([FromBody] AtualizarCategoriaDTO dto, int id)
        {
            try
            {
                // 1. Contexto de Segurança: Extrai a identidade do usuário logado via Claims.
                var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

                // 2. Orquestração: Envia o DTO (corpo), o ID (rota) e o usuário (token) para o serviço.
                var categoriaAtualizada = await _categoriaService.AtualizarCategoria(dto, id, usuarioId);

                // 3. Sucesso: Retorna 200 (OK) com o objeto já modificado.
                return Ok(categoriaAtualizada);
            }
            catch (KeyNotFoundException ex)
            {
                // 4. Erro de Identificação (404): Capturado se o ID fornecido não existir no banco.
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException)
            {
                // 5. Erro de Permissão (403): Capturado se a categoria existir, mas pertencer a outro usuário.
                // O status 'Forbid' indica que o servidor entendeu o pedido, mas se recusa a autorizá-lo.
                return Forbid();
            }
            catch (Exception ex)
            {
                // 6. Erro Genérico (400): Captura outras falhas (ex: validações de banco ou servidor).
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")] // Define o verbo HTTP DELETE e captura o ID do recurso na URL.
        [Authorize] // Exige autenticação JWT.
        public async Task<ActionResult> DeletarCategoria(int id)
        {
            try
            {
                // 1. Contexto de Segurança: Recupera o ID do usuário autenticado para garantir 
                // que ele só possa deletar suas próprias categorias.
                var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

                // 2. Execução: Chama o serviço para realizar a remoção lógica ou física.
                // O serviço validará se a categoria existe e se pertence ao usuário.
                await _categoriaService.DeletarCategoria(id, usuarioId);

                // 3. Resposta de Sucesso (204 No Content): 
                // Indica que a ação foi concluída com sucesso e não há conteúdo a ser retornado.
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                // 4. Erro de Identificação (404): Retornado se o ID da categoria não existir.
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException)
            {
                // 5. Erro de Permissão (403): Retornado se o usuário tentar deletar um recurso de terceiros.
                return Forbid();
            }
            catch (Exception ex)
            {
                // 6. Erro de Conflito ou Integridade (400): 
                // Pode ocorrer se a categoria estiver vinculada a outros registros (ex: Lançamentos)
                // e houver uma restrição de chave estrangeira (FK) no MySQL.
                return BadRequest(ex.Message);
            }
        }
    }
}
