using Financas.Api.Data;
using Financas.Api.DTOs.Dashboard;
using Financas.Api.DTOs.Lancamento;
using Financas.Api.Entities.Enums;
using Microsoft.EntityFrameworkCore;

namespace Financas.Api.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly FinancasDbContext _financasDbContext;

        // Injeção de dependência do contexto do banco de dados
        public DashboardService(FinancasDbContext financasDbContext)
        {
            _financasDbContext = financasDbContext;
        }

        public async Task<DashboardResumoResponseDto> GetResumoMensalAsync(int mes, int ano, int usuarioId)
        {
            // Filtra os lançamentos base por usuário e período (mês/ano) para reutilização na lógica abaixo
            var lancamento = _financasDbContext.Lancamentos
                .Where(l => l.UsuarioId == usuarioId &&
                    l.Data.Month == mes &&
                    l.Data.Year == ano);

            // Soma assíncrona de todos os lançamentos do tipo Receita no período
            var totalReceitas = await lancamento
                .Where(l => l.Tipo == TipoLancamento.Receita)
                .SumAsync(l => l.Valor);

            // Soma assíncrona de todos os lançamentos do tipo Despesa no período
            var totalDespesas = await lancamento
                .Where(l => l.Tipo == TipoLancamento.Despesa)
                .SumAsync(l => l.Valor);

            // Calcula o saldo somando todas as contas bancárias cadastradas no sistema
            var saldoBancarioTotal = await _financasDbContext.ContasBancarias
                .SumAsync(c => c.Saldo);

            // Busca os 5 lançamentos mais recentes, projetando para o DTO de resumo
            var ultimosLancamentos = await lancamento
                .OrderByDescending(l => l.Data)
                .Take(5)
                .Select(l => new LancamentoResumoDTO
                {
                    Descricao = l.Descricao,
                    Valor = l.Valor,
                    DataLancamento = l.Data,
                    Tipo = l.Tipo.ToString() // Converte o Enum para String para o Front-end
                })
                .ToListAsync();

            // Retorna o objeto consolidado para o Controller enviar ao React
            return new DashboardResumoResponseDto
            {
                TotalReceitas = totalReceitas,
                TotalDespesas = totalDespesas,
                SaldoMensal = totalReceitas - totalDespesas,
                SaldoBancarioTotal = saldoBancarioTotal,
                PeriodoReferencia = $"{mes:D2}/{ano}", // Formata mês com dois dígitos (ex: 01, 02)
                UltimosLancamentos = ultimosLancamentos
            };
        }
    }
}