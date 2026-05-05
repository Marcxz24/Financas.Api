using Financas.Api.Data;
using Financas.Api.DTOs.Dashboard;

namespace Financas.Api.Services
{
    /// <summary>
    /// Define o contrato para as operações de Dashboard.
    /// Segue o princípio de inversão de dependência (SOLID).
    /// </summary>
    public interface IDashboardService
    {
        /// <summary>
        /// Define a assinatura para busca de resumo financeiro mensal.
        /// </summary>
        /// <param name="mes">Mês de referência (1-12)</param>
        /// <param name="ano">Ano de referência</param>
        /// <param name="usuarioId">ID do usuário logado para filtro de segurança</param>
        /// <returns>Retorna um DTO com os totais e lançamentos do período</returns>
        Task<DashboardResumoResponseDto> GetResumoMensalAsync(int mes, int ano, int usuarioId);
    }
}