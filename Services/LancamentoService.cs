using Financas.Api.Data;
using Financas.Api.DTOs;
using Financas.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Financas.Api.Services
{
    public class LancamentoService
    {
        private readonly FinancasDbContext _financasDbContext;

        public LancamentoService(FinancasDbContext financasDbContext)
        {
            _financasDbContext = financasDbContext;
        }

        public async Task<LancamentoResponseDTO> CriarLancamento(CriarLancamentoDTO dto, int usuarioId)
        {

            var usuario = await _financasDbContext.Usuarios
                .FirstOrDefaultAsync(u => u.Id == usuarioId);

            if (usuario == null)
                throw new Exception("Usuário não encontrado");
            

            var lancamento = new Lancamento
            {
                Descricao = dto.Descricao,
                Valor = dto.Valor,
                Data = dto.Data,
                Tipo = dto.Tipo,
                UsuarioId = usuarioId
            };

            _financasDbContext.Lancamentos.Add(lancamento);
            await _financasDbContext.SaveChangesAsync();

            return new LancamentoResponseDTO
            {
                Id = lancamento.Id,
                UsuarioId = usuarioId,
                Descricao = lancamento.Descricao,
                Valor = lancamento.Valor,
                Data = lancamento.Data,
                Tipo = lancamento.Tipo.ToString()
            };
        }

        public async Task<List<LancamentoResponseDTO>> GetLancamentos(int usuarioId)
        {
            var lancamentos = await _financasDbContext.Lancamentos
                .Where(l => l.UsuarioId == usuarioId)
                .ToListAsync();

            return lancamentos.Select(l => new LancamentoResponseDTO
            {
                Id = l.Id,
                UsuarioId = l.UsuarioId,
                Descricao = l.Descricao,
                Valor = l.Valor,
                Data = l.Data,
                Tipo = l.Tipo.ToString()
            }).ToList();
        }
    }
}
