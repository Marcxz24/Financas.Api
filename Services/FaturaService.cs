using Financas.Api.Data;
using Financas.Api.DTOs.Fatura;
using Financas.Api.Entities;
using Financas.Api.Entities.Enums;
using Microsoft.EntityFrameworkCore;

namespace Financas.Api.Services
{
    public class FaturaService
    {
        private readonly FinancasDbContext _financasDbContext;

        public FaturaService(FinancasDbContext financasDbContext)
        {
            _financasDbContext = financasDbContext;
        }

        public async Task<FaturaResponseDTO> ObterOuCriarFaturaAtual(int cartaoId, int usuarioId, DateTime dataCompra)
        {
            var fatura = await ObterOuCriarFaturaAtualEntidade(cartaoId, usuarioId, dataCompra);

            return new FaturaResponseDTO
            {
                Id = fatura.Id,
                CartaoCreditoId = fatura.CartaoCreditoId,
                DataInicio = fatura.DataInicio,
                DataFechamento = fatura.DataFechamento,
                DataVencimento = fatura.DataVencimento,
                ValorTotal = fatura.ValorTotal,
                ValorPago = fatura.ValorPago,
                Status = fatura.Status.ToString()
            };
        }

        public async Task<Fatura> ObterOuCriarFaturaAtualEntidade(int cartaoId, int usuarioId, DateTime dataCompra)
        {
            var cartao = await _financasDbContext.CartaoCredito
                .FirstOrDefaultAsync(c => c.Id == cartaoId && c.UsuarioId == usuarioId);

            if (cartao == null)
                throw new KeyNotFoundException("Cartão não encontrado.");

            var diaFechamento = cartao.DiaFechamento;
            var diaVencimento = cartao.DiaVencimento;

            var referencia = dataCompra.Day > diaFechamento
                ? dataCompra.AddMonths(1)
                : dataCompra;

            var ano = referencia.Year;
            var mes = referencia.Month;

            var ultimoDiaMes = DateTime.DaysInMonth(ano, mes);

            var dataFechamento = new DateTime(ano, mes, Math.Min(diaFechamento, ultimoDiaMes));
            var dataInicio = dataFechamento.AddMonths(-1).AddDays(1);
            var dataVencimento = new DateTime(ano, mes, Math.Min(diaVencimento, ultimoDiaMes));

            var fatura = await _financasDbContext.Fatura
                .FirstOrDefaultAsync(f =>
                    f.CartaoCreditoId == cartaoId &&
                    f.DataInicio == dataInicio &&
                    f.DataFechamento == dataFechamento);

            if (fatura == null)
            {
                fatura = new Fatura
                {
                    CartaoCreditoId = cartaoId,
                    DataInicio = dataInicio,
                    DataFechamento = dataFechamento,
                    DataVencimento = dataVencimento,
                    ValorTotal = 0,
                    ValorPago = 0,
                    Status = FaturaStatus.Aberta
                };

                _financasDbContext.Fatura.Add(fatura);
                await _financasDbContext.SaveChangesAsync();
            }

            return fatura;
        }
    }
}
