using Fiap.Web.Alunos.Data.Contexts;
using FiapWebAluno.Service.Interface;
using FiapWebAluno.Views.Common;
using FiapWebAluno.Views.SensorUmidade;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiapWebAluno.Service.Implementations
{
    public class SensorService : ISensorService
    {
        private readonly DatabaseContext _context;

        public SensorService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<PagedResultView<SensorUmidadeListItemView>> ListarAsync(int page, int pageSize)
        {
            var query = _context.SensoresUmidade.AsQueryable();

            var total = await query.CountAsync();

            var items = await query
                .OrderBy(s => s.IdSensor)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(s => new SensorUmidadeListItemView
                {
                    IdSensor = s.IdSensor,
                    IdCanteiro = s.IdCanteiro,
                    DataHora = s.DataHora,
                    PercentualHumidade = s.PercentualUmidade
                })
                .ToListAsync();

            return new PagedResultView<SensorUmidadeListItemView>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalItems = total
            };
        }

        Task<PagedResultView<SensorUmidadeListItemView>> ISensorService.ListarAsync(int page, int pageSize)
        {
            throw new NotImplementedException();
        }
    }
}
